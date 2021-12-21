using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {

    [Export("fileList", typeof(IMenuItem))]
    internal class HideFileItem : InjectService, IRouteItem {

        public string Key { set; get; } = "HideFile";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 2;

        public string Command { set; get; } = "HideFile";

        public string ShortCutKey { set; get; }

        public string Group { set; get; }

        public bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_FILE);
        }


        public void Execute(object sender, EventArgs e) {
            var selecteds = this.Store.Get<IEnumerable<Album>>(StoreKeys.SELECTED_FILE_RANGE, new List<Album>());
            if(this.MessageBox.Show(this.Language["Tips"], this.Language["Tips", "HideFile"], CommonMessageBoxButton.OKCancel, CommonMessageBoxIcon.Question) == CommonMessageBoxResult.OK) {
                this.Controller.Do("HideFile", selecteds);
            }
        }

    }
}
