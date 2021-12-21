using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu.ImageList {

    [Export("imageList", typeof(IMenuItem))]
    internal class DeleteImageItem : InjectService, IRouteItem {

        public string Key { set; get; } = "DeleteImage";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 3;

        public string Command { set; get; } = "DeleteImage";

        public string ShortCutKey { set; get; }

        public bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_IMAGE_RANGE);
        }

        public void Execute(object sender, EventArgs e) {
            this.Store.Get(StoreKeys.SELECTED_IMAGE_INDICES, out int[] indices)
                .Get(StoreKeys.SELECTED_FILE, out Album file);
            if(this.MessageBox.Show(this.Language["Tips"], this.Language["Tips", "DeleteSelectedImages"], CommonMessageBoxButton.OKCancel, CommonMessageBoxIcon.Question) == CommonMessageBoxResult.OK) {
                this.Controller.Do("DeleteImage", new CommandContext(file){
                    {"Indices",indices }
                });
            }
        }
    }
}
