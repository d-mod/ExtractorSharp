using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;

namespace ExtractorSharp.Command.FileCommand {
    internal class CutFile : ICommand {
        private Album[] _array;

        private Clipboarder _clipboarder;

        private ClipMode _mode;

        public bool CanUndo => true;

        public bool IsChanged => false;

        public string Name => "CutFile";


        public void Do(params object[] args) {
            _array = args[0] as Album[];
            _mode = (ClipMode) args[1];
            _clipboarder = Clipboarder.Default;
            Clipboarder.Default = Clipboarder.CreateClipboarder(_array, null, _mode);
            var builder = new LSBuilder();
            var dir = $"{Program.Config["RootPath"]}/temp/clipbord_img";
            if (Directory.Exists(dir)) {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
            if (_array != null) {
                var pathArr = new string[_array.Length];
                for (var i = 0; i < _array.Length; i++) {
                    pathArr[i] = $"{dir}/{_array[i].Name}";
                    _array[i].Save(pathArr[i]);
                    var jsonPath = pathArr[i].RemoveSuffix(".ogg");
                    jsonPath = jsonPath.RemoveSuffix(".img");
                    jsonPath = $"{jsonPath}.json";
                    var root = new LSObject {
                        {"path", _array[i].Path}
                    };
                    builder.Write(root, jsonPath);
                }

                var collection = new StringCollection();
                collection.AddRange(pathArr);
                Clipboard.SetFileDropList(collection);
            }
        }

        public void Redo() {
            Do(_array, _mode);
        }

        public void Undo() {
            Clipboarder.Default = _clipboarder;
        }
    }
}