using System.IO;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;

namespace ExtractorSharp.Command.FileCommand {
    internal class PasteFile : ICommand, IFileFlushable {
        private Clipboarder _clipboarder;

        private int _index;

        private int[] _indexes;

        private static IConnector Connector => Program.Connector;
        public bool CanUndo => true;

        public bool IsChanged => true;

        public void Do(params object[] args) {
            _index = (int) args[0];
            _clipboarder = Clipboarder.Default;
            var array = new Album[0];
            if (_clipboarder != null) {
                array = _clipboarder.Albums;
                if (_clipboarder.Mode == ClipMode.Cut) {
                    Clipboarder.Clear();
                    Clipboard.Clear();
                    Connector.RemoveFile(array);
                }
                for (var i = 0; i < array.Length; i++) {
                    array[i] = array[i].Clone();
                }
            } else if (Clipboard.ContainsFileDropList()) {
                var collection = Clipboard.GetFileDropList();
                var fileArr = new string[collection.Count];
                collection.CopyTo(fileArr, 0);
                array = NpkCoder.Load(fileArr).ToArray();
                var builder = new LSBuilder();
                for (var i = 0; i < array.Length; i++) {
                    var name = fileArr[i].RemoveSuffix(".img");
                    name = name.RemoveSuffix(".ogg");
                    name += ".json";
                    if (File.Exists(name)) {
                        var root = builder.Read(name)["path"];
                        var path = root.Value?.ToString();
                        if (path != null) array[i].Path = path;
                    }
                }
            }

            _indexes = new int[array.Length];
            if (array.Length > 0) {
                if (Connector.FileCount > 0) {
                    Connector.SelectedFileIndex = Connector.FileCount - 1;
                }
                _index = _index > Connector.List.Count ? Connector.List.Count : _index;
                _index = _index < 0 ? 0 : _index;
                for (var i = 0; i < array.Length; i++) {
                    _indexes[i] = _index + i;
                }
                Connector.List.InsertRange(_index, array);
            }
        }

        public void Redo() {
            Do(_index);
        }

        public void Undo() {
            Clipboarder.Default = _clipboarder;
            if (_clipboarder == null) {
                return;
            }
            var array = Connector.List.GetRange(_index, _indexes.Length).ToArray();
            Connector.RemoveFile(array);
            if (_clipboarder.Mode != ClipMode.Cut) {
                return;
            }
            for (var i = 0; i < array.Length; i++) {
                Connector.List.Insert(_indexes[i], array[i]);
            }
        }

        public string Name => "PasteFile";
    }
}