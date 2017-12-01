using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.LayerCommand {
    class AddLayer : ICommand {
        public bool CanUndo => true;

        public bool Changed => false;

        public string Name => throw new NotImplementedException();

        public void Do( params object[] args) => throw new NotImplementedException();
        public void Redo() => throw new NotImplementedException();
        public void Undo() => throw new NotImplementedException();
    }
}
