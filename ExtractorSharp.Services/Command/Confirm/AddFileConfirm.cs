using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Command.Confirm {

    /// <summary>
    ///  添加文件到列表前验证是否清空之前的列表
    /// </summary>
    [ExportCommand("AddFileConfirm")]
    internal class AddFileConfirm : InjectService, ICommand {

        public void Do(CommandContext context) {
            var confirm = context.Get<bool>("Confirm");
            if(!confirm) {
                return;
            }
            var isClear = true;
            if(!this.Store.IsNullOrEmpty(StoreKeys.FILES)) {
                var result = this.MessageBox.Show(this.Language["Tips"], this.Language["Tips", "OpenFile"], CommonMessageBoxButton.YesNoCancel, CommonMessageBoxIcon.Question);
                if(result == CommonMessageBoxResult.Cancel) {
                    return;
                }
                isClear = result == CommonMessageBoxResult.Yes;
            }
            context.Add("IsClear", isClear);
        }
    }
}
