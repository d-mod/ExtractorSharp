using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.MergeCommand {
    /// <summary>
    ///     加入拼合
    /// </summary>
    internal class AddMerge : IMutipleAciton, ICommandMessage {
        private Album[] Array;

        public void Do(params object[] args) {
            Array = args as Album[];
            Program.Merger.Add(Array);
        }

        public void Undo() {
            Program.Merger.Remove(Array);
        }


        public void Redo() {
            Do(Array);
        }


        public void Action(Album[] array) {
            Program.Merger.Add(array);
        }

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "AddMerge";
    }
}