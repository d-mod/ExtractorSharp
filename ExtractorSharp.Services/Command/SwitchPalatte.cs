using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {


    [ExportCommand("SwitchPalatte")]
    [CommandListeners(ListenterKeys.REFRESH_IMAGE)]
    internal class SwitchPalatte : ICommand {

        [CommandParameter("File")]
        private Album Album;

        [CommandParameter]
        private int Index;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Album.PaletteIndex = this.Index;
            this.Album.Refresh();
        }
    }
}
