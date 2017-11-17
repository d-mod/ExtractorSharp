using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    class DeleteImg : ICommand,MutipleAciton{
        Dictionary<Album, int> Indexes1, Indexes2;
        Controller Controller => Program.Controller;

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="Main"></param>
        /// <param name="args"></param>
        public void Do(params object[] args) {
            var Array = args as Album[];
            if (Array.Length < 1)
                return;
            Indexes1 = new Dictionary<Album, int>();
            Indexes2 = new Dictionary<Album, int>();
            foreach (var album in Array) {
                Indexes1.Add(album, Controller.List.IndexOf(album));
                Indexes2.Add(album, Controller.AlbumList.Items.IndexOf(album));
            }
            Controller.RemoveAlbum(Array);
            int index = Indexes2[Array[0]] - 1;
            index = index < 0 ? 0 : index;
            if (Controller.AlbumList.Items.Count > 0)
                Controller.AlbumList.SelectedIndex = index;
        }

        public void Undo() {
            if (Indexes1.Count > 0) {
                foreach (var album in Indexes1.Keys) {
                    var index1 = Indexes1[album];
                    var index2 = Indexes2[album];
                    if (index1 < Controller.List.Count - 1 && index1 > -1)
                        Controller.List.Insert(index1, album);
                    else
                        Controller.List.Add(album);
                    if (index2 < Controller.AlbumList.Items.Count - 1 && index2 > -1)
                        Controller.AlbumList.Items.Insert(index1, album);
                    else
                        Controller.AlbumList.Items.Add(album);
                }
            }
        }
        
        /// <summary>
        /// 重做
        /// </summary>
        public void Redo() {
            var Array = new Album[Indexes1.Count];
            Indexes1.Keys.CopyTo(Array, 0);
            Do(Array);
        }

        public void Action(params Album[] Array) {
            foreach (var album in Array) {
                Controller.List.Remove(album);
                Controller.AlbumList.Items.Remove(album);
            }
        }

        public void RunScript(string arg) { }

        public bool Changed => true;

        public bool CanUndo => true;

        public override string ToString() => Language.Default["DeleteFile"];
        
    }
}
