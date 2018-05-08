using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Handle {
    public class SecondHandler : Handler {
        public SecondHandler(Album Album) : base(Album) {}

        public override Bitmap ConvertToBitmap(Sprite entity) {
            var data = entity.Data;
            var type = entity.Type;
            var size = entity.Width * entity.Height * (type == ColorBits.ARGB_8888 ? 4 : 2);
            if (entity.Compress == Compress.ZLIB) {
                data = Zlib.Decompress(data, size);
            }
            return Bitmaps.FromArray(data, entity.Size,type);
        }

        public override byte[] ConvertToByte(Sprite entity) {
            return entity.Picture.ToArray(entity.Type);
        }

        public override void NewImage(int count, ColorBits type, int index) {
            if (count < 1) {
                return;
            }
            var array = new Sprite[count];
            array[0] = new Sprite(Album);
            array[0].Index = index;
            if (type != ColorBits.LINK)
                array[0].Type = type;
            for (var i = 1; i < count; i++) {
                array[i] = new Sprite(Album);
                array[i].Type = type;
                if (type == ColorBits.LINK) {
                    array[i].Target = array[0];
                }
                array[i].Index = index + i;
            }
            Album.List.InsertAt(index, array);
        }

        public override byte[] AdjustData() {
            using (var ms = new MemoryStream()) {
                foreach (var entity in Album.List) {
                    ms.WriteInt((int)entity.Type);
                    if (entity.Type == ColorBits.LINK && entity.Target != null) {
                        ms.WriteInt(entity.Target.Index);
                        continue;
                    }
                    ms.WriteInt((int)entity.Compress);
                    ms.WriteInt(entity.Width);
                    ms.WriteInt(entity.Height);
                    ms.WriteInt(entity.Length);
                    ms.WriteInt(entity.X);
                    ms.WriteInt(entity.Y);
                    ms.WriteInt(entity.Canvas_Width);
                    ms.WriteInt(entity.Canvas_Height);
                }
                Album.Info_Length = ms.Length;
                foreach (var entity in Album.List) {
                    if (entity.Type == ColorBits.LINK) {
                        continue;
                    }
                    ms.Write(entity.Data);
                }
                return ms.ToArray();
            }
        }

        public override void CreateFromStream(Stream stream) {
            var dic = new Dictionary<Sprite, int>();
            var pos = stream.Position + Album.Info_Length;
            for (var i = 0; i < Album.Count; i++) {
                var image = new Sprite(Album);
                image.Index = Album.List.Count;
                image.Type = (ColorBits)stream.ReadInt();
                Album.List.Add(image);
                if (image.Type == ColorBits.LINK) {
                    dic.Add(image, stream.ReadInt());
                    continue;
                }
                image.Compress = (Compress)stream.ReadInt();
                image.Width = stream.ReadInt();
                image.Height = stream.ReadInt();
                image.Length = stream.ReadInt();
                image.X = stream.ReadInt();
                image.Y = stream.ReadInt();
                image.Canvas_Width = stream.ReadInt();
                image.Canvas_Height = stream.ReadInt();
            }
            if (stream.Position < pos) {
                Album.List.Clear();
                return;
            }
            foreach (var image in Album.List.ToArray()) {
                if (image.Type == ColorBits.LINK) {
                    if (dic.ContainsKey(image) && dic[image] < Album.List.Count && dic[image] > -1 && dic[image] != image.Index) {
                        image.Target = Album.List[dic[image]];
                        image.Size = image.Target.Size;
                        image.Canvas_Size = image.Target.Canvas_Size;
                        image.Location = image.Target.Location;
                    } else {
                        Album.List.Clear();
                        return;
                    }
                    continue;
                }
                if (image.Compress == Compress.NONE) {//空帧的长度
                    image.Length = image.Width * image.Height * (image.Type == ColorBits.ARGB_8888 ? 4 : 2);
                }
                var data = new byte[image.Length];
                stream.Read(data);
                image.Data = data;
            }
        }

        public override void ConvertToVersion(Img_Version Version) {
            if (Version == Img_Version.Ver4 || Version == Img_Version.Ver6) {
                Album.List.ForEach(item => item.Type = ColorBits.ARGB_1555);
            }
        }

    }
}
