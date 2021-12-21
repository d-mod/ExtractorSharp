using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Menu;

namespace ExtractorSharp.Services.Menu {

    [Export("fileList", typeof(IMenuItem))]
    internal class NewFileItem : ViewItem {

        public override string Key { set; get; } = "NewFile";

        public override int Order { set; get; } = 0;

        public override string Command { set; get; } = "NewFile";

    }
}
