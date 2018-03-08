using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Data;
using System.IO;

namespace ExtractorSharp.Command.FileCommand {
    class SaveFile : IMutipleAciton {
        public string Name => "SaveFile";

        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        private Album[] Array;
        private string Dir;

        public void Action(params Album[] array) {
            foreach(var al in array) {
                al.Save($"{Dir}/{al.Name}");
            }
        }
        public void Do(params object[] args) {
            Array = args[0] as Album[];
            Dir = args[1] as string;
            Action(Array);
        }
        public void Redo() {
        }
        public void Undo() {
        }
    }
}
