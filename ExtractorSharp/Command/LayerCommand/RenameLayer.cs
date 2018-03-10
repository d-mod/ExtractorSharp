using ExtractorSharp.Core;
using ExtractorSharp.Draw;

namespace ExtractorSharp.Command.LayerCommand {
    class RenameLayer : ICommand,ICommandMessage{

        private Layer Layer;

        private string oldName;

        private string newName;

        public void Do(params object[] args) {
            Layer = args[0] as Layer;
            oldName = Layer.Name;
            Layer.Name = newName = args[1] as string;           
        }

        public void Undo() {
            Layer.Name = oldName;
        }
        public void Redo() => Do(Layer, newName);

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "RenameLayer";

    }
}
