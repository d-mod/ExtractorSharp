using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    /// <summary>
    ///     修复文件，将文件缺少的贴图恢复为原始文件
    /// </summary>
    class RepairFile : IMutipleAciton, ICommandMessage {
        private Album[] array;

        private int[] counts;

        public bool CanUndo => true;

        public bool IsChanged => true;
       
        public string Name => "RepairFile";


        private string GamePath => Program.Config["GamePath"].Value;

        public void Do(params object[] args) {
            array = args as Album[];
            if (array == null) {
                return;
            }
            counts = new int[array.Length];
            var i = 0;
            NpkCoder.Compare(GamePath, (a1, a2) => {
                counts[i] = a1.List.Count - a2.List.Count;
                if (counts[i] > 0) {
                    var source = a1.List.GetRange(a2.List.Count, counts[i]); //获得源文件比当前文件多的贴图集合
                    source.ForEach(e => {
                        e.Load();
                        e.Parent = a2;
                    });
                    a2.List.AddRange(source); //加入到当前文件中,不修改原贴图。
                }

                i++;
            }, array);
        }


        public void Redo() {
            Do(array);
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
            for (var i = 0; i < array.Length; i++) {
                if (counts[i] > 0 && counts[i] <= array[i].List.Count) {
                    array[i].List.RemoveRange(array[i].List.Count - counts[i], counts[i]);
                }
            }
        }
    }
}