using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Menu.File.Save {

    [Export(typeof(IMenuItem))]
    internal class SaveDirectoryMenuItem : InjectService, IRouteItem {


        public string Key { set; get; } = "File/_SAVE[0]/SaveToDirectory";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 2;

        public string Command { set; get; } = "SaveToDirectory";

        public string ShortCutKey { set; get; } = "Shift+Ctrl+S";

        [StoreBinding(StoreKeys.FILES)]
        public IEnumerable<Album> Files { set; get; }

        public bool CanExecute() {
            return this.Files != null && this.Files.Count() > 0;
        }

        public void Execute(object sender, EventArgs e) {
            var dialog = new CommonOpenFileDialog {
                IsFolderPicker = true,
                Multiselect = false
            };
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                this.Controller.Do("SaveToDirectory", new CommandContext(this.Files){
                   { "Path", dialog.FileName }
                });
            }
        }
    }
}
