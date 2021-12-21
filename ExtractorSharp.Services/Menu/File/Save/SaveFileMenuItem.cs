using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {


    [Export(typeof(IMenuItem))]
    internal class SaveMenuItem : InjectService, IRouteItem {


        public string Key { set; get; } = "File/_SAVE[3]/SaveFile";

        public int ShortcutKeys { get; }

        public string ToolTip { set; get; }

        public int Order { set; get; } = 2;

        public string Command { set; get; } = "SaveFile";

        public string ShortCutKey { set; get; } = "Ctrl+S";

        public bool CanExecute() {
            return true;
        }

        public void Execute(object sender, EventArgs e) {
            var path = this.Store.Get<string>(StoreKeys.SAVE_PATH);
            var cancel = this.Store.Get<bool>(StoreKeys.SAVE_CANCEL);
            if(!cancel) {
                if(string.IsNullOrEmpty(path)) {
                    this.Controller.Do("SetSavePath");
                    this.Execute(sender, e);
                } else {
                    this.Controller.Do("SaveFile", path);
                }
            } else {
                this.Store.Set(StoreKeys.SAVE_CANCEL, false);
            }
        }
    }
}
