using System.Collections.Generic;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     移动文件
    /// </summary>
    /// 
    [ExportCommand(CommandKeys.MOVE_FILE)]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class MoveFile : InjectService, IRollback, IMutipleMacro {

        [CommandParameter("Index")]
        private int _index;

        [CommandParameter("Target")]
        private int _target;

        private List<Album> _list;


        public void Action(IEnumerable<Album> array) {
            for(var i = 0; i < array.Count(); i++) {
                this._list.Remove(array.ElementAt(i));
            }
            this._list.InsertRange(this._target, array);
        }

        public void Do(CommandContext context) {
            context.Export(this);

            this.Store.Get(StoreKeys.FILES, out this._list);
            this.Redo();
        }

        public void Redo() {
            this.Move(this._index, this._target);
        }

        public void Undo() {
            this.Move(this._target, this._index);
        }

        private void Move(int index, int target) {
            var item = this._list[index];
            this._list.RemoveAt(index);
            this._list.Insert(target, item);
            this.Store.Set(StoreKeys.FILES, this._list);
        }
    }
}