using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {

    [Export("fileList", typeof(IMenuItem))]
    internal class AddMergeQueueItem : InjectService, IRouteItem {

        public string Key { set; get; } = "_Merge[9]/AddMergeQueue";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 8;


        public string Command { set; get; } = "AddMergeQueue";

        public string ShortCutKey { set; get; }

        public bool CanExecute() {
            return Store.IsNullOrEmpty(StoreKeys.SELECTED_FILE);
        }

        public void Execute(object sender, EventArgs e) {
            Store.Get(StoreKeys.SELECTED_FILE_RANGE, out List<Album> files);
            this.Controller.Do("AddMerge", files);
        }

    }
}
