using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.MergeCommand {
    internal class RemoveMerge : ICommand, ICommandMessage {
        private Album[] Array;
        private Merger Merger;

        public void Do(params object[] args) {
            Array = args as Album[];
            Merger = Program.Merger;
            Merger.Remove(Array);
        }

        public void Undo() {
            Merger.Add(Array);
        }

        public void Redo() {
            Do(Array);
        }

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "RemoveMerge";

        public void Action(params Album[] Array) {
            Merger.Remove(Array);
        }
    }
}