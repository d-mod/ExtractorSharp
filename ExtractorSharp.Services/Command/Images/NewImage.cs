using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     新建贴图
    ///     可撤销
    ///     可宏命令
    /// </summary>
    [ExportCommand("NewImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    internal class NewImage : InjectService, IRollback, IMutipleMacro {

        [CommandParameter("File",IsDefault = true)]
        private Album Album;

        [CommandParameter]
        private int Count;

        [CommandParameter]
        private int Index;

        [CommandParameter]
        private ColorFormats Type;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
            this.Messager.Success(this.Language["<NewImage><Success>"]);
        }


        public void Action(IEnumerable<Album> array) {
            foreach(var al in array) {
                al.NewImage(this.Count, this.Type, this.Index);
            }
        }

        /// <summary>
        ///     撤销新建贴图
        /// </summary>
        public void Undo() {
            this.Album.List.RemoveRange(this.Index, this.Count);
        }

        public void Redo() {
            this.Album.NewImage(this.Count, this.Type, this.Index);
        }

    }
}