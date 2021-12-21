using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Command.Files {

    [ExportCommand("SaveAsFile")]
    internal class SaveAsFile : ICommand {

        [CommandParameter("File", IsDefault = true)]
        private Album album;


        [CommandParameter("Path")]
        private string path;

        public void Do(CommandContext context) {
            context.Export(this);
            this.album.Save(this.path);
        }
    }
}
