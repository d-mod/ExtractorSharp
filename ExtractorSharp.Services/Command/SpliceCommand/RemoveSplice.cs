using ExtractorSharp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Control.SpliceCommand {
    class RemoveSplice:ICommand{
        Controller Controller;
        Album[] Array;
        public void Do(Controller Controller, params object[] args) {
            this.Controller = Controller;
            Array = (Album[])args;
            Controller.RemoveSplice(Array);
            MainForm.Message.Show(Msg_Type.Operate, "从拼合队列移除" + Array.Length + "个文件成功");
        }

        public void Batch(params object[] args) { }

        public void Undo() {
            Controller.AddSplice(Array);
        }

        public void Redo() => Do(Controller, Array);
        

        public void RunScript(string arg) { }

        public bool CanUndo => true;

        public bool CanBatch => false;

        public bool isChange => false;

        public override string ToString() => "从拼合队列移除" + Array.Length + "个文件";
        
    }
}
