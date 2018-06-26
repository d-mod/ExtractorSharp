using System.Collections.Generic;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    /// <summary>
    ///     移动文件
    /// </summary>
    internal class MoveFile : IMutipleAciton, IFileFlushable {
        private int _index;

        private List<Album> _list;

        private int _target;
        public string Name => "MoveFile";

        public bool CanUndo => true;

        public bool IsChanged => true;
        

        public void Action(params Album[] array) {
            for (var i = 0; i < array.Length; i++) _list.Remove(array[i]);
            _list.InsertRange(_target, array);
        }


        public void Do(params object[] args) {
            _index = (int) args[0];
            _target = (int) args[1];
            _list = Program.Connector.List;
            Move(_index, _target);
        }

        public void Redo() {
            Do(_index, _target);
        }

        public void Undo() {
            Move(_target, _index);
        }

        private void Move(int index, int target) {
            var item = _list[index];
            _list.RemoveAt(index);
            _list.Insert(target, item);
        }
    }
}