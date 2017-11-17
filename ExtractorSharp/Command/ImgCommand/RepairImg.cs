using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExtractorSharp.Command.ImgCommand {
    /// <summary>
    /// 补帧
    /// </summary>
    class RepairImg : ICommand,MutipleAciton{
        Album[] Array;
        int[] Counts;
        public bool CanUndo => true;
        public bool Changed => true;


        public void Do(params object[] args) {
            Array = args as Album[];
            Counts = new int[Array.Length];
            var i = 0;
            Tools.GetOriginal( (a1,a2) => {
                Counts[i] = a1.List.Count - a2.List.Count;
                if (Counts[i] > 0) {
                    var source = a1.List.GetRange(a2.List.Count, Counts[i]);//获得源文件比当前文件多的贴图集合
                    a2.List.AddRange(source);//加入到当前文件中,不修改原贴图。
                    a2.List.ForEach(item => item.Parent = a2);
                }
                i++;        
            }, Array);          
        }



        public void Redo() => Do(Array);


        public void Action(params Album[] Array) {
            Tools.GetOriginal((a1, a2) => {
                var count = a1.List.Count - a2.List.Count;
                if (count > 0)
                    a2.List.AddRange(a1.List.GetRange(a2.List.Count, count));
            }, Array);
        }


        public void Undo() {
            for (var i = 0; i < Array.Length; i++)
                if (Counts[i] > 0)
                    Array[i].List.RemoveRange(Array[i].List.Count - Counts[i], Counts[i]);
        }

        public override string ToString() => Language.Default["RepairImg"];
    }
}
