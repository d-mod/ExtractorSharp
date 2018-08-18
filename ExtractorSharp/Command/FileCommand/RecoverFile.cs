using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    /// <summary>
    ///    恢复文件，将文件恢复为原始文件
    /// </summary>
    class RecoverFile : IMutipleAciton, ICommandMessage {
        private Album[] currents;
        private Album[] olds;

        public bool CanUndo => true;

        public bool IsChanged => true;

        public string Name => "RecoverFile";

        private string GamePath => Program.Config["GamePath"].Value;

        public void Do(params object[] args) {
            currents = args as Album[];
            if (currents == null) {
                return;
            }
            olds = new Album[currents.Length];
            var i = 0;
            NpkCoder.Compare(GamePath, (a1, a2) => {
                var old = new Album();
                old.Replace(a2);//保存旧文件
                a2.Replace(a1); //替换为源文件
                olds[i++] = old;
            }, currents);
        }


        public void Redo() {
            Do(currents);
        }


        public void Action(params Album[] array) {
            NpkCoder.Compare(GamePath, (a1, a2) =>a1.Replace(a2), array);
        }


        public void Undo() {
            if (currents == null) {
                return;
            }
            for (var i = 0; i < currents.Length; i++) {
                if (olds[i] == null) {
                    continue;
                }
                currents[i].Replace(olds[i]);
            }
        }
    }
}