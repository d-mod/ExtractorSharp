using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    /// <summary>
    /// 重命名文件
    /// </summary>
    [ExportCommand("RenameFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class RenameFile : InjectService, IRollback {


        [CommandParameter("File", IsRequired = true)]
        private Album Album;

        /// <summary>
        ///     新文件名
        /// </summary>
        [CommandParameter("Path")]
        private string _newPath;

        /// <summary>
        ///     原文件名
        /// </summary>
        private string _oldPath;

        public void Do(CommandContext context) {
            context.Export(this);
            this._oldPath = this.Album.Path;
            this.Redo();
        }

        public void Undo() {
            this.Album.Path = this._oldPath;
        }

        public void Redo() {
            if(this.Album == null) {
                return;
            }
            this.Album.Path = this._newPath; //修改文件名
            this.Messager.Success($"<RenameFile><Success>!");
        }

        public void Action(params Album[] array) {
            foreach(var item in array) {
                item.Path = this._newPath;
            }
        }


    }
}