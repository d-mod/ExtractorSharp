using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu.ImageList {

    [Export("imageList", typeof(IMenuItem))]
    internal class HideImageItem : InjectService, IRouteItem {

        public string Key { set; get; } = "HideImage";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 3;

        public string Command { set; get; } = "HideImage";

        public string ShortCutKey { set; get; }



        public bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_IMAGE_RANGE);
        }

        public void Execute(object sender, EventArgs e) {

            this.Store.Get(StoreKeys.SELECTED_IMAGE_INDICES, out int[] indices)
                .Get(StoreKeys.SELECTED_FILE, out Album file);
            if(this.MessageBox.Show(this.Language["Tips"], this.Language["Tips", "HideSelectedImages"], CommonMessageBoxButton.OKCancel, CommonMessageBoxIcon.Question) == CommonMessageBoxResult.OK) {
                this.Controller.Do("HideImage", new CommandContext(file){
                    {"Indices",indices }
                });
            }
        }
    }
}
