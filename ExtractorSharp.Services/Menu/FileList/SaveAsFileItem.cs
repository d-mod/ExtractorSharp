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

namespace ExtractorSharp.Services.Menu {

    [Export("fileList", typeof(IMenuItem))]
    internal class SaveAsFileItem : InjectService, IRouteItem {

        public string Key { set; get; } = "SaveAs";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 1;

        public string Command { set; get; } = "SaveAsFile";

        public string ShortCutKey { set; get; }

        public string Group { set; get; }

        public bool CanExecute() {
            return this.Store.IsNullOrEmpty(StoreKeys.SELECTED_FILE);
        }

        public void Execute(object sender, EventArgs e) {
            this.Store.Get(StoreKeys.SELECTED_FILE_RANGE, out List<Album> files);
            var count = files.Count();
            if(count == 1) {
                var file = files.ElementAt(0);
                var dialog = new CommonSaveFileDialog() {
                    DefaultFileName = file.Name,
                    DefaultExtension = ".img",
                    AlwaysAppendDefaultExtension = true
                };
                dialog.Filters.Add(new CommonFileDialogFilter("IMG", "*.img"));
                dialog.Filters.Add(new CommonFileDialogFilter("OGG", "*.ogg"));
                dialog.Filters.Add(new CommonFileDialogFilter("ALL", "*.*"));
                if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    this.Controller.Do("SaveAsFile", new CommandContext(file) {
                        {"Path",dialog.FileName }
                    });
                }
            } else if(count > 1) {
                var dialog = new CommonOpenFileDialog {
                    IsFolderPicker = true,
                    Multiselect = false
                };
                if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    this.Controller.Do("SaveToDirectory", new CommandContext(files) {
                        { "Path", dialog.FileName }
                    });
                }
            }
        }

    }
}
