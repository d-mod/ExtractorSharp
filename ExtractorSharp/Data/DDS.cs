using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using ExtractorSharp.Handle;
using ExtractorSharp.Lib;

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
        public int unknow = 0x12;
        public Bitmap Pictrue {
            get {
                if (image != null)
                    return image;
                var data = FreeImage.Uncompress(Data, DDS_Size);
                if (unknow == 0xe) {
                    var ms = new MemoryStream(data);
                    data = new byte[Width * Height * 4];
                    for (var i = 0; i < data.Length; i += 4) {
                        var temp = ms.ReadColor(ColorBits.ARGB_1555);
                        temp.CopyTo(data, i);
                    }
                    ms.Close();
                    return Tools.FromArray(data, new Size(Width, Height));
                } else {
                    var bmp = FreeImage.Load(data, new Size(Width, Height));
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                    return bmp;
                }
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
        public Size Size;

        public DDS_Info(DDS DDS,Point LeftUp,Point RightDown) {
            this.DDS = DDS;
            this.LeftUp = LeftUp;
            this.RightDown = RightDown;
            Size = new Size(RightDown.X - LeftUp.X, RightDown.Y - LeftUp.Y);
        }
    }

    public enum DDS_Version {
        DXT1 = 0x01,
        DXT3 = 0X03,
        DXT5 = 0x05,
    }
}
