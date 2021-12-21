using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Draw;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("DeleteLayer")]
    internal class DeleteLayer : IRollback {

        private IPaint[] Layers;

        private int[] Indices;

        private List<IPaint> LayerList = new List<IPaint>();


        public void Do(CommandContext context) {
            context.Get(out this.Indices);
            this.Redo();
        }

        public void Redo() {
            this.Layers = new IPaint[this.LayerList.Count];
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this.Indices[i] < 2 || this.Indices[i] > this.LayerList.Count - 1) {
                    continue;
                }
                var paint = this.LayerList[this.Indices[i]];
                this.Layers[this.Indices[i]] = paint;
                this.LayerList[this.Indices[i]] = null;
                i--;
            }
            this.LayerList.RemoveAll(e => e == null);
        }

        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                var index = this.Indices[i];
                if(index < 2) {
                    continue;
                }
                this.LayerList.InsertAt(index, new IPaint[] { this.Layers[index] });
            }
        }

    }
}