using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Menu;

namespace ExtractorSharp.Services.Menu.ImageList {
    [Export("imageList", typeof(IMenuItem))]
    internal class ImagePropertiesItem : ViewItem {

        public override string Key { set; get; } = "Properties";

        public override int Order { set; get; } = 7;

        public override string Command { set; get; } = "ImageProperties";

    }
}
