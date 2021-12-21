using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu.Edit {

    [Export(typeof(IMenuItem))]
    class ChangePositionMenuItem : InjectService,IRouteItem{

        public string Key { set; get; } = "Edit/SetPosition";

        public string ToolTip { set; get; }

        public List<IMenuItem> Children { set; get; }

        public int Order { set; get; } = 1;

        public string Command { set; get; } = "SetPosition";

        public string ShortCutKey { set; get; } = "Ctrl+B";


        public bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_IMAGE);
        }

        public void Execute(object sender, EventArgs e) {
            this.Store
                .Get("/draw/image-x", out int x)
                .Get("/draw/image-y", out int y)
                .Get(StoreKeys.SELECTED_FILE, out Album file)
                .Get(StoreKeys.SELECTED_IMAGE_INDEX, out int index);
            this.Controller.Do("ChangePosition", new CommandContext(file) {
                {"Indices",new[]{index} },
                { "X",x },
                { "Y",y }
            });
        }
    }
}
