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
                return FreeImage.Load(data, new Size(Width, Height));
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
        DXT1 = 0x01
    }
}
