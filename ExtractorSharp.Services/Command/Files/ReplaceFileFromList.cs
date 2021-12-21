using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {


    /// <summary>
    /// 列表内的文件替换
    /// </summary>
    [ExportCommand("ReplaceFromList")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    internal class ReplaceFileFromList : InjectService, IRollback {

        private Album _oldSource;


        [CommandParameter("Source", IsRequired = true)]
        private Album _source;

        [CommandParameter("Target", IsRequired = true)]
        private Album _target;

        public void Do(CommandContext context) {
            context.Export(this);

            this._oldSource = new Album();
            this._oldSource.Replace(this._target);
            this.Redo();
            this.Messager.Success(this.Language["<RepairFile><Success>!"]);
        }

        public void Undo() {
            this._target.Replace(this._oldSource);
        }


        public void Redo() {
            this._target.Replace(this._source);
        }


        public void Action(params Album[] array) {
            foreach(var al in array) {
                al.Replace(this._source);
            }
        }

        public bool IsChanged => true;

    }
}
