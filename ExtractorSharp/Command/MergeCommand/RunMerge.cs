using ExtractorSharp.Core.Command;

namespace ExtractorSharp.Command.MergeCommand {
    internal class RunMerge : ICommand {
        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public void Do(params object[] args) {
            Program.Merger.RunMerge();
        }

        public void Redo() {
            // Method intentionally left empty.
        }

        public void Undo() {
            // Method intentionally left empty.
        }

        public string Name => "RunMerge";
    }
}