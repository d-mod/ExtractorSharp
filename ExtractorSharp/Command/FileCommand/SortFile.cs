using System;
using System.Collections.Generic;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    /// <inheritdoc />
    /// <summary>
    ///     文件排序
    /// </summary>
    internal class SortFile : ICommand,IFileFlushable {
        private List<Album> _list;
        private static IConnector Connector => Program.Connector;

        public void Do(params object[] args) {
            _list = new List<Album>();
            _list.AddRange(Connector.List);
            Connector.List.Sort(Comparision); //排序
        }

        public void Redo() {
            Do();
        }


        public void Undo() {
            Connector.List.Clear();
            Connector.List.AddRange(_list); //将原文件数组还原到文件列表里
        }

        public bool CanUndo => true;

        public bool IsChanged => true;

        public string Name => "SortFile";

        public int Comparision(Album al1, Album al2) {
            var cs1 = al1.Path.ToCharArray();
            var cs2 = al2.Path.ToCharArray();
            var i = Math.Min(cs1.Length, cs2.Length) - 1;
            for (var j = 0; j < i; j++) {
                if (cs1[i] < cs2[i]) {
                    return -1;
                }
                if (cs1[i] > cs2[i]) {
                    return 1;
                }
            }
            return 0;
        }
    }
}