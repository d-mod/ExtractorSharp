using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;

namespace ExtractorSharp.Services.Menu.Edit {

    [Export(typeof(IMenuItem))]
    internal class UndoMenuItem : InjectService, IRouteItem {
        public string Key { set; get; } = "Edit/Undo";

        public string ToolTip { set; get; }


        public int Order { set; get; } = 0;

        public string Command { set; get; } = "Undo";

        public string ShortCutKey { set; get; } = "Ctrl+Z";

        public bool CanExecute() {
            return this.Controller.CanUndo;
        }

        public void Execute(object sender, EventArgs e) {

            this.Controller.Move(-1);
        }
    }
}
