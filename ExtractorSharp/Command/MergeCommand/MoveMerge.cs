using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;

namespace ExtractorSharp.Command.MergeCommand {
    internal class MoveMerge : ICommand {
        public int Index;

        public int Target;

        public Merger Merger => Program.Merger;
        public string Name => "MoveMerge";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public void Do(params object[] args) {
            Index = (int) args[0];
            Target = (int) args[1];
            Merger.Move(Index, Target);
        }


        public void Redo() {
            Do(Index, Target);
        }

        public void Undo() {
            Merger.Move(Target, Index);
        }
    }
}