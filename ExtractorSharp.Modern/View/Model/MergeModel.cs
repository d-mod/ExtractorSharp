using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.View.Model {

    [Export]
    internal class MergeModel : BaseViewModel, IProgress<ProgressEventArgs> {

        private List<Album> _queue = new List<Album>();

        [StoreBinding("/merge/queue", ToWay = true)]
        public List<Album> Queue {

            set {
                this._queue = value?.ToList();
                this.OnPropertyChanged("Queue");
                this.OnPropertyChanged("CanMerge");
                this.OnPropertyChanged("PreviewImage");
                this.OnPropertyChanged("FrameCount");
                this.OnPropertyChanged("Indices");
            }

            get => this._queue;
        }

        public ImageSource PreviewImage {
            get {
                if(this.Index > -1) {
                    return Avatars.Preview(this.Queue.ToArray().Reverse(), this.Index).ToImageSource();
                }
                return null;
            }
        }

        public int[] Indices {
            get {
                var indices = new int[this.FrameCount];
                for(var i = 0; i < indices.Length; i++) {
                    indices[i] = i;
                }
                return indices;
            }
        }

        public int FrameCount {
            get {
                var queue = this.Queue;
                if(queue.Count > 0) {
                    return queue.Max(e => e.Count);
                }
                return 0;
            }
        }

        private int _index = 0;

        public int Index {
            set {
                this._index = value;
                this.OnPropertyChanged("Index");
                this.OnPropertyChanged("PreviewImage");
            }
            get {
                this._index = Math.Min(this._index, this.FrameCount);
                return this._index;
            }
        }



        private int _mergeProgress = 0;

        public int MergeProgress {
            set {
                this._mergeProgress = value;
                this.OnPropertyChanged("MergeProgress");
                this.OnPropertyChanged("MergeTips");
            }
            get => this._mergeProgress;
        }

        private int _mergeCount = 1;

        public int MergeCount {
            set {
                if(this._mergeCount != value) {
                    this._mergeCount = value;
                    this.OnPropertyChanged("MergeCount");
                }
            }
            get => this._mergeCount;
        }


        private bool _isCompleted = false;

        public bool IsCompleted {
            set {
                if(this._isCompleted != value) {
                    this._isCompleted = value;
                    this.OnPropertyChanged("IsCompleted");
                    this.OnPropertyChanged("MergeTips");
                }
            }
            get => this._isCompleted;
        }


        public string MergeTips {
            get {
                if(this.IsCompleted) {
                    return this.Language["MergeCompleted"];
                }
                return this.Language.Parse($"<MergeProcessing>({this.MergeProgress}/{this.MergeCount - 1})...");
            }
        }
        public BackgroundWorker MergeWorker;

        public override void OnImportsSatisfied() {
            base.OnImportsSatisfied();
            this.MergeWorker = new BackgroundWorker();
            this.MergeWorker.DoWork += (o, e) => this.Controller.Do("runMerge", this);
        }

        public void Report(ProgressEventArgs e) {
            this.MergeProgress = e.Value;
            this.IsCompleted = e.IsCompleted;
            this.MergeCount = e.Maximum;
            if(this.IsCompleted) {
                this.ResultFile = e.Result as Album;
            }
        }

        public bool CanMerge => this.Queue.Count > 0;

        private List<Album> _files;

        [StoreBinding(StoreKeys.FILES)]
        public List<Album> Files {
            set {
                this._files = value?.ToList();
                this.OnPropertyChanged("Files");
                this.OnPropertyChanged("TargeFile");
            }

            get => this._files;
        }

        private Album _targetFile;

        [StoreBinding("/merge/target-file", ToWay = true)]
        public Album TargetFile {
            set {
                this._targetFile = value;
                this.OnPropertyChanged("TargetFile");
            }
            get => this._targetFile;
        }


        public Album ResultFile { set; get; }

        public CommandContext ReplaceFileContext => new CommandContext {
                    {"Source", this.TargetFile },
                    {"Target", this.ResultFile }
                };
    }
}
