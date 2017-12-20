
using ExtractorSharp.Core.Control;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Command.ImgCommand {
    class MixFile : ICommand {
        private Album[] Array;

        private Album Album;

        private Controller Controller => Program.Controller;

        private IConnector Data => Program.Connector;

        public void Do(params object[] args) {
            Array = args as Album[];
            if (Array.Length > 0) {
                var regex = new Regex("\\d+");
                var match = regex.Match(Array[0].Name);
                if (match.Success) {
                    var code = int.Parse(match.Value);
                    var code_str = (code / 100 * 100).toCodeString();
                    Album = Array[0].Clone();
                    Album.ConvertTo(Handle.Img_Version.Ver6);
                    Album.Tables.Clear();
                    Album.Path = Array[0].Path.Replace(match.Value, code_str);
                    var max = 0;
                    var table = new List<Color>();
                    foreach (var al in Array) {
                        if (al.CurrentTable.Count > max) {
                            max = al.CurrentTable.Count;
                            table = al.CurrentTable;
                        }
                        Album.Tables.Add(al.CurrentTable);
                    }
                    foreach(var tl in Album.Tables) {
                        if (tl.Count < max) {
                            tl.AddRange(table.GetRange(tl.Count, max - tl.Count));
                        }
                    }
                    Data.RemoveFile(Array);
                    Data.AddFile(false, Album);
                }
            }
        }

        public void Redo() {
            Do(Array);
        }

        public void Undo() {
            Data.RemoveFile(Album);
            Data.AddFile(false, Array);
        }


        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => true;

        public string Name => "MixFile";
    }
}
