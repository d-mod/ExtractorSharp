using System.ComponentModel.Composition;
using System.Text;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Services.Constants;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Commands {


    /// <summary>
    /// 打开一个文件对话框 设置文件保存路径
    /// </summary>

    [ExportCommand("SetSavePath")]
    public class SetSavePath : InjectService, ICommand {

        public string Name { set; get; } = "SetSavePath";

        public void Do(CommandContext context) {
            var dir = this.Store.Get<string>(StoreKeys.SAVE_PATH);
            var path = dir?.GetSuffix();
            if(!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(dir)) {
                dir = dir.Replace(path, "");
            }
            var dialog = new CommonSaveFileDialog {
                Title = this.Language["SetSavePath"],
                InitialDirectory = dir,
                DefaultFileName = path,
                DefaultExtension = ".NPK",
                AlwaysAppendDefaultExtension = true,
            };
            dialog.Filters.Add(new CommonFileDialogFilter("NPK", ".NPK"));
            var rs = dialog.ShowDialog();
            if(rs == CommonFileDialogResult.Ok) {
                this.Store.Set(StoreKeys.SAVE_PATH, dialog.FileName);
            }

            this.Store.Set(StoreKeys.SAVE_CANCEL, rs != CommonFileDialogResult.Ok);
        }
    }
}
