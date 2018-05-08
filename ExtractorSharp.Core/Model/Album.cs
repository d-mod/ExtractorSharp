using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Handle;
using ExtractorSharp.Json.Attr;

namespace ExtractorSharp.Data {

    public sealed class Album {
        /// <summary>
        /// 处理器
        /// </summary>
        [LSIgnore]
        public Handler Handler { get; set; }
        /// <summary>
        /// 文件数据长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 文件所在偏移量
        /// </summary>
        public int Offset { set; get; }
        /// <summary>
        /// 贴图列表
        /// </summary>
        public List<Sprite> List { get; } = new List<Sprite>();
        /// <summary>
        /// 索引信息长度
        /// </summary>
        public long Info_Length { set; get; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { set; get; } = string.Empty;
        public string Name {
            get {
                return Path.GetSuffix();
            }
            set {
                Path = Path.Replace(Name, value);
            }
        }

        /// <summary>
        /// 文件版本
        /// </summary>
        public Img_Version Version { get; set; } = Img_Version.Ver2;
        /// <summary>
        /// 文件数据
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// 贴图个数
        /// </summary>
        public int Count;



        public List<Color> CurrentTable {
            get {
                if (TableIndex > -1 && TableIndex < Tables.Count) {
                    return Tables[TableIndex];
                }
                return new List<Color>();
            }
        }

        /// <summary>
        /// 色表
        /// </summary>
        public List<List<Color>> Tables { set; get; }

        /// <summary>
        /// 色表索引
        /// </summary>
        public int TableIndex {
            set {
                if (_tabindex != value) {
                    Refresh();
                    _tabindex = value;
                }
            }
            get {
                return _tabindex;
            }
        }


        private int _tabindex;

        
        public override string ToString() => Name;

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


        public Album() {
            Tables = new List<List<Color>>();
            Handler = Handler.CreateHandler(Version,this);
        }

        public Album(Bitmap[] array) : this() {
            var sprites = new Sprite[array.Length];
            for (var i = 0; i < array.Length; i++) {
                sprites[i] = new Sprite(this);
                sprites[i].Index = i;
                sprites[i].Picture = array[i];
                sprites[i].Compress = Compress.ZLIB;
                sprites[i].Size = array[i].Size;
                sprites[i].Canvas_Size = array[i].Size;
            }
            List.AddRange(sprites);
            Adjust();
        }


        /// <summary>
        /// 重置图片
        /// </summary>
        public void Refresh() => List.ForEach(e => e.Picture = null);

        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <param name="stream"></param>
        public void InitHandle(Stream stream) {
            Handler = Handler.CreateHandler(Version,this);
            if (Handler != null && stream != null) {
                Handler.CreateFromStream(stream);
            }
        }

        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="album"></param>
        public void Replace(Album album) {
            album = album.Clone();
            Version = album.Version;
            Tables = album.Tables;
            Handler = album.Handler;
            Handler.Album = this;
            List.Clear();
            List.AddRange(album.List);
            AdjustIndex();
        }

        /// <summary>
        /// 转换版本
        /// </summary>
        /// <param name="Version"></param>
        public void ConvertTo(Img_Version Version) {
            Handler.ConvertToVersion(Version);
            this.Version = Version;
            InitHandle(null);
        }

        /// <summary>
        /// 校正序号
        /// </summary>
        public void AdjustIndex() {
            for (var i = 0; i < List.Count; i++) {
                List[i].Index = i;
                List[i].Parent = this;
            }
        }

        /// <summary>
        /// 隐藏贴图
        /// </summary>
        public void Hide() {
            var count = List.Count;
            List.Clear();
            ConvertTo(Img_Version.Ver2);
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
            using(var fs=new FileStream(file, FileMode.Create)) {
                Save(fs);
            }
        }


        /// <summary>
        /// 根据路径判断唯一
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        public bool Equals(Album al) => Path.Equals(al?.Path);

        /// <summary>
        /// 从流初始化
        /// </summary>
        /// <param name="stream"></param>
        public void CreateFromStream(Stream stream) => Handler.CreateFromStream(stream);

        /// <summary>
        /// 解析贴图
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Bitmap ConvertToBitmap(Sprite entity)=>Handler.ConvertToBitmap(entity);

        /// <summary>
        /// 转储为字节数组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public byte[] ConvertToByte(Sprite entity) => Handler.ConvertToByte(entity);
        
        /// <summary>
        /// 新建贴图
        /// </summary>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public void NewImage(int count, ColorBits type, int index) {
            Handler.NewImage(count, type, index);
            AdjustIndex();
        }

        /// <summary>
        /// 校正贴图
        /// </summary>
        public void Adjust() {
            AdjustIndex();
            Handler.Adjust();
        }

        public IEnumerator<Sprite> GetEnumerator() => List.GetEnumerator();



    }
}
