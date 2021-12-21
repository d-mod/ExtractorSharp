using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Menu;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Menu {

    [Export(typeof(IMenuItem))]
    internal class OpenDirectoryMenuItem : InjectService, IRouteItem {

        public string Key { set; get; } = "File/_OPEN[0]/OpenDirectory";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 2;

        public string Command { set; get; } = "OpenDirectory";

        public string ShortCutKey { set; get; } = "Shift+Ctrl+O";


        public bool CanExecute() {
            return true;
        }

        public void Execute(object sender, EventArgs e) {
            var dialog = new CommonOpenFileDialog {
                IsFolderPicker = true,
                NavigateToShortcut = false
            };
            //dialog.Filters.Add(new CommonFileDialogFilter(Language["ImageResources"], "*.npk;*.img"));
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                this.Controller.Do("OpenFile", dialog.FileNames);
            }
        }

    }
}
