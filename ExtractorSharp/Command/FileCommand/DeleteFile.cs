using ExtractorSharp.Core;
using ExtractorSharp.Data;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    class DeleteFile : IMutipleAciton{
        private Dictionary<Album, int> Indices;
        private IConnector Connector => Program.Connector;

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="Main"></param>
        /// <param name="args"></param>
        public void Do(params object[] args) {
            var indices = (int[])args[0];
            var array = new Album[indices.Length];
            Indices = new Dictionary<Album, int>();
            var all_array = Connector.FileArray;
            for(var i = 0; i < indices.Length; i++) {
                array[i] = all_array[indices[i]];
                Indices.Add(array[i], indices[i]);
            }
            Connector.RemoveFile(array);
        }

        public void Undo() {
            if (Indices.Count > 0) {
                foreach (var album in Indices.Keys) {
                    var index1 = Indices[album];
                    if (index1 < Connector.FileCount - 1 && index1 > -1) {
                        Connector.List.Insert(index1, album);
                    } else {
                        Connector.List.Add(album);
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
            Connector.RemoveFile(array);
        }
        

        public bool IsChanged => true;

        public bool IsFlush => true;

        public bool CanUndo => true;

        public string Name => "DeleteFile";

    }
}
