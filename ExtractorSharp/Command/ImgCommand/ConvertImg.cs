using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.ImgCommand {
    class ConvertImg : ICommand {
        public void Batch(params object[] args) {
            throw new NotImplementedException();
        }

        public bool CanUndo => true;

        public bool Changed => true;

        public void Do(params object[] args) {
            throw new NotImplementedException();
        }

        public void Redo() {
            throw new NotImplementedException();
        }

        public void RunScript(string arg) {
            throw new NotImplementedException();
        }

        public void Undo() {
            throw new NotImplementedException();
        }
    }
}
