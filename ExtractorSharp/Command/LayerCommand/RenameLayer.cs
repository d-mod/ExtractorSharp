using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Draw;
using ExtractorSharp.View;

namespace ExtractorSharp.Command.LayerCommand {
    class RenameLayer : ICommand {
        Controller Controller => Program.Controller;
        Layer Layer;
        string oldName;
        string newName;
        public void Do(params object[] args) {
            Layer = args[0] as Layer;
            oldName = Layer.Name;
            Layer.Name = newName = args[1] as string;
            Controller.LayerList.Refresh();
            Messager.ShowMessage(Msg_Type.Operate, Layer + "重命名成功");
        }

        public void Undo() {
            Layer.Name = oldName;
            Controller.LayerList.Refresh();
        }
        public void Redo() => Do(Layer, newName);

        public bool CanUndo => true;

        public void RunScript(string arg) { }

        public bool Changed => false;

        public string Name => "RenameLayer";

    }
}
