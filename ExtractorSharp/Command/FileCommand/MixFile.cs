using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class MixFile : ICommand, IFileFlushable {
        private Album _album;
        private Album[] _array;

        private static IConnector Connector => Program.Connector;

        public void Do(params object[] args) {
            _array = args as Album[];
            if (_array == null || _array.Length <= 0) {
                return;
            }
            var regex = new Regex("\\d+");
            var match = regex.Match(_array[0].Name);
            if (!match.Success) {
                return;
            }
            var code = int.Parse(match.Value);
            var codeStr = NpkCoder.CompleteCode(code / 100 * 100);
            _album = _array[0].Clone();
            _album.ConvertTo(ImgVersion.Ver6);
            _album.Adjust();
            _album.Tables.Clear();
            _album.Path = _array[0].Path.Replace(match.Value, codeStr);
            var max = 0;
            var table = new List<Color>();
            foreach (var al in _array) {
                if (al.CurrentTable.Count > max) {
                    max = al.CurrentTable.Count;
                    table = al.CurrentTable;
                }
                _album.Tables.Add(al.CurrentTable);
            }
            foreach (var tl in _album.Tables) {
                if (tl.Count < max) {
                    tl.AddRange(table.GetRange(tl.Count, max - tl.Count));
                }
            }
            Connector.RemoveFile(_array);
            Connector.AddFile(false, _album);
        }

        public void Redo() {
            Do(_array);
        }

        public void Undo() {
            Connector.RemoveFile(_album);
            Connector.AddFile(false, _array);
        }


        public bool CanUndo => true;

        public bool IsChanged => true;

        public string Name => "MixFile";
    }
}