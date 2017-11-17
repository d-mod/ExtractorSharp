using ExtractorSharp.View;

namespace ExtractorSharp.Control {
    /// <summary>
    /// 加入拼合
    /// </summary>
    class AddSplice : ICommand {
        Controller Controller;
        Album[] Array;
         public void Do(Controller Controller, params object[] args) {
            this.Controller = Controller;
            Array = (Album[])args;
            Controller.AddSplice(Array);
            MainForm.Message.Show(Msg_Type.Operate, "加入" + Array.Length + "个文件到拼合队列成功");
        }

        public void Undo() => Controller.RemoveSplice(Array);
        

        public void Redo() {
            Do(Controller,Array);
        }

        public void Batch(params object[] args) { }

        public void RunScript(string arg) { }


        public bool CanUndo => true;

        public bool CanBatch => false;

        public bool isChange => false;

        public override string ToString() => "加入" + Array.Length + "个文件到拼合队列";        
    }
}
