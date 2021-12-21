using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Draw.Paint;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("RenameLayer")]
    internal class RenameLayer : IRollback {

        [CommandParameter]
        private Layer layer;

        [CommandParameter("Name")]
        private string newName;

        private string oldName;

        public void Do(CommandContext context) {
            context.Export(this);
            this.oldName = this.layer.Name;
            this.Redo();
        }

        public void Undo() {
            this.layer.Name = this.oldName;
        }

        public void Redo() {
            this.layer.Name = this.newName;
        }
    }
}