using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Listener {

    [ExportMetadata("Name", ListenterKeys.REFRESH_FILE)]
    [Export(typeof(ICommandListener))]
    internal class RefreshFile : ICommandListener {

        public void Before(CommandEventArgs e) {
        }

        public void After(CommandEventArgs e) {
            e.Context.Add("__REFRESH_MODE", RefreshMode.File);
        }
    }
}
