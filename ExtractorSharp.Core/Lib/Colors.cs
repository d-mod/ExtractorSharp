using ExtractorSharp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ExtractorSharp.Core.Lib {
    public static class Colors {

        public const int ARGB_1555 = 0x0e;
        public const int ARGB_4444 = 0x0f;
        public const int ARGB_8888 = 0X10;

        /// <summary>
        /// 将所有ARGB类型的数据转换为ARGB_8888的字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte[] ReadColor(Stream stream, int bits) {
            byte[] bs;
            if (bits == ARGB_8888) {
                stream.Read(4, out bs);
                return bs;
            }
            byte a = 0;
            byte r = 0;
            byte g = 0;
            byte b = 0;
            stream.Read(2, out bs);
            switch (bits) {
                case ARGB_1555:
                    a = (byte)(bs[1] >> 7);
                    r = (byte)((bs[1] >> 2) & 0x1f);
                    g = (byte)((bs[0] >> 5) | (bs[1] & 3) << 3);
                    b = (byte)(bs[0] & 0x1f);
                    a = (byte)(a * 0xff);
                    r = (byte)(r << 3 | r >> 2);
                    g = (byte)(g << 3 | g >> 2);
                    b = (byte)(b << 3 | b >> 2);
                    break;
                case ARGB_4444:
                    a = (byte)(bs[1] & 0xf0);
                    r = (byte)((bs[1] & 0xf) << 4);
                    g = (byte)(bs[0] & 0xf0);
                    b = (byte)((bs[0] & 0xf) << 4);
                    break;
                default:
                    break;
            }
            return new byte[] { b, g, r, a };
        }
        public static byte[] ReadColor(Stream stream, ColorBits bits) => ReadColor(stream, (int)bits);
        /// <summary>
        /// 将ARGB1555和ARGB4444转换为ARGB8888
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public static void WriteColor(Stream stream, byte[] data, ColorBits bits) {
            if (bits == ColorBits.ARGB_8888) {
                stream.Write(data);
                return;
            }
            var a = data[3];
            var r = data[2];
            var g = data[1];
            var b = data[0];
            var left = 0;
            var right = 0;
            switch (bits) {
                case ColorBits.ARGB_1555:
                    a = (byte)(a >> 7);
                    r = (byte)(r >> 3);
                    g = (byte)(g >> 3);
                    b = (byte)(b >> 3);
                    left = (byte)((g & 7) << 5 | b);
                    right = (byte)((a << 7) | (r << 2) | (g >> 3));
                    break;
                case ColorBits.ARGB_4444:
                    left = g | b >> 4;
                    right = a | r >> 4;
                    break;
            }
            stream.WriteByte((byte)left);
            stream.WriteByte((byte)right);
        }

        /// <summary>
        /// 写入一个颜色
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="color"></param>
        /// <param name="type"></param>
        public static void WriteColor(this Stream stream, Color color,ColorBits bits) {
            byte[] data = { color.B, color.G, color.R, color.A };
            WriteColor(stream, data, bits);
        }


            /// <summary>
            /// 读取一个指定数量的色表
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public static IEnumerable<Color> ReadPalette(Stream stream, int count) {
            for (var i = 0; i < count; i++) {
                var data = new byte[4];
                stream.Read(data);
                yield return Color.FromArgb(data[3], data[0], data[1], data[2]);
            }
        }







        public static void WritePalette(Stream stream, IEnumerable<Color> table) {
            var list = table.ToList();
            for (var i = 0; i < list.Count; i++) {
                var color = list[i];
                var data = new byte[] { color.R, color.G, color.B, color.A };
                stream.Write(data);
            }
        }
    }
}
