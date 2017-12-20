using ExtractorSharp.Core;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Data;
using ExtractorSharp.View;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    class DeleteFile : IMutipleAciton{
        private Dictionary<Album, int> Indices;
        private Controller Controller => Program.Controller;
        private IConnector Data => Program.Connector;

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="Main"></param>
        /// <param name="args"></param>
        public void Do(params object[] args) {
            var indices = (int[])args[0];
            var array = new Album[indices.Length];
            Indices = new Dictionary<Album, int>();
            var all_array = Data.FileArray;
            for(var i = 0; i < indices.Length; i++) {
                array[i] = all_array[indices[i]];
                Indices.Add(array[i], indices[i]);
            }
            Data.RemoveFile(array);
        }

        public void Undo() {
            if (Indices.Count > 0) {
                foreach (var album in Indices.Keys) {
                    var index1 = Indices[album];
                    if (index1 < Data.FileCount - 1 && index1 > -1) {
                        Data.List.Insert(index1, album);
                    } else {
                        Data.List.Add(album);
                    }
                }
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public void Redo() {
            var indices = new int[Indices.Count];
            Indices.Values.CopyTo(indices, 0);
            Do(indices);
        }

        public void Action(params Album[] array) {
            Data.RemoveFile(array);
        }
        

        public bool IsChanged => true;

        public bool IsFlush => true;

        public bool CanUndo => true;

        public string Name => "DeleteFile";

    }
}
