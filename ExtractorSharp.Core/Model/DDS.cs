using System;
using System.Drawing;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Data {
    /// <summary>
    /// DDS文件信息
    /// </summary>
    public class DDS {
        public int Index { set; get; }
        public int Width { set; get; } = 4;
        public int Height { set; get; } = 4;
        public int Length { set; get; }
        public int FullLength { set; get; }
        public byte[] Data { set; get; }
        public DDS_Version Version { set; get; } = DDS_Version.DXT1;
        public ColorBits Type { set; get; } = ColorBits.DXT_1;

        public Bitmap Pictrue {
            get {
                if (image != null) {
                    return image;
                }
                var data = Zlib.Decompress(Data, FullLength);
                if (Type < ColorBits.LINK) {
                    return Bitmaps.FromArray(data, new Size(Width, Height), Type);
                }
                var dds = Ddss.Decode(data);
                data = dds.Mipmaps[0].Data;
                var bmp = Bitmaps.FromArray(data, new Size(Width, Height));
                return bmp;
            }
            set => image = value;
        }

        private Bitmap image;

        public static DDS CreateFromBitmap(Sprite sprite) {
            var bmp = sprite.Picture;
            var type = sprite.Type;
            if (type > ColorBits.LINK) {
                type -= 4;
            }
            var data = bmp.ToArray(type);
            var fullLength = data.Length;
            var width = bmp.Width;
            var height = bmp.Height;
            data = Zlib.Compress(data);
            var dds = new DDS {
                Data = data,
                FullLength = fullLength,
                Length = data.Length,
                Width = width,
                Height = height,
                Type = type
            };
            return dds;
        }
    }

    public class DDS_Info {
        public DDS DDS { set; get; }

        /// <summary>
        /// 左上角顶点坐标
        /// </summary>
        public Point LeftUp { set; get; }

        /// <summary>
        /// 右下角顶点坐标
        /// </summary>
        public Point RightDown { set; get; }

        /// <summary>
        /// 大小
        /// </summary>
        public Size Size => new Size(RightDown.X - LeftUp.X, RightDown.Y - LeftUp.Y);

        public int Top { set; get; }

        public Rectangle Rectangle => new Rectangle(LeftUp, Size);

        public int Unknown { get; set; }
    }

    public enum DDS_Version {
        DXT1 = 0x01,
        DXT3 = 0X03,
        DXT5 = 0x05,
    }
}
