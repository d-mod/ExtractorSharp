using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;

namespace ExtractorSharp.Services.Menu.File {
    [Export(typeof(IMenuItem))]
    internal class OpenNewWindowMenuItem : InjectService, IRouteItem {

        public string Key { set; get; } = "File/OpenNewWindow";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 5;

        public string Command { set; get; } = "OpenNewWindow";

        public string ShortCutKey { set; get; } = "Ctrl+W";

        public bool CanExecute() {
            return true;
        }

        public void Execute(object sender, EventArgs e) {
            Store.Dispatch("/app/new-window");
        }
    }
}
