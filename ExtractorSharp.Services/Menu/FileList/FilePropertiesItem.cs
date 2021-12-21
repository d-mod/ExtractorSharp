using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Menu;

namespace ExtractorSharp.Services.Menu.FileList {

    [Export("fileList", typeof(IMenuItem))]
    internal class FilePropertiesItem : ViewItem {

        public override string Key { set; get; } = "Properties";

        public override int Order { set; get; } = 7;

        public override string Command { set; get; } = "FileProperties";

    }
}
