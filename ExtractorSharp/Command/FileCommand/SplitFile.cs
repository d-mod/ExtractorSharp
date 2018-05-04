using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Command.ImgCommand {
    class SplitFile : ICommand {

        private Album[] Array;

        private List<Album> List;

        private IConnector Connector => Program.Connector;


        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => true;

        public string Name => "SplitFile";

        public void Do(params object[] args) {
            Array = args as Album[];
            List = new List<Album>();
            foreach (var al in Array) {
                var arr = Npks.SplitFile(al);
                Connector.RemoveFile(al);
                Connector.AddFile(false, arr);
                List.AddRange(arr);
            }
        }

        public void Redo() {
            Do(Array);
        }

        public void Undo() {
            Connector.RemoveFile(List.ToArray());
            Connector.AddFile(false, Array);
        }
    }
}
