using System.Collections.Generic;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Draw.Paint;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.Command.LayerCommand {
    internal class DeleteLayer : ICommand, ICommandMessage {
        private IPaint[] Array { set; get; }
        private int[] Indices { set; get; }
        private Drawer Drawer => Program.Drawer;
        private List<IPaint> List => Drawer.LayerList;
        public string Name => "DeleteLayer";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public void Do(params object[] args) {
            Indices = args[0] as int[];
            Array = new IPaint[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] < 2 || Indices[i] > List.Count - 1) continue;
                var layer = List[Indices[i]];
                if (!(layer is Layer)) {
                    Indices[i] = -1;
                    continue;
                }

                Array[i] = layer;
                List[Indices[i]] = null;
            }

            Indices = System.Array.FindAll(Indices, e => e > 0);
            Array = System.Array.FindAll(Array, e => e != null);
            List.RemoveAll(e => e == null);
            Drawer.OnLayerChanged(new LayerEventArgs());
        }

        public void Redo() {
            Do(Indices);
        }

        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                var index = Indices[i];
                if (index < List.Count - 1 && index > 1) {
                    List.Insert(index, Array[i]);
                } else {
                    List.Add(Array[i]);
                }
            }
        }
    }
}