using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {


    [ExportCommand("ClearMerge")]
    internal class ClearMerge : InjectService, IRollback {

        private List<Album> Queues = new List<Album>();

        public void Do(CommandContext context) {
            this.Redo();
        }

        public void Undo() {
            this.Store.Set("/merge/queue", this.Queues);
        }


        public void Redo() {
            var empty = new List<Album>();
            this.Store.Use("/merge/queue", queues => {
                this.Queues = queues.ToList();
                return empty;
            }, empty);
            this.Messager.Success(this.Language["<ClearMerge><Success>"]);
        }


    }
}