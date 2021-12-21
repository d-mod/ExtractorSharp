using System.Collections.Generic;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("SplitFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    public class SplitFile : InjectService, IRollback {

        [CommandParameter(IsDefault = true)]
        private Album[] _array;

        private List<Album> _list;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this.Store.Use<List<Album>>(StoreKeys.FILES, list => {
                this._list = list.ToList();
                foreach(var al in this._array) {
                    var arr = Avatars.SplitFile(al);
                    var index = list.FindIndex(e => e.Equals(al));
                    list.RemoveAt(index);

                }
                return list;
            });
            this.Messager.Success(this.Language["<SplitFile><Success>!"]);
        }

        public void Undo() {
            this.Store.Set(StoreKeys.FILES, this._list);
        }

    }
}