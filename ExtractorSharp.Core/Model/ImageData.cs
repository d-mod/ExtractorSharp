using System;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtractorSharp.Core.Model {
    public class ImageData {


        public ImageData(byte[] data, int width, int height) {
            this.Data = data;
            this.Width = width;
            this.Height = height;
        }

        public byte[] Data { get; }

        public int Width { get; }

        public int Height { get; }


        public ImageSource ToImageSource() {
            return BitmapSource.Create(this.Width, this.Height, 0, 0, PixelFormats.Bgra32, null, this.Data, 4 * this.Width);
        }

        public Bitmap ToBitmap() {
            return Bitmaps.FromArray(this.Data, new Size(this.Width, this.Height));
        }


        public ImageData Clone(Rectangle rectangle) {
            var newData = new byte[rectangle.Width * rectangle.Height * 4];
            for(var i = rectangle.Y; i < rectangle.Height; i++) {
                var stride = rectangle.Width * 4;
                var sourceIndex = (i * this.Width + rectangle.X) * 4;
                var targetIndex = i * rectangle.Width * 4;
                Array.Copy(this.Data, sourceIndex, newData, targetIndex, stride);
            }
            return new ImageData(newData, rectangle.Width, rectangle.Height);
        }

        public void Draw(ImageData image) {

        }


        public int[] ToPixels() {
            var pixels = new int[this.Width * this.Height];
            for(var i = 0; i < pixels.Length; i++) {
                var r = (this.Data[i * 4] & 0xff) << 24;
                var g = (this.Data[i * 4 + 1] & 0xff) << 16;
                var b = (this.Data[i * 4 + 2] & 0xff) << 8;
                var a = this.Data[i * 4 + 3];
                pixels[i] = r | g | b | a;
            }
            return pixels;
        }


        public ImageData RotateFlip() {
            var data = this.Data;
            var newData = new byte[data.Length];
            var m = this.Width;
            var n = this.Height;

            var pixels = this.ToPixels();

            var matrix = new int[this.Height][];

            for(var j = 0; j < this.Height; j++) {
                matrix[j] = new int[this.Width];
                Array.Copy(pixels, j * this.Width, matrix[j], 0, this.Width);
            }


            var newMatrix = new int[this.Width][];

            for(var i = 0; i < m; i++) {
                newMatrix[i] = new int[this.Height];

                for(var j = n - 1; j > 0; j--) {
                    newMatrix[i][j] = matrix[j][i];
                }
                Array.Reverse(newMatrix[i]);
            }


            var newPixels = new int[this.Width * this.Height];

            for(var j = 0; j < this.Width; j++) {
                Array.Copy(newMatrix[j], 0, newPixels, j * this.Height, this.Height);
            }

            for(var i = 0; i < newPixels.Length; i++) {
                var r = (byte)((newPixels[i] >> 24) & 0xff);
                var g = (byte)((newPixels[i] >> 16) & 0xff);
                var b = (byte)((newPixels[i] >> 8) & 0xff);
                var a = (byte)(newPixels[i] & 0xff);
                Array.Copy(new byte[] { r, g, b, a }, 0, newData, i * 4, 4);
            }

            return new ImageData(newData, this.Height, this.Width);
        }


        public static readonly ImageData Empty = new ImageData(new byte[4], 1, 1);

        /// <summary>
        /// 创建一个有尺寸的空白图像
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ImageData CreateImageData(int width, int height) {
            return new ImageData(new byte[width * height * 4], width, height);
        }

        public static ImageData CreateByBitmap(Bitmap image) {
            if(image == null) {
                return null;
            }
            var data = image.ToArray();
            return new ImageData(data, image.Width, image.Height);
        }


    }
}
