using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu.FileList {


    [Export("fileList", typeof(IMenuItem))]
    internal class RenameFileItem : InjectService, IRouteItem {

        public string Key { set; get; } = "Rename";

        public string ToolTip { set; get; }

        public int Order { set; get; } = 6;

        public List<IMenuItem> Children { set; get; }

        public string Command { set; get; } = "RenameFile";

        public string ShortCutKey { set; get; }

        public string Group { set; get; }

      
        public bool CanExecute() {
            return this.Store.IsNullOrEmpty(StoreKeys.SELECTED_FILE);
        }


        public void Execute(object sender, EventArgs e) {
            this.Store
                .Get(StoreKeys.FILES,out List<Album> files)
                .Set("/app/rename-file", files?.ElementAt(0));
        }

    }
}
