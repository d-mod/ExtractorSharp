using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;

namespace ExtractorSharp.Services.Menu.Edit {

    [Export(typeof(IMenuItem))]
    internal class RedoMenuItem : InjectService, IRouteItem {

        public string Key { set; get; } = "Edit/Redo";

        public string ToolTip { set; get; }

        public List<IMenuItem> Children { set; get; }

        public int Order { set; get; } = 1;

        public string Command { set; get; } = "Redo";

        public string ShortCutKey { set; get; } = "Ctrl+Y";


        public bool CanExecute() {
            return this.Controller.CanRedo;
        }

        public void Execute(object sender, EventArgs e) {
            this.Controller.Move(1);
        }

    }
}
