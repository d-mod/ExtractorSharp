using ExtractorSharp.Core;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    /// <summary>
    /// 文件分类
    /// </summary>
    class SortImg : ICommand {
        List<Album> List;
        List<Album> oldList;
        Controller Controller => Program.Controller;
        public void Do(params object[] args) {
            oldList = new List<Album>();
            oldList.AddRange(Controller.List);
            List = Controller.List;
            List.Sort(Comparision);//排序
        }

        public int Comparision(Album al1, Album al2) {
            var cs1 = al1.Path.ToCharArray();
            var cs2 = al2.Path.ToCharArray();
            var i = Math.Min(cs1.Length, cs2.Length)-1;
            for (int j = 0; j < i; j++) {
                if (cs1[i] < cs2[i])
                    return -1;
                else if (cs1[i] > cs2[i])
                    return 1;
            }
            return 0;
        }

        public void Redo() => Do();
        

        public void Undo() {
            Controller.List.Clear();
            Controller.List.AddRange(oldList);//将原文件数组还原到文件列表里
            Controller.AlbumList.Items.Clear();
            Controller.AlbumList.Items.AddRange(oldList.ToArray());
        }

        public void Batch(params object[] args) { }

        public bool CanUndo => true;

        public void RunScript(string arg) { }

        public bool Changed => true;

        public override string ToString() => Language.Default["Sort"];
    }
}
