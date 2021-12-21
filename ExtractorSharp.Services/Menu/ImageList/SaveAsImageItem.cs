using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Composition;
using ExtractorSharp.Services.Constants;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Menu.ImageList {

    [Export("imageList", typeof(IMenuItem))]
    internal class SaveAsImageItem : InjectService, IRouteItem {

        public string Key { set; get; } = "_SAVE[2]/SaveAs";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 0;

        public string Command { set; get; } = "SaveAsImage";

        public string ShortCutKey { set; get; }

        public bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_IMAGE);
        }

        public void Execute(object sender, EventArgs e) {
            this.Store.Get(StoreKeys.SELECTED_IMAGE, out Sprite sprite);
            Controller.Do("SaveAsImage", sprite);
        }

    }
}
