using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Menu.FileList {

    [Export("fileList", typeof(IMenuItem))]
    internal class ReplaceFromOutsideFileItem : InjectService, IRouteItem {

        public string Command { set; get; } = "ReplaceFromOutsideFile";

        public string ShortCutKey { set; get; } = "Ctrl+Q";

        public string Key { set; get; } = "_REPLACE[2]/ReplaceFromOutsideFile";

        public string ToolTip { set; get; }

        public int Order { set; get; }

        public bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_FILE);
        }

        public void Execute(object sender, EventArgs e) {
            this.Store.Get(StoreKeys.SELECTED_FILE,out Album file);
            var dialog = new CommonOpenFileDialog {
                Multiselect = false,
                DefaultExtension = ".img"
            };
            dialog.Filters.Add(new CommonFileDialogFilter(this.Language["ImageResources"], "*.img"));
            dialog.Filters.Add(new CommonFileDialogFilter(this.Language["SoundResources"], "*.ogg"));
            if(file.Path.EndsWith(".ogg")) {
                dialog.DefaultExtension = ".ogg";
            }
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                this.Controller
                    .Do("LoadFile", new[] { dialog.FileName })
                    .Do("ReplaceFile", file);
            }
        }
    }
}
