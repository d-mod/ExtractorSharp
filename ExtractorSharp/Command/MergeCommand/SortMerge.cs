using System;

namespace ExtractorSharp.Command.MergeCommand {
    class SortMerge : ICommand {
        public void Batch(params object[] args) {
            throw new NotImplementedException();
        }

        public bool CanUndo => true;

        public bool Changed => false;

        public string Name => throw new NotImplementedException();

        public void Do(params object[] args) {
            throw new NotImplementedException();
        }

        public void RunScript(string arg) { }

        public void Redo() {
            throw new NotImplementedException();
        }

        public void Undo() {
            throw new NotImplementedException();
        }

    }
}
