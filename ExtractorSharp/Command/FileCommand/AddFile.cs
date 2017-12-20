using ExtractorSharp.Core;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    class AddFile : ICommand{

        private Album[] Array;

        private Album[] List;

        private bool Clear;

        private Controller Controller => Program.Controller;

        private IConnector Data => Program.Connector;

        public void Do( params object[] args) {
            Array = args[0] as Album[];
            Clear = (bool)args[1];
            if (Clear) {
                List = Data.List.ToArray();
            }
            Data.AddFile(Clear, Array);
        }

        public void Redo() => Do(Array,Clear);


        public void Undo() {
            Data.RemoveFile(List);
            if (Clear) {
                Data.AddFile(true, List);
            }
        }

        public string Name => "AddFile";

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => true;
    }
}
