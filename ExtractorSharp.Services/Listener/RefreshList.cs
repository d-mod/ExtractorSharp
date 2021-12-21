using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Listener {
    [ExportMetadata("Name", ListenterKeys.REFRESH_LIST)]
    [Export(typeof(ICommandListener))]
    internal class RefreshList : ICommandListener {

        public void Before(CommandEventArgs e) {

        }

        public void After(CommandEventArgs e) {
            e.Context.Add("__REFRESH_MODE", RefreshMode.List);
        }
    }
}
