using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {


    [Export(typeof(IMenuItem))]
    internal class CloseFileMenuItem : InjectService, IRouteItem {

        public Image Image { get; }

        public string Key { set; get; } = "File/CloseFile";

        public int ShortcutKeys { get; }

        public string ToolTip { set; get; }

        public int Order { set; get; } = 2;

        public string Command { set; get; } = "CloseFile";

        public string ShortCutKey { set; get; }

        public bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.FILES);
        }

        public void Execute(object sender, EventArgs e) {
            this.Store.Set(StoreKeys.FILES, new List<Album>())
            .Set(StoreKeys.SAVE_PATH, string.Empty)
            .Set(StoreKeys.IS_SAVED, true);
        }

    }
}
