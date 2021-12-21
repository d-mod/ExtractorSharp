using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Command.Images {

    [ExportCommand("SaveAsImage")]
    class SaveAsImage : InjectService, ICommand {

        [CommandParameter(IsDefault =true)]
        private Sprite sprite;

        [CommandParameter(IsRequired =false)]
        private string Path;

        public void Do(CommandContext context) {
            context.Export(this);
            if(string.IsNullOrEmpty(Path)) {
                var file = sprite.Parent;
                var defaultName = file.Name.Replace(".img", "");
                var dialog = new CommonSaveFileDialog {
                    DefaultFileName = defaultName,
                    DefaultExtension = ".png",
                    AlwaysAppendDefaultExtension = true
                };
                dialog.Filters.Add(new CommonFileDialogFilter(Language["ImageFile"], ".png"));
                dialog.Filters.Add(new CommonFileDialogFilter(Language["ImageFile"], ".jpg"));
                dialog.Filters.Add(new CommonFileDialogFilter(Language["ImageFile"], ".bmp"));
                if(dialog.ShowDialog() != CommonFileDialogResult.Ok) {
                    context.IsCancel = true;
                    return;
                }
                Path = dialog.FileName;
            }
            sprite.Image.Save(Path);
        }
    }
}
