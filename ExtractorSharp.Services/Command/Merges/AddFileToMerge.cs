using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Command.Merges {

    [ExportCommand("AddFileToMerge")]
    internal class AddFileToMerge : InjectService, ICommand {

        public string Name => "AddFileToMerge";

        public void Do(CommandContext context) {
            var dialog = new CommonOpenFileDialog {
                Multiselect = true
            };
            dialog.Filters.Add(new CommonFileDialogFilter(this.Language["ImageResources"], "*.npk;*.img"));
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                this.Controller
                    .Do("LoadFile", dialog.FileNames)
                    .Do("AddMerge");

            }

        }
    }
}
