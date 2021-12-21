using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {



    [ExportCommand("RmoveMerge")]
    internal class RemoveMerge : InjectService, IRollback {

        private Album[] Files;

        public void Do(CommandContext context) {
            context.Get(out this.Files);
            this.Redo();
        }

        public void Undo() {
            this.Store.Use<List<Album>>("/merge/queue", queues => {
                queues.AddRange(this.Files);
                return queues;
            });
        }

        public void Redo() {
            this.Action(this.Files);
            this.Messager.Success(this.Language["<RemoveMerge><Success>"]);
        }

        public void Action(params Album[] array) {
            this.Store.Use<List<Album>>("/merge/queue", queues => {
                if(array != null) {
                    queues.RemoveAll(array.Contains);
                }
                return queues;
            });
        }
    }
}