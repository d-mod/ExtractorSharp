using System.Collections.Generic;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class DeleteFile : IMutipleAciton, IFileFlushable {
        private Dictionary<Album, int> _indices;
        private static IConnector Connector => Program.Connector;

        /// <inheritdoc />
        /// <summary>
        ///     执行
        /// </summary>
        /// <param name="args"></param>
        public void Do(params object[] args) {
            var indices = (int[]) args[0];
            var array = new Album[indices.Length];
            _indices = new Dictionary<Album, int>();
            var allArray = Connector.FileArray;
            for (var i = 0; i < indices.Length; i++) {
                array[i] = allArray[indices[i]];
                _indices.Add(array[i], indices[i]);
            }

            Connector.RemoveFile(array);
        }

        public void Undo() {
            if (_indices.Count > 0) {
                foreach (var album in _indices.Keys) {
                    var index1 = _indices[album];
                    if (index1 < Connector.FileCount - 1 && index1 > -1) {
                        Connector.List.Insert(index1, album);
                    } else {
                        Connector.List.Add(album);
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     重做
        /// </summary>
        public void Redo() {
            var indices = new int[_indices.Count];
            _indices.Values.CopyTo(indices, 0);
            Do(indices);
        }

        public void Action(params Album[] array) {
            Connector.RemoveFile(array);
        }


        public bool IsChanged => true;

        public bool CanUndo => true;

        public string Name => "DeleteFile";
    }
}