using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    /// <summary>
    ///     修复文件，将文件缺少的贴图恢复为原始文件
    /// </summary>
    class RepairFile : IMutipleAciton, ICommandMessage {
        private Album[] _array;

        private int[] _counts;

        public bool CanUndo => true;

        public bool IsChanged => true;
       
        public string Name => "RepairFile";


        private string GamePath => Program.Config["GamePath"].Value;

        public void Do(params object[] args) {
            _array = args as Album[];
            if (_array == null) {
                return;
            }
            _counts = new int[_array.Length];
            var i = 0;
            NpkCoder.Compare(GamePath, (a1, a2) => {
                _counts[i] = a1.List.Count - a2.List.Count;
                if (_counts[i] > 0) {
                    var source = a1.List.GetRange(a2.List.Count, _counts[i]); //获得源文件比当前文件多的贴图集合
                    source.ForEach(e => {
                        e.Load();
                        e.Parent = a2;
                    });
                    a2.List.AddRange(source); //加入到当前文件中,不修改原贴图。
                }

                i++;
            }, _array);
        }


        public void Redo() {
            Do(_array);
        }


        public void Action(params Album[] array) {
            NpkCoder.Compare(GamePath, (a1, a2) => {
                var count = a1.List.Count - a2.List.Count;
                if (count <= 0) {
                    return;
                }
                var source = a1.List.GetRange(a2.List.Count, count);
                source.ForEach(e => {
                    e.Load();
                    e.Parent = a2;
                });
                a2.List.AddRange(source);
            }, array);
        }


        public void Undo() {
            for (var i = 0; i < _array.Length; i++) {
                if (_counts[i] > 0 && _counts[i] < _array[i].List.Count) {
                    _array[i].List.RemoveRange(_array[i].List.Count - _counts[i], _counts[i]);
                }
            }
        }
    }
}