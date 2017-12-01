using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.ImgCommand {
    class SplitFile : ICommand {
        private Album[] Array;
        private List<Album> List;
        private Controller Controller => Program.Controller;
        public bool CanUndo => true;

        public bool Changed => false;

        public string Name => "SplitFile";

        public void Do(params object[] args) {
            Array = args as Album[];
            List = new List<Album>();
            foreach (var al in Array) {
                var arr = new Album[al.Tables.Count];
                var path = al.Name;
                var regex = new Regex("\\d+");
                var match = regex.Match(path);
                if (!match.Success) {
                    continue;
                }
                var prefix = path.Substring(0, match.Index);
                var suffix = path.Substring(match.Index + match.Length);
                var code = int.Parse(match.Value);
                al.Adjust();
                var data = al.Data;
                var ms = new MemoryStream(data);
                for (var i = 0; i < arr.Length; i++) {
                    var name = prefix + (code + i).toCodeString() + suffix;
                    arr[i] = ms.ReadNPK(al.Name)[0];
                    arr[i].Path = al.Path.Replace(al.Name, name);
                    arr[i].Tables.Clear();
                    arr[i].Tables.Add(al.Tables[i]);
                    ms.Seek(0, SeekOrigin.Begin);
                }
                ms.Close();
                Controller.RemoveAlbum(al);
                Controller.AddAlbum(false, arr);
                List.AddRange(arr);
            }
        }

        public void Redo() {
            Do(Array);
        }

        public void Undo() {
            Controller.RemoveAlbum(List.ToArray());
            Controller.AddAlbum(false, Array);
        }
    }
}
