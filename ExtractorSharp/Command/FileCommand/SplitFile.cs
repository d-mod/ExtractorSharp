using System.Collections.Generic;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class SplitFile : ICommand, IFileFlushable {
        private Album[] _array;

        private List<Album> _list;

        private static IConnector Connector => Program.Connector;

        public bool CanUndo => true;

        public bool IsChanged => false;       

        public string Name => "SplitFile";

        public void Do(params object[] args) {
            _array = args as Album[];
            _list = new List<Album>();
            if (_array == null) {
                return;
            }
            foreach (var al in _array) {
                var arr = NpkCoder.SplitFile(al);
                Connector.RemoveFile(al);
                Connector.AddFile(false, arr);
                _list.AddRange(arr);
            }
        }

        public void Redo() {
            Do(_array);
        }

        public void Undo() {
            Connector.RemoveFile(_list.ToArray());
            if (_array == null) {
                return;
            }
            Connector.AddFile(false, _array);
        }
    }
}