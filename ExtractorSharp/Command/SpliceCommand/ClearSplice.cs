using ExtractorSharp.View;

namespace ExtractorSharp.Control.SpliceCommand {
    class ClearSplice:ICommand{
        Controller Controller;
        Album[] Array;
        public void Do(Controller Controller, params object[] args) {
            this.Controller = Controller;
            Array = Controller.Splice.ToArray();
            Controller.ClearSplice();
            MainForm.Message.Show(Msg_Type.Operate, "清空拼合队列成功");
        }

        public void Undo() => Controller.AddSplice(Array);


        public void Redo() => Do(Controller);
        

        public void RunScript(string arg) { }

        public void Batch(params object[] args) { }

        public bool CanUndo => true;

        public bool CanBatch => false;

        public bool isChange => false;

        public override string ToString() => "清空拼合队列";
        
    }
}
