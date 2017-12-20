using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExtractorSharp.Core;
using ExtractorSharp.Handle;
using ExtractorSharp.Properties;
using ExtractorSharp.Loose;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Sorter;

namespace ExtractorSharp.Core {
    /// <summary>
    /// 拼合器
    /// </summary>
    public class Merger {
       

        public delegate void MergeQueueHandler(object sender, MergeQueueEventArgs e);

        public delegate void MergeHandler(object sender, MergeEventArgs e);

        /// <summary>
        /// 拼合队列事件
        /// </summary>
        public event MergeQueueHandler MergeQueueChanged;
        /// <summary>
        /// 拼合队列事件参数
        /// </summary>
        public MergeQueueEventArgs Arguments { get; }

        /// <summary>
        /// 启动拼合
        /// </summary>
        public event MergeHandler MergeStarted;

        /// <summary>
        /// 拼合进行
        /// </summary>
        public event MergeHandler MergeProcessing;

        /// <summary>
        /// 拼合完成
        /// </summary>
        public event MergeHandler MergeCompleted;

        public void OnMergeQueueChanged() => MergeQueueChanged?.Invoke(this,null);

        private void OnMergeStarted(MergeEventArgs e) => MergeStarted?.Invoke(this, e);

        private void OnMergeProcessing(MergeEventArgs e) => MergeProcessing?.Invoke(this, e);

        private void OnMergeCompleted(MergeEventArgs e) => MergeCompleted?.Invoke(this, e);

        private List<ISorter> Sorters;

        private string RulePath => $"{Program.Config["RootPath"]}/rules/";

        public Merger() {
            Queues = new List<Album>();
            Sorters = new List<ISorter>();
            InitDictionary();
        }

        /// <summary>
        /// 初始化排序规则
        /// </summary>
        public void InitDictionary() {
            var builder = new LSBuilder();
            var defaultObj = builder.ReadJson(Resources.Queue);
            InitSorter(defaultObj);
            if (Directory.Exists(RulePath)) {
                foreach (var json in Directory.GetFiles(RulePath)) {
                    var obj = builder.Read(json);
                    InitSorter(obj);
                }
            } else {
                Directory.CreateDirectory(RulePath);
            }
        }

        private void InitSorter(LSObject obj) {
            var name = obj["Name"].Value.ToString();
            var sorter = Sorters.Find(e => e.Name.Equals(name));
            if (sorter == null) {
                sorter = new DefaultSorter {
                    Name = name
                };
                Sorters.Add(sorter);
            }
            var rule = obj["Rules"];
            sorter.Data = rule.GetValue(sorter.Type);
        }


        /// <summary>
        /// 加入拼合
        /// </summary>
        /// <param name="array"></param>
        public void Add(params Album[] array) {
            for (var i = 0; i < array.Length; i++) {
                array[i] = array[i].Clone();
            }
            Queues.AddRange(array);
            OnMergeQueueChanged();
        }

        /// <summary>
        /// 移出拼合队列
        /// </summary>
        /// <param name="array"></param>
        public void Remove(params Album[] array) {
            foreach (var al in array) {
                Queues.Remove(al);
            }
            var args = new MergeQueueEventArgs();
            OnMergeQueueChanged();
        }

        /// <summary>
        /// 执行拼合
        /// </summary>
        public void RunMerge() {
            var e = new MergeEventArgs();
            var Array = Queues.ToArray().Reverse();//序列反转
            int count = 0;
            var version = Img_Version.Ver2;
            foreach (Album al in Array) {
                if (al.List.Count > count) {//获得最大帧数
                    count = al.List.Count;
                }
                if (al.Version > version) {//获得最高文件版本
                    version = al.Version;
                }
            }
            var Album = new Album {
                Version = version
            };
            Album.InitHandle(null);
            Album.Tables = new List<List<Color>>();
            e.Count = count;
            e.Album = Album;
            OnMergeStarted(e);//启动拼合事件
            var entitys = new List<ImageEntity>();
            for (var i = 0; i < count; i++) {
                var entity = new ImageEntity(Album);
                var width = 1;
                var height = 1;
                var max_width = 0;
                var max_height = 0;
                var x = 800;
                var y = 600;
                var type = ColorBits.ARGB_1555;
                foreach (var al in Array) {
                    if (i < al.List.Count) {
                        var source = al.List[i];
                        if (source.Type == ColorBits.LINK)//如果为链接贴图。则引用指向贴图的属性
                            source = source.Target;
                        if (source.Cavas_Width > max_width)//最大画布
                            max_width = source.Cavas_Height;
                        if (source.Cavas_Height > max_height)
                            max_height = source.Cavas_Height;
                        if (source.Compress == Compress.NONE && source.Width * source.Height == 1)//将透明图层过滤
                            continue;
                        if (source.Width + source.X > width)//获得最右点坐标
                            width = source.Width + source.X;
                        if (source.Height + source.Y > height)//获得最下点坐标
                            height = source.Height + source.Y;
                        if (source.X < x)//获得最左点坐标
                            x = source.X;
                        if (source.Y < y)//获得最上点坐标
                            y = source.Y;
                        if (source.Type > type && source.Type < ColorBits.LINK)
                            type = source.Type;
                    }
                }
                width -= x;//获得上下左右两端的差,即宽高
                height -= y;
                width = width > 1 ? width : 1;//防止宽高小于1
                height = height > 1 ? height : 1;
                if (width * height > 1)//当贴图面积大于1时,使用zlib压缩
                    entity.Compress = Compress.ZLIB;
                entity.Type = type;
                entity.Index = entitys.Count;
                entity.Location = new Point(x, y);
                entity.Cavas_Size = new Size(max_width, max_height);
                var image = new Bitmap(width, height);
                using (var g = Graphics.FromImage(image)) {
                    foreach (var al in Array) {
                        if (i < al.List.Count) {
                            var source = al.List[i];
                            if (source.Type == ColorBits.LINK) {
                                source = source.Target;
                            }
                            g.DrawImage(source.Picture, source.X - x, source.Y - y);//绘制贴图
                        }
                    }
                }
                entity.ReplaceImage(type, false, image);//替换贴图
                e.Progress++;//拼合进度自增
                OnMergeProcessing(e);
                entitys.Add(entity);
            }
            Album.List.AddRange(entitys);
            OnMergeCompleted(e);//拼合完成
        }

        

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="useOther"></param>
        public void Sort(bool useOther) {
            Queues.Sort(Sorters[0].Comparer);
            OnMergeQueueChanged();
        }

       


        public void Clear() {
            Queues.Clear();
            OnMergeQueueChanged();
        }

        public List<Album> Queues { set; get; }


    }
}