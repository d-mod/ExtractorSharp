using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.MergeCommand {
    internal class ClearMerge : ICommand, ICommandMessage {
        private Album[] Array;
        private Merger Merger;

        public void Do(params object[] args) {
            Merger = Program.Merger;
            Array = Merger.Queues.ToArray();
            Merger.Clear();
        }

        public void Undo() {
            Merger.Add(Array);
        }


        public void Redo() {
            Do();
        }

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "ClearMerge";
    }
}