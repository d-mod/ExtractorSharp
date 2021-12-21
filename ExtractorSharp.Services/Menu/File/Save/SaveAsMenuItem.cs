using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Services.Constants;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Menu {

    [Export(typeof(IMenuItem))]
    internal class SaveAsMenuItem : InjectService, IRouteItem {

        public string Key { set; get; } = "File/_SAVE[3]/SaveAs";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 1;

        public string Command { set; get; } = "SaveAs";

        public string ShortCutKey { set; get; }

        public bool CanExecute() {
            return true;
        }

        public void Execute(object sender, EventArgs e) {
            var path = this.Store.Get<string>(StoreKeys.SAVE_PATH);
            var dialog = new CommonSaveFileDialog(path) {
                DefaultExtension = ".NPK",
                AlwaysAppendDefaultExtension = true
            };
            dialog.Filters.Add(new CommonFileDialogFilter("NPK", "*.npk"));
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                this.Controller.Do("saveFile", dialog.FileName);
            }
        }

    }
}
