using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {

    [Export("imageList", typeof(IMenuItem))]
    internal class ReplaceImageItem : ViewItem {

        public override string Key { set; get; } = "_ONE[1]/ReplaceImage";

        public override int Order { set; get; } = 1;

        public override string Command { set; get; } = "ReplaceImage";

        public override bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_IMAGE);
        }

    }
}
