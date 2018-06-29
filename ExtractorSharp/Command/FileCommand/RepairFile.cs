using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExtractorSharp.Command.ImgCommand {
    /// <summary>
    /// 补帧
    /// </summary>
    class RepairFile : IMutipleAciton,ICommandMessage{
        private Album[] Array;

        private int[] Counts;

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public string Name => "RepairFile";

        public void Do(params object[] args) {
            Array = args as Album[];
            Counts = new int[Array.Length];
            var i = 0;
            Npks.Compare(Program.Config["GamePath"].Value, (a1, a2) => {
                Counts[i] = a1.List.Count - a2.List.Count;
                if (Counts[i] > 0) {
                    var source = a1.List.GetRange(a2.List.Count, Counts[i]);//获得源文件比当前文件多的贴图集合
                    source.ForEach(e => {
                        e.Load();
                        e.Parent = a2;
                    });
                    a2.List.AddRange(source);//加入到当前文件中,不修改原贴图。
                }
                i++;
            }, Array);
        }



        public void Redo() => Do(Array);


        public void Action(params Album[] array) {
            Npks.Compare(Program.Config["GamePath"].Value, (a1, a2) => {
                var count = a1.List.Count - a2.List.Count;
                if (count > 0) {
                    var source = a1.List.GetRange(a2.List.Count, count);
                    source.ForEach(e => {
                        e.Load();
                        e.Parent = a2;
                    });
                    a2.List.AddRange(source);
                }
            }, array);
        }


        public void Undo() {
            for (var i = 0; i < Array.Length; i++) {
                if (Counts[i] > 0 && Counts[i] <= Array[i].List.Count) {
                    Array[i].List.RemoveRange(Array[i].List.Count - Counts[i], Counts[i]);
                }
            }
        }

    }
}
