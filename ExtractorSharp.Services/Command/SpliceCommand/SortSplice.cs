using System;

namespace ExtractorSharp.Control.SpliceCommand {
    class SortSplice : ICommand {
        public void Batch(params object[] args) {
            throw new NotImplementedException();
        }

        public bool CanBatch => false;

        public bool CanUndo => true;

        public bool isChange => false;

        public void Do(Controller Controller, params object[] args) {
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
