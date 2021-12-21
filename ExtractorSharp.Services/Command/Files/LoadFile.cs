using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {


    /// <summary>
    /// 加载文件，并临时存储
    /// </summary>
    [ExportCommand("LoadFile")]
    public class LoadFile : InjectService, ICommand {

        [ImportMany(typeof(IFileSupport))]
        private List<IFileSupport> FileSupports { set; get; }

        [CommandParameter(IsRequired = false)]
        private IProgress<ProgressEventArgs> Progress { set; get; }

        [CommandParameter(IsDefault = true)]
        private IEnumerable<string> Paths { set; get; }

        private List<string> GetFiles(IEnumerable<string> args) {
            var list = new List<string>();
            foreach(var arg in args) {
                if(Directory.Exists(arg)) {
                    list.AddRange(this.GetFiles(Directory.GetFiles(arg)));
                } else {
                    list.Add(arg);
                }
            }
            return list;
        }


        public void Do(CommandContext context) {
            context.Export(this);
            if(this.Progress == null) {
                this.Progress = new LoadFileProgress(this.Store);
            }

            var args = this.GetFiles(this.Paths);
            var list = new List<Album>();
            var e = new ProgressEventArgs {
                Value = 0,
                Maximum = args.Count,
            };
            this.Store
                .Set("/load/progress", 0)
                .Set("/load/count", args.Count);
            foreach(var arg in args) {
                var support = this.FileSupports.Find(s => arg.ToLower().EndsWith(s.Extension));
                if(support != null) {
                    var arr = support.Decode(arg);
                    list.AddRange(arr);
                }
                e.Value++;
                this.Progress.Report(e);
            }
            this.Store
                .Set(StoreKeys.LOAD_FILES, list);
            this.Progress.Report(e);
        }

        public class LoadFileProgress : IProgress<ProgressEventArgs> {

            private readonly Store Store;

            public LoadFileProgress(Store Store) {
                this.Store = Store;
            }

            public void Report(ProgressEventArgs e) {
                this.Store.Set("/load/progress", e.Value);
            }

        }
    }
}
