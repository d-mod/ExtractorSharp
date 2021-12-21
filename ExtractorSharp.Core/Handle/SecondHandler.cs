using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    public class SecondHandler : Handler {
        public SecondHandler(Album album) : base(album) { }

        public override ImageData GetImageData(Sprite sprite) {
            var data = sprite.Data;
            var type = sprite.ColorFormat;
            var width = sprite.Width;
            var height = sprite.Height;
            var size = width * height * (type == ColorFormats.ARGB_8888 ? 4 : 2);
            if(sprite.CompressMode == CompressMode.ZLIB) {
                data = Zlib.Decompress(data, size);
            }
            data = Bitmaps.ConvertTo32Bits(data, type);
            return new ImageData(data, width, height);
        }

        public override byte[] ConvertToByte(Sprite sprite) {
            if(sprite.ColorFormat > ColorFormats.LINK) {
                sprite.ColorFormat -= 4;
            }
            if(sprite.CompressMode > CompressMode.ZLIB) {
                sprite.CompressMode = CompressMode.ZLIB;
            }
            var data = sprite.ImageData.Data;
            var bits = sprite.ColorFormat;
            data = Bitmaps.ConvertToBits(data, bits);
            return data;
        }

        public override byte[] AdjustData() {
            using(var ms = new MemoryStream()) {
                foreach(var entity in this.Album.List) {
                    ms.WriteInt((int)entity.ColorFormat);
                    if(entity.IsLink) {
                        ms.WriteInt(entity.TargetIndex);
                        continue;
                    }
                    ms.WriteInt((int)entity.CompressMode);
                    ms.WriteInt(entity.Width);
                    ms.WriteInt(entity.Height);
                    ms.WriteInt(entity.Length);
                    ms.WriteInt(entity.X);
                    ms.WriteInt(entity.Y);
                    ms.WriteInt(entity.FrameWidth);
                    ms.WriteInt(entity.FrameHeight);
                }
                this.Album.IndexLength = ms.Length;
                foreach(var sprite in this.Album.List) {
                    if(sprite.ColorFormat == ColorFormats.LINK) {
                        continue;
                    }
                    ms.Write(sprite.Data);
                }
                return ms.ToArray();
            }
        }

        public override void CreateFromStream(Stream stream) {
            var pos = stream.Position + this.Album.IndexLength;
            for(var i = 0; i < this.Album.Count; i++) {
                var image = new Sprite(this.Album) {
                    Index = this.Album.List.Count,
                    ColorFormat = (ColorFormats)stream.ReadInt()
                };
                this.Album.List.Add(image);
                if(image.ColorFormat == ColorFormats.LINK) {
                    image.TargetIndex = stream.ReadInt();
                    continue;
                }
                image.CompressMode = (CompressMode)stream.ReadInt();
                image.Width = stream.ReadInt();
                image.Height = stream.ReadInt();
                image.Length = stream.ReadInt();
                image.X = stream.ReadInt();
                image.Y = stream.ReadInt();
                image.FrameWidth = stream.ReadInt();
                image.FrameHeight = stream.ReadInt();
            }
            if(stream.Position < pos) {
                this.Album.List.Clear();
                return;
            }
            foreach(var image in this.Album.List.ToArray()) {
                if(image.IsLink) {
                    continue;
                }
                if(image.CompressMode == CompressMode.NONE) {
                    image.Length = image.Width * image.Height * (image.ColorFormat == ColorFormats.ARGB_8888 ? 4 : 2);
                }
                var data = new byte[image.Length];
                stream.Read(data);
                image.Data = data;
            }
        }

        public override void ConvertToVersion(ImgVersion version) {
            if(version == ImgVersion.Ver4 || version == ImgVersion.Ver6) {
                this.Album.List.ForEach(item => item.ColorFormat = ColorFormats.ARGB_1555);
            }
        }

    }
}