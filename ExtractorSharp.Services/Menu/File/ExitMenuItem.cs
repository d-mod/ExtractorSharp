using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {

    [Export(typeof(IMenuItem))]
    internal class ExitMenuItem : InjectService, IRouteItem {

        public string Key { set; get; } = "File/Exit";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 99;

        public string Command { set; get; } = "ExitApp";

        public string ShortCutKey { set; get; } = "Alt+F4";

        public bool CanExecute() {
            return true;
        }

        public void Execute(object sender, EventArgs e) {
            this.Store.Dispatch(StoreKeys.APP_EXIT);
        }
    }
}
