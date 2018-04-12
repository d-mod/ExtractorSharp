using ExtractorSharp.Data;
using Gif.Components;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ExtractorSharp.Core.Lib {
    public static class Bitmaps {



        /// <summary>
        /// 图片扫描
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns>返回实际有色彩数据的最小矩形范围</returns>
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
            var width = Math.Abs(right - left + 1);//最小=1
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


        /// <summary>
        /// 线性减淡
        /// </summary>
        /// <see cref="https://www.cnblogs.com/godzza/p/7428080.html"/>
        /// <param name="bmp"></param>
        /// <returns></returns>LinearBrun
        public static Bitmap LinearDodge(this Bitmap bmp) {
            var data = ToArray(bmp);
            for (var i = 0; i < data.Length; i += 4) {
                var max = Math.Max(data[i], Math.Max(data[i + 1], data[i + 2]));
                var sub = (byte)(0xff - max);
                data[i + 3] = Math.Min(data[i + 3], max);
                data[i + 2] += sub;
                data[i + 1] += sub;
                data[i + 0] += sub;
            }
            bmp = FromArray(data, bmp.Size);
            return bmp;
        }

        /// <summary>
        /// 线性减淡
        /// </summary>
        /// <see cref="https://www.cnblogs.com/godzza/p/7428080.html"/>
        /// <param name="bmp"></param>
        /// <returns></returns>LinearDodge
        public static Bitmap LinearBrun(this Bitmap bmp) {
            var data = ToArray(bmp);
            for (var i = 0; i < data.Length; i += 4) {
                var min = Math.Min(data[i], Math.Max(data[i + 1], data[i + 2]));
                var sub = min;
                data[i + 3] = Math.Max(data[i + 3], min);
                data[i + 2] = Math.Max((byte)0, (byte)(data[i + 2] - min));
                data[i + 1] =Math.Max((byte)0, (byte)(data[i + 1] - min));
                data[i + 0] =Math.Max((byte)0, (byte)(data[i + 2] - min));
            }
            bmp = FromArray(data, bmp.Size);
            return bmp;
        }


        public static Bitmap Star(this Bitmap bmp, decimal scale) {
            var size = bmp.Size.Star(scale);
            var image = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(image)) {
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
            ToArray(bmp, out byte[] data);
            return data;
        }

        public static void ToArray(this Bitmap bmp, out byte[] data) {
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

        /// <summary>
        /// 将rgb数组转换为Bitmap
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap FromArray(byte[] data, Size size, ColorBits bits) {
            var ms = new MemoryStream(data);
            data = new byte[size.Width * size.Height * 4];
            for (var i = 0; i < data.Length; i += 4) {
                var temp = Colors.ReadColor(ms, bits);
                temp.CopyTo(data, i);
            }
            ms.Close();
            return FromArray(data, size);
        }

        public static Bitmap[] ReadGif(string path) {
            GifDecoder decoder = new GifDecoder();
            decoder.Read(path);
            var count = decoder.GetFrameCount();
            var array = new Bitmap[count];
            for (var i = 0; i < count; i++) {
                array[i] = decoder.GetFrame(i) as Bitmap;
            }
            return array;
        }

        public static void WriteGif(string path, Image[] array, Color transparent, int delay = 75) {
            var encoder = new AnimatedGifEncoder();
            encoder.Start();
            encoder.SetDelay(75);
            encoder.SetTransparent(transparent);
            for (var i = 0; i < array.Length; i++) {
                encoder.AddFrame(array[i]);
            }
            encoder.Finish();
            encoder.Output(path);
        }
    }
}
