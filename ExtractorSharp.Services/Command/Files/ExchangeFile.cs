using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    /// <summary>
    /// 两个文件互换内容
    /// </summary>
    /// 
    [ExportCommand("ExchangeFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_LIST)]
    public class ExchangeFile : InjectService, IRollback {

        [CommandParameter("Source")]
        private Album source;

        [CommandParameter("Target")]
        private Album target;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            var temp = new Album();
            temp.Replace(this.target);

            this.target.Replace(this.source);
            this.source.Replace(temp);
            this.Messager.Success(this.Language["<ExchangeFile><Success>!"]);
        }

        public void Undo() {
            this.Redo();
        }
    }
}
