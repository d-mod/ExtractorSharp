using System.Collections.Generic;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Draw.Paint;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.Command.LayerCommand {
    internal class DeleteLayer : ICommand, ICommandMessage {

        private ILayer[] Array { set; get; }

        private int[] Indices { set; get; }

        private Drawer Drawer => Program.Drawer;

        private List<IPaint> List => Drawer.LayerList;

        public string Name => "DeleteLayer";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public void Do(params object[] args) {
            Indices = args[0] as int[];
            Array = new ILayer[List.Count];
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] < 2 || Indices[i] > List.Count - 1) {
                    continue;
                }
                var paint = List[Indices[i]];
                if (paint is ILayer layer) {
                    Array[Indices[i]] = layer;
                    List[Indices[i]] = null;
                    continue;
                }
                Indices[i] = -1;
            }
            List.RemoveAll(e => e == null);
            Drawer.OnLayerChanged(new LayerEventArgs());
        }

        public void Redo() {
            Do(Indices);
        }

        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                var index = Indices[i];
                if (index < 0) {
                    continue;
                }
                List.InsertAt(index, new ILayer[] { Array[index] });
            }
        }
    }
}