using System;
using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Data {
    /// <summary>
    /// DDS文件信息
    /// </summary>
    public class DDS {
        public int Index;
        public int Width;
        public int Height;
        public int Size;
        public int DDS_Size;
        public byte[] Data;
        public DDS_Version Version;
        public ColorBits Type = ColorBits.DXT_1;
        public Bitmap Pictrue {
            get {
                if (image != null)
                    return image;
                var data = FreeImage.Decompress(Data, DDS_Size);
                if (Type < ColorBits.DXT_1) {
                    return Bitmaps.FromArray(data, new Size(Width, Height), Type);
                }
                var bmp = FreeImage.Load(data, new Size(Width, Height));
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                return bmp;
            }
            set => image = value;
        }

        private Bitmap image;

        internal static DDS CreateFromBitmap(Bitmap picture, Compress compress) => throw new NotImplementedException();
    }

    public class DDS_Info {
        public DDS DDS;
        /// <summary>
        /// 左上角顶点坐标
        /// </summary>
        public Point LeftUp;
        /// <summary>
        /// 右下角顶点坐标
        /// </summary>
        public Point RightDown;
        /// <summary>
        /// 大小
        /// </summary>
        public Size Size => new Size(RightDown.X - LeftUp.X, RightDown.Y - LeftUp.Y);

        public int Top;
        
        public Rectangle Rectangle => new Rectangle(LeftUp, Size);
    }

    public enum DDS_Version {
        DXT1 = 0x01,
        DXT3 = 0X03,
        DXT5 = 0x05,
    }
}
