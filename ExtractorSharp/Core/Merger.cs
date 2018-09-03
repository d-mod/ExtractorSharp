using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Core.Sorter;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Json;
using ExtractorSharp.Properties;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     拼合器
    /// </summary>
    public class Merger :IMergeProgress{

        public delegate void MergeQueueHandler(object sender, MergeQueueEventArgs e);

        private readonly List<ISorter> Sorters;

        public ISorter Sorter => Sorters[0];

        public int Version { set; get; }

        public int GlowLayerMode { set; get; }


        public readonly List<Func<List<Album>,int,int>> PreHandles;

        public Merger() {
            Queues = new List<Album>();
            Sorters = new List<ISorter>();
            PreHandles = new List<Func<List<Album>, int, int>>();
            InitDictionary();
        }

        private string RulePath => $"{Program.Config["RootPath"]}/rules/";

        public List<Album> Queues { set; get; }

        /// <summary>
        ///     拼合队列事件
        /// </summary>
        public event MergeQueueHandler MergeQueueChanged;
        public event Avatars.MergeHandler MergeStarted;
        public event Avatars.MergeHandler MergeProcessing;
        public event Avatars.MergeHandler MergeCompleted;

        public void OnMergeQueueChanged(MergeQueueEventArgs e) {
            MergeQueueChanged?.Invoke(this, e);
        }

        public void OnMergeStarted(MergeEventArgs e) {
            MergeStarted?.Invoke(this, e);
        }

        public void OnMergeProcessing(MergeEventArgs e) {
            MergeProcessing?.Invoke(this, e);
        }

        public void OnMergeCompleted(MergeEventArgs e) {         
            MergeCompleted?.Invoke(this, e);
        }

        /// <summary>
        ///     初始化排序规则
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
        ///     加入拼合
        /// </summary>
        /// <param name="array"></param>
        public void Add(params Album[] array) {
            for (var i = 0; i < array.Length; i++) {
                array[i] = array[i].Clone();
            }
            Queues.AddRange(array);
            OnMergeQueueChanged(new MergeQueueEventArgs {
                Mode = QueueChangeMode.Add,
                Tag = array
            });
        }

        /// <summary>
        ///     移出拼合队列
        /// </summary>
        /// <param name="array"></param>
        public void Remove(params Album[] array) {
            foreach (var al in array) Queues.Remove(al);
            var args = new MergeQueueEventArgs();
            OnMergeQueueChanged(new MergeQueueEventArgs {
                Mode = QueueChangeMode.Remove,
                Tag = array
            });
        }


        /// <summary>
        ///     互换位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="target"></param>
        public void Move(int index, int target) {
            if (index > -1 && index != target) {
                var item = Queues[index];
                Queues.RemoveAt(index);
                Queues.InsertAt(target, new[] {item}); //插入到指定位置
                OnMergeQueueChanged(new MergeQueueEventArgs {
                    Mode = QueueChangeMode.Move
                }); //触发序列更改事件
            }
        }

        /// <summary>
        ///     执行拼合
        /// </summary>
        public void RunMerge() {
            var list = Queues.ToList();
            var version = Version;
            PreHandles.ForEach(e => version = e.Invoke(list, version));
            Avatars.Merge(list, version, this);
        }

        public void Priview(int index, Graphics g) {
            var array = Queues.ToArray();
            Array.Reverse(array);
            var bmp = Avatars.Preview(array, index);
            g.DrawImage(bmp, new Point(20, 20));
        }


        public int GetFrameCount() {
            var count = 0;
            foreach (var al in Queues) {
                if (al.List.Count > count) {
                    count = al.List.Count;
                }
            }
            return count;
        }


        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="useOther"></param>
        public void Sort() {
            Queues.Sort(Sorters[0].Comparer);
            OnMergeQueueChanged(new MergeQueueEventArgs {
                Mode = QueueChangeMode.Sort
            });
        }


        public void Clear() {
            Queues.Clear();
            OnMergeQueueChanged(new MergeQueueEventArgs {
                Mode = QueueChangeMode.Clear
            });
        }
    }
}