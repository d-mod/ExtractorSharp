using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ExtractorSharp.Core.Lib {
    public static class Bitmaps {

        /// <summary>
        /// 图片扫描
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Rectangle Scan(this Bitmap bmp) {
            var up = bmp.Height;
            var down = 0;
            var left = bmp.Width;
            var right = 0;
            var data = bmp.ToArray();
            for (var i = 0; i < bmp.Width; i++) {
                for (var j = 0; j < bmp.Height; j++)
                    for (var k = 0; k < 4; k++) {
                        var tp = data[(j * bmp.Width + i) * 4 + k];
                        if (tp != 0) {
                            up = up > j ? j : up;
                            left = left > i ? i : left;
                            down = down < j ? j : down;
                            right = right < i ? i : right;
                            break;
                        }
                    }
            }
            var width = Math.Abs(right - left + 1);
            var height = Math.Abs(down - up + 1);
            return new Rectangle(left, up, width, height);
        }


        public static Bitmap Canvas(this Bitmap bmp, Rectangle rect) {
            var image = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(image)) {
                g.DrawImage(bmp, rect.X, rect.Y);
            }
            return image;
        }



        public static Bitmap LinearDodge(this Bitmap bmp) {
            var data = bmp.ToArray();
            for (var i = 0; i < data.Length; i += 4) {
                var r = data[i];
                var g = data[i + 1];
                var b = data[i + 2];
                var a = data[i + 3];
                if (r + (g + b + a) / 2 < 0xff) {
                    a = (byte)(a >> 6 & a << 3);
                    g = (byte)(g << 1 & g >> 2);
                    b = (byte)(b << 2 & b >> 3);
                }
                data[i] = r;
                data[i + 1] = g;
                data[i + 2] = b;
                data[i + 3] = a;
            }
            bmp = FromArray(data, bmp.Size);
            return bmp;
        }

        public static Bitmap Star(this Bitmap bmp, decimal scale) {
            var size = bmp.Size.Star(scale);
            var image = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(image)) {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(bmp, new Rectangle(Point.Empty, size));
            }
            return image;
        }

        /// <summary>
        /// 将Bitmap转换为rgb数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] ToArray(this Bitmap bmp) {
            ToArray(bmp,out byte[] data);
            return data;
        }

        public static void ToArray(this Bitmap bmp,out byte[] data) {
            data = new byte[bmp.Width * bmp.Height * 4];
            var bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(bmpData.Scan0, data, 0, data.Length);
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// 将rgb数组转换为Bitmap
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap FromArray(byte[] data, Size size) {
            var bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(Point.Empty, size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
