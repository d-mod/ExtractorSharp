using System;

namespace ExtractorSharp.Command.SpliceCommand {
    class SortSplice : ICommand {
        public void Batch(params object[] args) {
            throw new NotImplementedException();
        }

        public bool CanUndo => true;

        public bool Changed => false;

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
