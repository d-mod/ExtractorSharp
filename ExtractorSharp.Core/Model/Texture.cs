using System.Drawing;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Core.Model {
    /// <summary>
    ///     DDS文件信息
    /// </summary>
    public sealed class Texture {

        private Bitmap _image;

        private ImageData _imageData;

        public int Index { set; get; }

        public int Width { set; get; } = 4;

        public int Height { set; get; } = 4;

        public int Length { set; get; }

        public int FullLength { set; get; }

        public byte[] Data { set; get; }

        public TextureVersion Version { set; get; } = TextureVersion.DXT1;

        public ColorFormats Type { set; get; } = ColorFormats.DDS_DXT1;

        public Bitmap Pictrue {
            get {
                if(this._image != null) {
                    return this._image;
                }
                return this.ImageData.ToBitmap();
            }
            set => this._image = value;
        }

        public ImageData ImageData {
            get {
                if(this._imageData != null) {
                    return this._imageData;
                }

                var data = Zlib.Decompress(this.Data, this.FullLength);
                if(this.Type < ColorFormats.LINK) {
                    data = Bitmaps.ConvertTo32Bits(data, this.Type);
                    return new ImageData(data, this.Width, this.Height);
                }
                var dds = DdsDecoder.Decode(data);
                data = dds.DdsMipmaps[0].Data;
                return this._imageData = new ImageData(data, this.Width, this.Height);
            }
        }

        public static Texture CreateFromSprite(Sprite sprite) {
            var bmp = sprite.Image;
            var type = sprite.ColorFormat;
            if(type > ColorFormats.LINK) {
                type -= 4;
            }

            var data = bmp.ToArray(type);
            var fullLength = data.Length;
            var width = bmp.Width;
            var height = bmp.Height;
            data = Zlib.Compress(data);
            var dds = new Texture {
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

    public class TextureInfo {
        public Texture Texture { set; get; }

        /// <summary>
        ///     左上角顶点坐标
        /// </summary>
        public Point LeftUp { set; get; }

        /// <summary>
        ///     右下角顶点坐标
        /// </summary>
        public Point RightDown { set; get; }

        /// <summary>
        ///     大小
        /// </summary>
        public Size Size => new Size(this.RightDown.X - this.LeftUp.X, this.RightDown.Y - this.LeftUp.Y);

        public int Top { set; get; }

        public Rectangle Rectangle => new Rectangle(this.LeftUp, this.Size);

        public int Unknown { get; set; }
    }

    public enum TextureVersion {

        DXT1 = 0x01,

        DXT3 = 0X03,

        DXT5 = 0x05

    }
}