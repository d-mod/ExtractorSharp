using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Control {
    class RenameCommand : ICommand {
        IName IName;
        string Name;
        string oldName;
        Controller Controller;
        public bool CanUndo => true;

        public bool CanBatch => true;
        
        public bool isChange => true;

        public void Do(Controller Controller, params object[] args) {
            this.Controller = Controller;
            var IName = args[0] as IName;
            Name = args[1] as string;
            oldName = IName.Name;
            IName.Name = Name;
        }

        public void Redo() => Do(Controller,IName,Name);
        
        public void Undo() => IName.Name = oldName;

        public void Batch(params object[] args) {

        }

        public void RunScript(string arg) => throw new NotImplementedException();
    }
}
