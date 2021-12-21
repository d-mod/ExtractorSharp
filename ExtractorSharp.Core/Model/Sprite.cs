using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Core.Model {
    public sealed class Sprite : IFormattable, INotifyPropertyChanged {

        private Size _frameSize = Size.Empty;

        /// <summary>
        ///     帧域宽高
        /// </summary>
        public Size FrameSize {
            set {
                this._frameSize = value;
                this.OnPropertyChanged("FrameSize");
            }
            get => this._frameSize;
        }

        private CompressMode _compressMode = CompressMode.NONE;

        /// <summary>
        ///     压缩类型
        /// </summary>
        public CompressMode CompressMode {
            set {
                this._compressMode = value;
                this.OnPropertyChanged("CompressMode");
            }
            get => this._compressMode;
        }

        /// <summary>
        ///     贴图的数据
        /// </summary>
        public byte[] Data = new byte[2];

        private int _index;

        /// <summary>
        ///     贴图在img中的下标
        /// </summary>
        public int Index {
            set {
                this._index = value;
                this.OnPropertyChanged("Index");
            }
            get => this._index;

        }


        /// <summary>
        ///     数据长度
        /// </summary>
        public int Length = 2;

        private Point _location;

        /// <summary>
        ///     贴图坐标
        /// </summary>
        public Point Location {
            set {
                this._location = value;
                this.OnPropertyChanged("Location");
            }
            get => this._location;
        }

        /// <summary>
        ///     存储该贴图的img
        /// </summary>
        public Album Parent { set; get; }


        private Size _size = new Size(1, 1);

        /// <summary>
        ///     贴图宽高
        /// </summary>
        public Size Size {
            set {
                this._size = value;
                this.OnPropertyChanged("Size");
            }
            get {
                if(this.Target != null) {
                    return this.Target.Size;
                }
                return this._size;
            }
        }

        private int _targetIndex = -1;

        /// <summary>
        /// 索引贴图的下标
        /// </summary>
        public int TargetIndex {
            set {
                this._targetIndex = value;
                if(value > -1) {
                    this.ColorFormat = ColorFormats.LINK;
                } else {
                    this.ColorFormat = ColorFormats.ARGB_1555;
                }
                this.OnPropertyChanged("TargetIndex");
            }
            get => this._targetIndex;
        }

        public bool IsLink => this.ColorFormat == ColorFormats.LINK && this.TargetIndex < this.Parent.Count && this.TargetIndex > -1;


        /// <summary>
        ///     当贴图为链接贴图时所指向的贴图
        /// </summary>
        public Sprite Target {
            get {
                if(this.IsLink) {
                    return this.Parent[this.TargetIndex];
                }
                return null;
            }
        }


        private ColorFormats _type = ColorFormats.ARGB_1555;

        /// <summary>
        ///     色位
        /// </summary>
        public ColorFormats ColorFormat {
            set {
                if(this._type != value) {
                    this._type = value;
                    this.OnPropertyChanged("ColorFormat");
                }
            }
            get => this._type;
        }

        /// <summary>
        /// 图片缓存
        /// </summary>
        private Bitmap _image;

        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap Image {
            get {
                if(this.IsLink) {
                    return this.Target?.Image;
                }
                if(this._image == null) {
                    this._image = this.ImageData.ToBitmap(); //使用父容器解析
                }
                return this._image;
            }
            set {
                this._image = value;
                this._imageData = ImageData.CreateByBitmap(value);
                if(value != null) {
                    this.Size = value.Size;
                } else {
                    this.Size = new Size(1, 1);
                }
            }
        }

        /// <summary>
        /// 图片数据缓存
        /// </summary>
        private ImageData _imageData;
        /// <summary>
        ///  Bitmap形式数据
        /// </summary>
        public ImageData ImageData {
            get {
                if(this.IsLink) {
                    return this.Target?.ImageData;
                }
                if(this._imageData == null) {
                    this._imageData = this.Parent.GetImageData(this);//使用父容器解析
                }
                return this._imageData;
            }
            set {
                this._imageData = value;
            }
        }


        public bool IsOpen => this._image != null || this._imageData != null;

        public int X {
            set => this.Location = new Point(value, this.Y);
            get => this.Location.X;
        }

        public int Y {
            set => this.Location = new Point(this.X, value);
            get => this.Location.Y;
        }

        public int Width {
            set => this.Size = new Size(value, this.Height);
            get => this.Size.Width;
        }

        public int Height {
            set => this.Size = new Size(this.Width, value);
            get => this.Size.Height;
        }

        public int FrameWidth {
            set => this.FrameSize = new Size(value, this.FrameHeight);
            get => this.FrameSize.Width;
        }

        public int FrameHeight {
            set => this.FrameSize = new Size(this.FrameWidth, value);
            get => this.FrameSize.Height;
        }

        /// <summary>
        ///     文件版本
        /// </summary>
        public ImgVersion Version => this.Parent.Version;

        public bool IsHidden => this.Width * this.Height == 1 && this.CompressMode == CompressMode.NONE;

        public event PropertyChangedEventHandler PropertyChanged;


        public Sprite() { }

        public Sprite(Album parent) {
            this.Parent = parent;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Load() {
            this._imageData = this.Parent.GetImageData(this); //使用父容器
        }

        public void Refresh() {
            this._imageData = null;
            this._image = null;
        }
        /// <summary>
        ///     替换贴图
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isAdjust"></param>
        /// <param name="bmp"></param>
        public void ReplaceImage(ColorFormats type, Bitmap bmp) {
            if(bmp == null) {
                return;
            }
            this.Image = bmp;
            this.TargetIndex = -1;
            if(type == ColorFormats.UNKNOWN) {
                if(this.ColorFormat == ColorFormats.LINK) {
                    type = ColorFormats.ARGB_1555;
                } else if(this.Version != ImgVersion.Ver5 && this.ColorFormat > ColorFormats.LINK) {
                    type = this.ColorFormat - 4;
                } else {
                    type = this.ColorFormat;
                }
            }
            this.ColorFormat = type;
            this.Size = bmp.Size;
            if(this.FrameHeight < bmp.Height) {
                this.FrameHeight = bmp.Height;
            }
            if(this.FrameWidth < bmp.Width) {
                this.FrameWidth = bmp.Width;
            }
            if(this.Width * this.Height > 1) {
                this.CompressMode = CompressMode.ZLIB;
            }
        }


        /// <summary>
        ///     裁剪画布透明部分
        /// </summary>
        public void TrimImage() {
            if(this.ColorFormat == ColorFormats.LINK || this.CompressMode == CompressMode.NONE) {
                return;
            }
            if(this.Image == null) {
                return;
            }
            var rct = this.Image.Scan();
            var image = new Bitmap(rct.Width, rct.Height);
            var g = Graphics.FromImage(image);
            var empty = new Rectangle(Point.Empty, rct.Size);
            g.DrawImage(this.Image, empty, rct, GraphicsUnit.Pixel);
            g.Dispose();
            this.Size = rct.Size;
            this.Location = this.Location.Add(rct.Location);
            this.Image = image;
        }

        /// <summary>
        ///     画布化
        /// </summary>
        /// <param name="target"></param>
        public void CanvasImage(Rectangle target) {
            if(this.IsLink) {
                return;
            }
            this.Image = this.Image.Canvas(target.Add(new Rectangle(this.Location, Size.Empty)));
            this.Size = target.Size;
            this.Location = target.Location;
        }


        /// <summary>
        ///     数据校正
        /// </summary>
        public void Adjust() {
            if(this.IsLink) {
                this.Length = 0;
                return;
            }
            if(!this.IsOpen) {
                return;
            }
            this.Data = this.Parent.Handler.ConvertToByte(this);
            if(this.Data.Length > 0 && this.CompressMode >= CompressMode.ZLIB) {
                this.Data = Zlib.Compress(this.Data);
            }
            this.Length = this.Data.Length; //不压缩时，按原长度保存
        }


        public bool Equals(Sprite entity) {
            return entity != null && this.Parent.Equals(entity.Parent) && this.Index == entity.Index;
        }

        public override string ToString() {
            if(this.ColorFormat == ColorFormats.LINK) {
                return $"{this.Index},\u00A0<TargetIndex>{this.TargetIndex}";
            }
            if(this.IsHidden) {
                return $"{this.Index},\u00A0<NullImage>";
            }
            return $"{this.Index},\u00A0<{this.ColorFormat}>, <Position>{this.Location.GetString()},\u00A0<Size>{this.Size.GetString()},\u00A0<FrameSize>{this.FrameSize.GetString()}";
        }

        public Sprite Clone(Album album) {
            return new Sprite(album) {
                Image = Image,
                CompressMode = CompressMode,
                ColorFormat = ColorFormat,
                Location = Location,
                FrameSize = FrameSize,
                TargetIndex = TargetIndex
            };
        }

        public string ToString(string formatString, IFormatProvider formatProvider) {
            if(formatProvider.GetFormat(this.GetType()) is ICustomFormatter formatter) {
                return formatter.Format(formatString, this, formatProvider);
            }
            return null;
        }

    }

}