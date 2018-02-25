using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using System.IO;
using System.Windows.Forms;

namespace ExtractorSharp.Command.ImgCommand {
    class PasteFile : ICommand {
        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => true;

        private int Index;

        private IConnector Connector => Program.Connector;

        private Clipboarder Clipboarder;

        private int[] Indexes;

        public void Do(params object[] args) {
            Index = (int)args[0];
            Clipboarder = Clipboarder.Default;
            var array = new Album[0];
            if (Clipboarder != null) {
                array = Clipboarder.Albums;
                if (Clipboarder.Mode == ClipMode.Cut) {
                    Clipboarder.Default = null;
                    Clipboard.Clear();
                    Connector.RemoveFile(array);
                }
                for(var i=0;i<array.Length;i++) {
                    array[i] = array[i].Clone();
                }
            } else if (Clipboard.ContainsFileDropList()) {
                var collection = Clipboard.GetFileDropList();
                var file_arr = new string[collection.Count];
                collection.CopyTo(file_arr, 0);
                array = NpkReader.Load(file_arr).ToArray();
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
            if (array.Length > 0) {
                if (Connector.FileCount > 0) {
                    Connector.SelectedFileIndex = Connector.FileCount - 1;
                }
                Index = Index > Connector.List.Count ? Connector.List.Count : Index;
                Index = Index < 0 ? 0 : Index;
                Connector.List.InsertRange(Index, array);
            }
        }

        public void Redo() {
            Do(Index);
        }

        public void Undo() {
            Clipboarder.Default = Clipboarder;
            if (Clipboarder != null) {
                var array = Connector.List.GetRange(Index, Indexes.Length).ToArray();
                Connector.RemoveFile(array);
                if (Clipboarder.Mode == ClipMode.Cut) {
                    for (var i = 0; i < array.Length; i++) {
                        Connector.List.Insert(Indexes[i], array[i]);
                    }
                }
            }
        }

        public string Name => "PasteFile";
    }
}
