using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Listener {

    [ExportMetadata("Name", ListenterKeys.SAVE_CHANGED)]
    [Export(typeof(ICommandListener))]
    internal class SaveChanged : InjectService, ICommandListener {

        public void Before(CommandEventArgs e) {
        }

        public void After(CommandEventArgs e) {
            this.Store.Set(StoreKeys.IS_SAVED, false);
        }
    }
}
