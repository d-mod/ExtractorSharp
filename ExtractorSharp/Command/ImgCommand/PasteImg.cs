using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Command.ImgCommand {
    class PasteImg : ICommand {
        public bool CanUndo => true;

        public bool Changed => true;

        private int Index;

        private Controller Controller => Program.Controller;

        private Clipboarder Clipboarder;

        private Album[] Array;

        private int[] Indexes;

        public void Do(params object[] args) {
            Index = (int)args[0];
            Clipboarder = Controller.Clipboarder;
            var array = new Album[0];
            if (Clipboarder != null) {
                Array = Clipboarder.Albums;
                array = new Album[Array.Length];
                Indexes = new int[Array.Length];
                for (var i = 0; i < array.Length; i++) {
                    array[i] = Array[i].Clone();
                }
                if (Clipboarder.Mode == ClipMode.Cut) {
                    Controller.Clipboarder = null;
                    Clipboard.Clear();
                    Controller.RemoveAlbum(Array);
                }
            } else if (Clipboard.ContainsFileDropList()) {
                var collection = Clipboard.GetFileDropList();
                var file_arr = new string[collection.Count];
                collection.CopyTo(file_arr, 0);
                array = Tools.Load(file_arr).ToArray();
                var builder = new LSBuilder();
                for (var i = 0; i < array.Length; i++) {
                    var name = file_arr[i].RemoveSuffix(".img");
                    name = name.RemoveSuffix(".ogg");
                    name += ".json";
                    if (File.Exists(name)) {
                        var root = builder.Read(name)["path"];
                        var path = root.Value?.ToString();
                        if (path != null) {
                            array[i].Path = path;
                        }
                    }
                }
            }
            Controller.AddAlbum(false, Index, array);
        }

        public void Redo() {
            Do(Index);
        }

        public void Undo() {
            Controller.Clipboarder = Clipboarder;
            if (Clipboarder != null) {
                var array = Controller.List.GetRange(Index, Indexes.Length).ToArray();
                Controller.RemoveAlbum(array);
                if (Clipboarder.Mode == ClipMode.Cut) {
                    for (var i = 0; i < array.Length; i++) {
                        Controller.AddAlbum(false, Indexes[i], array[i]);
                    }
                }
            }
        }

        public override string ToString() => Language.Default["Paste"];
    }
}
