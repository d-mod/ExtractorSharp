using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("MoveLayer")]
    internal class MoveLayer : IRollback {
        [CommandParameter]
        private int sourceIndex;

        [CommandParameter]
        private int targetIndex;

        /*        [Import]
                private Drawer Drawer;*/

        public void Do(CommandContext context) {
            context.Export(this);
            //  Drawer.MoveLayer(sourceIndex, targetIndex);
        }

        public void Redo() {
            //  Drawer.MoveLayer(sourceIndex, targetIndex);
        }

        public void Undo() {
            //   Drawer.MoveLayer(targetIndex, sourceIndex);
        }

    }
}