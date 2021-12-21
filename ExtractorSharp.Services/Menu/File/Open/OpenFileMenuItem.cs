using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;

namespace ExtractorSharp.Services.Menu {

    [Export(typeof(IMenuItem))]
    internal class OpenFileMenuItem : InjectService, IRouteItem {


        public string Key { set; get; } = "File/_OPEN[0]/OpenFile";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 1;

        public string Command { set; get; } = "OpenFile";

        public string ShortCutKey { set; get; } = "Ctrl+O";

        public string Group { set; get; }


        public bool CanExecute() {
            return true;
        }

        public void Execute(object sender, EventArgs e) {
            this.Controller.Do("OpenFile");
        }


    }

}
