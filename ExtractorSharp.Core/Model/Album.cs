using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Json.Attr;

namespace ExtractorSharp.Core.Model {
    public sealed class Album {
        private int _tabindex;

        /// <summary>
        ///     贴图个数
        /// </summary>
        public int Count;

        /// <summary>
        ///     文件数据
        /// </summary>
        public byte[] Data;


        public List<Texture> Textures = new List<Texture>();


        public Album() {
            Handler = Handler.CreateHandler(Version, this);
        }

        public Album(Bitmap[] array) : this() {
            var sprites = new Sprite[array.Length];
            for (var i = 0; i < array.Length; i++) {
                sprites[i] = new Sprite(this) {
                    Index = i,
                    Picture = array[i],
                    CompressMode = CompressMode.ZLIB,
                    Size = array[i].Size,
                    CanvasSize = array[i].Size
                };
            }
            List.AddRange(sprites);
            Adjust();
        }

        /// <summary>
        ///     处理器
        /// </summary>
        [LSIgnore]
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
        public List<Sprite> List { get; } = new List<Sprite>();

        /// <summary>
        ///     索引信息长度
        /// </summary>
        public long IndexLength { set; get; }

        /// <summary>
        ///     文件路径
        /// </summary>
        public string Path { set; get; } = string.Empty;

        public string Name {
            get => Path.GetSuffix();
            set => Path = Path.Replace(Name, value);
        }

        /// <summary>
        ///     文件版本
        /// </summary>
        public ImgVersion Version { get; set; } = ImgVersion.Ver2;


        public List<Color> CurrentTable {
            get {
                if (TableIndex > -1 && TableIndex < Tables.Count) {
                    return Tables[TableIndex];
                }
                return new List<Color>();
            }
        }

        /// <summary>
        ///     色表
        /// </summary>
        public List<List<Color>> Tables { set; get; } = new List<List<Color>> {new List<Color>()};

        /// <summary>
        ///     色表索引
        /// </summary>
        public int TableIndex {
            set {
                if (_tabindex != value) {
                    Refresh();
                    _tabindex = Math.Min(value, Tables.Count - 1);
                }
            }
            get => _tabindex;
        }

        public Sprite this[int index] {
            get => List[index];
            set {
                if (index < List.Count) {
                    List[index] = value;
                } else {
                    List.Add(value);
                }
            }
        }


        public override string ToString() {
            return Name;
        }


        /// <summary>
        ///     重置图片
        /// </summary>
        public void Refresh() {
            List.ForEach(e => e.Picture = null);
        }

        /// <summary>
        ///     初始化处理器
        /// </summary>
        /// <param name="stream"></param>
        public void InitHandle(Stream stream) {
            Handler = Handler.CreateHandler(Version, this);
            if (Handler != null && stream != null) Handler.CreateFromStream(stream);
        }

        /// <summary>
        ///     替换文件
        /// </summary>
        /// <param name="album"></param>
        public void Replace(Album album) {
            album = album.Clone();
            Version = album.Version;
            Tables = album.Tables;
            TableIndex = album.TableIndex;
            Handler = album.Handler;
            Handler.Album = this;
            List.Clear();
            List.AddRange(album.List);
            AdjustIndex();
        }

        /// <summary>
        ///     转换版本
        /// </summary>
        /// <param name="version"></param>
        public void ConvertTo(ImgVersion version) {
            Handler.ConvertToVersion(version);
            Version = version;
            InitHandle(null);
        }

        /// <summary>
        ///     校正序号
        /// </summary>
        public void AdjustIndex() {
            for (var i = 0; i < List.Count; i++) {
                List[i].Index = i;
                List[i].Parent = this;
            }
        }

        /// <summary>
        ///     隐藏贴图
        /// </summary>
        public void Hide() {
            var count = List.Count;
            List.Clear();
            Tables = new List<List<Color>> {new List<Color>()};
            TableIndex = 0;
            ConvertTo(ImgVersion.Ver2);
            NewImage(count, ColorBits.LINK, -1);
        }

        public Album Clone() {
            Adjust();
            var ms = new MemoryStream(Data);
            var list = ms.ReadNPK(Name);
            ms.Close();
            Album temp = null;
            if (list.Count > 0) {
                temp = list[0];
                temp.Path = Path;
                temp.TableIndex = TableIndex;
            }
            return temp;
        }

        public void Save(Stream stream) {
            Adjust();
            stream.Write(Data);
        }

        public void Save(string file) {
            using (var fs = new FileStream(file, FileMode.Create)) {
                Save(fs);
            }
        }


        /// <summary>
        ///     根据路径判断唯一
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        public bool Equals(Album al) {
            return Path.Equals(al?.Path);
        }

        /// <summary>
        ///     从流初始化
        /// </summary>
        /// <param name="stream"></param>
        public void CreateFromStream(Stream stream) {
            Handler.CreateFromStream(stream);
        }

        /// <summary>
        ///     解析贴图
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Bitmap ConvertToBitmap(Sprite entity) {
            return Handler.ConvertToBitmap(entity);
        }

        /// <summary>
        ///     转储为字节数组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public byte[] ConvertToByte(Sprite entity) {
            return Handler.ConvertToByte(entity);
        }

        /// <summary>
        ///     新建贴图
        /// </summary>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public void NewImage(int count, ColorBits type, int index) {
            Handler.NewImage(count, type, index);
            AdjustIndex();
        }

        /// <summary>
        ///     校正贴图
        /// </summary>
        public void Adjust() {
            AdjustIndex();
            Handler.Adjust();
        }

        public IEnumerator<Sprite> GetEnumerator() {
            return List.GetEnumerator();
        }
    }
}