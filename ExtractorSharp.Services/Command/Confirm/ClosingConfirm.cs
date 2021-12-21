using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Command {

    [ExportCommand("ClosingConfirm")]
    class ClosingConfirm : InjectService, ICommand {
        public void Do(CommandContext context) {
            var cancel = false;
            var isSave = this.Store.Get<bool>(StoreKeys.IS_SAVED);
            if(!isSave) {
                var result = this.MessageBox.Show(this.Language["Tips"], this.Language["SaveTips"], CommonMessageBoxButton.YesNoCancel, CommonMessageBoxIcon.Question);
                switch(result) {
                    case CommonMessageBoxResult.Yes:
                        var path = this.Store.Get<string>(StoreKeys.SAVE_PATH);
                        Controller.Do("SaveFile", path);
                        break;
                    case CommonMessageBoxResult.Cancel:
                        cancel = true;
                        break;
                    default:
                        break;
                }
            }
            context.Add("cancel", cancel);
        }
    }
}
