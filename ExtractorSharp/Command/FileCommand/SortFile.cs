using ExtractorSharp.Core;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    /// <summary>
    /// 文件排序
    /// </summary>
    class SortFile : ICommand {
        private List<Album> List;
        private Controller Controller => Program.Controller;
        private IConnector Data => Program.Connector;
        public void Do(params object[] args) {
            List = new List<Album>();
            List.AddRange(Data.List);
            Data.List.Sort(Comparision);//排序
        }

        public int Comparision(Album al1, Album al2) {
            var cs1 = al1.Path.ToCharArray();
            var cs2 = al2.Path.ToCharArray();
            var i = Math.Min(cs1.Length, cs2.Length)-1;
            for (int j = 0; j < i; j++) {
                if (cs1[i] < cs2[i]) {
                    return -1;
                } else if (cs1[i] > cs2[i]) {
                    return 1;
                }
            }
            return 0;
        }

        public void Redo() => Do();
        

        public void Undo() {
            Data.List.Clear();
            Data.List.AddRange(List);//将原文件数组还原到文件列表里
        }
        
        public bool CanUndo => true;

        public bool IsFlush => true;

        public bool IsChanged => true;

        public string Name => "SortFile";
    }
}
