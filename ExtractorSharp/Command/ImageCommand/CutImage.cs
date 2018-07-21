using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;

namespace ExtractorSharp.Command.ImageCommand {
    public class CutImage : ICommand {
        private Album _album;

        private Clipboarder _clipboarder;

        private int[] _indexes;

        private ClipMode _mode;

        public bool CanUndo => true;

        public bool IsChanged => true;
        
        public string Name => "CutImage";

        public void Do(params object[] args) {
            _album = args[0] as Album;
            _indexes = args[1] as int[];
            _mode = (ClipMode) args[2];
            _clipboarder = Clipboarder.Default;
            Clipboarder.Default = Clipboarder.CreateClipboarder(_album, _indexes, _mode);
            var arr = new string[_indexes.Length];
            var dir = $"{Program.Config["RootPath"]}/temp/clipbord_image";
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            var builder = new LSBuilder();
            for (var i = 0; i < _indexes.Length; i++) {
                var entity = _album[_indexes[i]];
                var path = $"{dir}/{entity.Index}";
                builder.WriteObject(entity, path + ".json");
                arr[i] = path + ".png";
                entity.Picture.Save(arr[i]);
            }
            var collection = new StringCollection();
            collection.AddRange(arr);
            Clipboard.SetFileDropList(collection);
        }

        public void Redo() {
            Do(_album, _indexes, _mode);
        }

        public void Undo() {
            Clipboarder.Default = _clipboarder;
        }
    }
}