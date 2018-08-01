using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class AddFile : ICommand, IFileFlushable {
        private Album[] _array;

        private bool _clear;

        private Album[] List;

        private IConnector Connector => Program.Connector;

        public void Do(params object[] args) {
            _array = args[0] as Album[];
            _clear = (bool) args[1];
            if (_clear) {
                List = Connector.List.ToArray();
            }
            Connector.AddFile(_clear, _array);
        }

        public void Redo() {
            Do(_array, _clear);
        }


        public void Undo() {
            Connector.RemoveFile(_array);
            if (_clear) {
                Connector.AddFile(true, List);
            }
        }

        public string Name => "AddFile";

        public bool CanUndo => true;

        public bool IsChanged => false;

    }
}