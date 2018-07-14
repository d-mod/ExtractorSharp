using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    /// <summary>
    ///     替换IMG文件
    /// </summary>
    internal class ReplaceFile : IMutipleAciton, ICommandMessage {
        private Album _oldSource, _source, _target;

        public void Do(params object[] args) {
            _target = args[0] as Album;
            _source = args[1] as Album;
            if (_target == null || _source == null) {
                return;
            }
            _oldSource = new Album();
            _oldSource.Replace(_target);
            _target.Replace(_source);
        }

        public void Undo() {
            if (_target == null || _source == null) {
                return;
            }
            _target.Replace(_oldSource);
        }


        public void Redo() {
            Do(_target, _source);
        }


        public void Action(params Album[] array) {
            foreach (var al in array) {
                al.Replace(_source);
            }
        }

        public bool CanUndo => true;

        public bool IsChanged => true;        

        public string Name => "ReplaceFile";
    }
}