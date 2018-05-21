using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Data;
using System.IO;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Command.FileCommand {
    class SaveFile : IMutipleAciton,ICommandMessage{
        public string Name => "SaveFile";

        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        private Album[] Array;
        private string Path;
        private int Type;

        private const int SAVE_TO_DIR = 0X00;
        private const int SAVE_TO_IMG = 0x01;
        private const int SAVE_TO_NPK = 0X02;

        public void Action(params Album[] array) {
            switch(Type){
                case SAVE_TO_DIR:
                    Npks.SaveToDirectory(Path, array);
                    break;
                case SAVE_TO_IMG:
                    if (array.Length > 0) {
                        array[0].Save(Path);
                    }
                    break;
                case SAVE_TO_NPK:
                    Npks.Save(Path, new List<Album>(array));
                    break;
            }
        }
        public void Do(params object[] args) {
            Array = args[0] as Album[];
            Path = args[1] as string;
            if (args.Length > 2) {
                Type = (int)args[2];
            }
            Action(Array);
        }
        public void Redo() {
        }
        public void Undo() {
        }
    }
}
