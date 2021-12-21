using System;
using System.Collections.Generic;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Command.Files {

    [ExportCommand("ConvertFile")]
    public class ConvertFile : InjectService, ICommand {

        [CommandParameter(IsRequired = false)]
        private IEnumerable<Album> Files;

        [CommandParameter(IsRequired = false)]
        public IProgress<ProgressEventArgs> Progress;

        [CommandParameter("Version", IsDefault = true)]
        private ImgVersion TargetVersion = ImgVersion.Ver2;

        public void Do(CommandContext context) {
            context.Export(this);
            if(this.Progress == null) {
                this.Progress = new ConvertProgress(this.Store);
            }
            if(this.Files == null) {
                this.Files = this.Store.Get("/convert/queue", new List<Album>());
            }

            var count = 0;
            foreach(var al in this.Files) {
                count += al.Count;
            }
            var e = new ProgressEventArgs {
                Maximum = count
            };
            this.Progress.Report(e);
            foreach(var al in this.Files) {
                foreach(var sprite in al) {
                    sprite.Load();
                    e.Value++;
                    this.Progress.Report(e);
                }
                al.ConvertTo(this.TargetVersion);
            }
        }

        private class ConvertProgress : IProgress<ProgressEventArgs> {

            private Store Store { set; get; }

            public ConvertProgress(Store Store) {
                this.Store = Store;
            }

            public void Report(ProgressEventArgs e) {
                this.Store.Set("/covert/progrss", e.Value);
                if(e.IsCompleted) {
                    this.Store.Set("/covert/result", e.Result);
                }
            }
        }
    }
}
