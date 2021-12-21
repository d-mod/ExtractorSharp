using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {



    [ExportCommand("MoveMerge")]
    internal class MoveMerge : InjectService, IRollback {

        [CommandParameter]
        public int Index;

        [CommandParameter]
        public int Target;

        private void Move(int source, int dest) {
            this.Store.Use<List<Album>>("/merge/queue", queues => {
                queues.Switch(source, dest);
                return queues;
            });
        }


        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this.Move(this.Index, this.Target);
        }

        public void Undo() {
            this.Move(this.Target, this.Index);
        }
    }
}