using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {

    [Export("fileList", typeof(IMenuItem))]
    internal class RunMergeItem : ViewItem {

        public override string Key { set; get; } = "_Merge[9]/RunMerge";

        public override int Order { set; get; } = 9;

        public override string Command { set; get; } = "Merge";

        public override void Execute(object sender, EventArgs e) {
            this.Store.Get(StoreKeys.SELECTED_FILE,out Album file);
            if(file != null) {
                this.Store.Set("/merge/target-file", file);
            }
            base.Execute(sender, e);
        }

    }
}
