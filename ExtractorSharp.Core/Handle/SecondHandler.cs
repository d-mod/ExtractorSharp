using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    public class SecondHandler : Handler {
        public SecondHandler(Album album) : base(album) { }

        public override Bitmap ConvertToBitmap(Sprite entity) {
            var data = entity.Data;
            var type = entity.Type;
            var size = entity.Width * entity.Height * (type == ColorBits.Argb8888 ? 4 : 2);
            if (entity.CompressMode == CompressMode.Zlib) data = Zlib.Decompress(data, size);
            return Bitmaps.FromArray(data, entity.Size, type);
        }

        public override byte[] ConvertToByte(Sprite entity) {
            if (entity.Type > ColorBits.Link) entity.Type -= 4;
            if (entity.CompressMode > CompressMode.Zlib) entity.CompressMode = CompressMode.Zlib;
            return entity.Picture.ToArray(entity.Type);
        }

        public override void NewImage(int count, ColorBits type, int index) {
            if (count < 1) return;
            var array = new Sprite[count];
            array[0] = new Sprite(Album) {
                Index = index
            };
            if (type != ColorBits.Link) array[0].Type = type;
            for (var i = 1; i < count; i++) {
                array[i] = new Sprite(Album) {
                    Type = type
                };
                if (type == ColorBits.Link) array[i].Target = array[0];
                array[i].Index = index + i;
            }
            Album.List.InsertAt(index, array);
        }

        public override byte[] AdjustData() {
            using (var ms = new MemoryStream()) {
                foreach (var entity in Album.List) {
                    ms.WriteInt((int) entity.Type);
                    if (entity.Type == ColorBits.Link && entity.Target != null) {
                        ms.WriteInt(entity.Target.Index);
                        continue;
                    }
                    ms.WriteInt((int) entity.CompressMode);
                    ms.WriteInt(entity.Width);
                    ms.WriteInt(entity.Height);
                    ms.WriteInt(entity.Length);
                    ms.WriteInt(entity.X);
                    ms.WriteInt(entity.Y);
                    ms.WriteInt(entity.CanvasWidth);
                    ms.WriteInt(entity.CanvasHeight);
                }
                Album.IndexLength = ms.Length;
                foreach (var entity in Album.List) {
                    if (entity.Type == ColorBits.Link) continue;
                    ms.Write(entity.Data);
                }
                return ms.ToArray();
            }
        }

        public override void CreateFromStream(Stream stream) {
            var dic = new Dictionary<Sprite, int>();
            var pos = stream.Position + Album.IndexLength;
            for (var i = 0; i < Album.Count; i++) {
                var image = new Sprite(Album);
                image.Index = Album.List.Count;
                image.Type = (ColorBits) stream.ReadInt();
                Album.List.Add(image);
                if (image.Type == ColorBits.Link) {
                    dic.Add(image, stream.ReadInt());
                    continue;
                }
                image.CompressMode = (CompressMode) stream.ReadInt();
                image.Width = stream.ReadInt();
                image.Height = stream.ReadInt();
                image.Length = stream.ReadInt();
                image.X = stream.ReadInt();
                image.Y = stream.ReadInt();
                image.CanvasWidth = stream.ReadInt();
                image.CanvasHeight = stream.ReadInt();
            }
            if (stream.Position < pos) {
                Album.List.Clear();
                return;
            }
            foreach (var image in Album.List.ToArray()) {
                if (image.Type == ColorBits.Link) {
                    if (dic.ContainsKey(image) && dic[image] < Album.List.Count && dic[image] > -1 &&
                        dic[image] != image.Index) {
                        image.Target = Album.List[dic[image]];
                        image.Size = image.Target.Size;
                        image.CanvasSize = image.Target.CanvasSize;
                        image.Location = image.Target.Location;
                    } else {
                        Album.List.Clear();
                        return;
                    }
                    continue;
                }
                if (image.CompressMode == CompressMode.None) {
                    image.Length = image.Width * image.Height * (image.Type == ColorBits.Argb8888 ? 4 : 2);
                }
                var data = new byte[image.Length];
                stream.Read(data);
                image.Data = data;
            }
        }

        public override void ConvertToVersion(ImgVersion version) {
            if (version == ImgVersion.Ver4 || version == ImgVersion.Ver6) {
                Album.List.ForEach(item => item.Type = ColorBits.Argb1555);
            }
        }
    }
}