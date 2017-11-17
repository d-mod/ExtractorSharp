using ExtractorSharp.Core;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    class AddImg : ICommand{
        private Album[] Array;
        private Album[] List;
        private bool Clear;
        Controller Controller => Program.Controller;
        public void Do( params object[] args) {
            Array = args[0] as Album[];
            Clear = (bool)args[1];
            if (Clear) {
                List = Controller.List.ToArray();
            }
            Controller.AddAlbum(Clear, Array);
        }

        public void Redo() => Do(Array,Clear);


        public void Undo() {
            Controller.RemoveAlbum(List);
            if (Clear) {
                Controller.AddAlbum(true, List);
            }
        }

        public bool CanUndo => true;

        public override string ToString() => Language.Default["Add"];

        public bool Changed => false;
    }
}
