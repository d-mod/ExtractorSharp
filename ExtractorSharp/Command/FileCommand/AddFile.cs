using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Command.ImgCommand {
    class AddFile : ICommand{

        private Album[] Array;

        private Album[] List;

        private bool Clear;

        private IConnector Connector => Program.Connector;

        public void Do( params object[] args) {
            Array = args[0] as Album[];
            Clear = (bool)args[1];
            if (Clear) {
                List = Connector.List.ToArray();
            }
            Connector.AddFile(Clear, Array);
        }

        public void Redo() => Do(Array,Clear);


        public void Undo() {
            Connector.RemoveFile(List);
            if (Clear) {
                Connector.AddFile(true, List);
            }
        }

        public string Name => "AddFile";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => true;
    }
}
