using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;

namespace ExtractorSharp.Command.LayerCommand {
    internal class MoveLayer : ICommand {
        private int SoureIndex { set; get; }

        private int TargetIndex { set; get; }

        private static Drawer Drawer => Program.Drawer;

        public string Name => "MoveLayer";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public void Do(params object[] args) {
            SoureIndex = (int) args[0];
            TargetIndex = (int) args[1];
            Drawer.MoveLayer(SoureIndex, TargetIndex);
        }

        public void Redo() {
            Drawer.MoveLayer(SoureIndex, TargetIndex);
        }

        public void Undo() {
            Drawer.MoveLayer(TargetIndex, SoureIndex);
        }
    }
}