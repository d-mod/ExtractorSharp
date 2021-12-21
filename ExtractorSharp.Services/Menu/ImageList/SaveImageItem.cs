using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Composition;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;
using ExtractorSharp.Composition.Control;

namespace ExtractorSharp.Services.Menu.ImageList {

    [Export("imageList", typeof(IMenuItem))]
    internal class SaveImageItem : ViewItem {

        public override string Key { set; get; } = "_SAVE[2]/SaveImage";

        public override string ToolTip { set; get; }

        public override int Order { set; get; } = 0;

        public override string Command { set; get; } = "SaveImage";

        public override string ShortCutKey { set; get; }

        public override bool CanExecute() {
            return !this.Store.IsNullOrEmpty(StoreKeys.SELECTED_IMAGE);
        }


    }
}