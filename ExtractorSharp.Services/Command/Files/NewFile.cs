using System.Collections.Generic;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {


    /// <summary>
    /// 新建文件
    /// </summary>
    [ExportCommand("NewFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class NewFile : InjectService, IRollback {

        [CommandParameter("Target", IsDefault = true)]
        private Album file = new Album();

        [CommandParameter("Index")]
        private int index = -1;


        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
            this.Messager.Success(this.Language["<NewFile><Success>!"]);
        }

        public void Undo() {
            this.Store.Use<List<Album>>(StoreKeys.FILES, list => {
                list.Remove(this.file);
                return list;
            });
        }

        public void Redo() {
            this.file.NewImage(this.file.Count, ColorFormats.LINK, -1);
            this.Store.Use<List<Album>>(StoreKeys.FILES, list => {
                list.Insert(this.index + 1, this.file);
                return list;
            }).Set(StoreKeys.IS_SAVED, false);
        }

    }
}