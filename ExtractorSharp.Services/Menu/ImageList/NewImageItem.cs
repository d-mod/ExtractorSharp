using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu.ImageList {

    [Export("imageList", typeof(IMenuItem))]
    internal class NewImageItem : ViewItem {

        public override string Key { set; get; } = "_ONE[1]/NewImage";

        public override int Order { set; get; } = 0;

        public override string Command { set; get; } = "NewImage";

        public override bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_FILE);
        }
    }
}
