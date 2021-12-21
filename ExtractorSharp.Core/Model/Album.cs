using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Handle;

namespace ExtractorSharp.Core.Model {
    public sealed class Album : INotifyPropertyChanged , IFileObject {

        /// <summary>
        ///     贴图个数
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        ///     文件数据
        /// </summary>
        public byte[] Data;

        public List<Texture> Textures = new List<Texture>();

        public event PropertyChangedEventHandler PropertyChanged;

        public Album() {
            this.List = new List<Sprite>();
            this.Handler = Handler.CreateHandler(this.Version, this);
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Album(Bitmap[] array) : this() {
            for(var i = 0; i < array.Length; i++) {
                var sprite = new Sprite(this) {
                    Index = i,
                    Image = array[i],
                    CompressMode = CompressMode.ZLIB,
                    Size = array[i].Size,
                    FrameSize = array[i].Size
                };
                this.List.Add(sprite);
            }
            this.Adjust();
        }

        /// <summary>
        ///     处理器
        /// </summary>
        public Handler Handler { get; set; }

        /// <summary>
        ///     文件数据长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        ///     文件所在偏移量
        /// </summary>
        public int Offset { set; get; }

        /// <summary>
        ///     贴图列表
        /// </summary>
        public List<Sprite> List { private set; get; }

        /// <summary>
        ///     索引信息长度
        /// </summary>
        public long IndexLength { set; get; }

        private string _path = string.Empty;

        /// <summary>
        ///     文件路径
        /// </summary>
        public string Path {
            set {
                this._path = value;
                this.OnPropertyChanged("Path");
            }
            get => this._path;
        }

        public string Name {
            set => this.Path = this.Path.Replace(this.Name, value);
            get => this.Path.GetSuffix();
        }

        private ImgVersion _version = ImgVersion.Ver2;

        /// <summary>
        ///     文件版本
        /// </summary>
        public ImgVersion Version {
            set {
                this._version = value;
                this.OnPropertyChanged("Version");
            }
            get => this._version;
        }


        public List<Color> CurrentPalette {
            get {
                if(this.PaletteIndex > -1 && this.PaletteIndex < this.Palettes.Count) {
                    return this.Palettes[this.PaletteIndex];
                }
                return new List<Color>(256);
            }
            set {
                if(this.PaletteIndex > -1 && this.PaletteIndex < this.Palettes.Count) {
                    this.Palettes[this.PaletteIndex] = value;
                }
            }
        }

        /// <summary>
        ///     色表
        /// </summary>
        public List<List<Color>> Palettes { set; get; } = new List<List<Color>> { new List<Color>() };


        /// <summary>
        /// 重定向目标
        /// </summary>
        public Album Target { set; get; }

        private int _paletteIndex;
        /// <summary>
        ///     色表索引
        /// </summary>
        public int PaletteIndex {
            set {
                if(this._paletteIndex != value) {
                    this.Refresh();
                    this._paletteIndex = Math.Min(value, this.Palettes.Count - 1);
                    this.OnPropertyChanged("PaletteIndex");
                }
            }
            get => this._paletteIndex;
        }

        public Sprite this[int index] {
            get => this.List[index];
            set {
                if(index < this.List.Count) {
                    this.List[index] = value;
                } else {
                    this.List.Add(value);
                }
            }
        }


        public override string ToString() {
            return this.Name;
        }


        /// <summary>
        ///     重置图片
        /// </summary>
        public void Refresh() {
            foreach(var sprite in this.List) {
                sprite.Image = null;
            }
        }

        /// <summary>
        ///     初始化处理器
        /// </summary>
        /// <param name="stream"></param>
        public void InitHandle(Stream stream) {
            this.Handler = Handler.CreateHandler(this.Version, this);
            if(this.Handler != null && stream != null) {
                this.Handler.CreateFromStream(stream);
            }
        }

        /// <summary>
        ///     替换文件
        /// </summary>
        /// <param name="album"></param>
        public void Replace(Album album) {
            album = album.Clone();
            this.Version = album.Version;
            this.Palettes = album.Palettes;
            this.PaletteIndex = album.PaletteIndex;
            this.Handler = album.Handler;
            this.Handler.Album = this;
            this.List = album.List.ToList();
            this.AdjustIndex();
        }

        /// <summary>
        ///     转换版本
        /// </summary>
        /// <param name="version"></param>
        public void ConvertTo(ImgVersion version) {
            this.Handler.ConvertToVersion(version);
            this.Version = version;
            this.InitHandle(null);
        }

        /// <summary>
        ///     校正序号
        /// </summary>
        public void AdjustIndex() {
            for(var i = 0; i < this.List.Count; i++) {
                this.List[i].Index = i;
                //todo
                this.List[i].Parent = this;
            }
        }

        /// <summary>
        ///     隐藏贴图
        /// </summary>
        public void Hide() {
            var count = this.List.Count;
            this.List.Clear();
            this.Palettes = new List<List<Color>> { new List<Color>() };
            this.PaletteIndex = 0;
            this.ConvertTo(ImgVersion.Ver2);
            this.NewImage(count, ColorFormats.LINK, -1);
        }

        public Album Clone() {
            this.Adjust();
            var temp = NpkCoder.ReadImg(this.Data, this.Path);
            temp.PaletteIndex = this.PaletteIndex;
            return temp;
        }

        public void Save(Stream stream) {
            this.Adjust();
            stream.Write(this.Data);
        }

        public void Save(string file) {
            using(var fs = new FileStream(file, FileMode.Create)) {
                this.Save(fs);
            }
        }


        /// <summary>
        ///     根据路径判断唯一
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        public bool Equals(Album al) {
            return this.Path.Equals(al?.Path);
        }

        /// <summary>
        ///     从流初始化
        /// </summary>
        /// <param name="stream"></param>
        public void Load(Stream stream) {
            this.Handler.CreateFromStream(stream);
        }

        public ImageData GetImageData(Sprite sprite) {
            return this.Handler.GetImageData(sprite);
        }

        /// <summary>
        ///     转储为字节数组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public byte[] ConvertToByte(Sprite sprite) {
            return this.Handler.ConvertToByte(sprite);
        }

        /// <summary>
        ///     新建贴图
        /// </summary>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public void NewImage(int count, ColorFormats type, int index) {
            if(count < 1) {
                return;
            }
            if(index < 0) {
                index = this.List.Count;
            }
            var array = new Sprite[count];
            var first = new Sprite(this) {
                Index = index
            };

            this.List.Insert(index, first);

            if(type != ColorFormats.LINK) {
                array[0].ColorFormat = type;
            }

            for(var i = 1; i < count; i++) {
                array[i] = new Sprite(this) {
                    ColorFormat = type
                };
                if(type == ColorFormats.LINK) {
                    array[i].TargetIndex = index;
                }
                array[i].Index = index + i;
                this.List.Insert(index + i, array[i]);
            }
            this.AdjustIndex();
        }

        public void RemoveImage(int start, int length) {
            if(start < 0 || length < 1) {
                return;
            }
            this.List.RemoveRange(start, length);
            this.AdjustIndex();
        }


        /// <summary>
        ///     校正贴图
        /// </summary>
        public void Adjust() {
            if(this.Target != null) {
                return;
            }
            this.AdjustIndex();
            this.Handler.Adjust();
        }

        public IEnumerator<Sprite> GetEnumerator() {
            return this.List.GetEnumerator();
        }


    }
}