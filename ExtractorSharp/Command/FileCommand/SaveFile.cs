using System.Collections.Generic;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class SaveFile : IMutipleAciton, ICommandMessage {
        private const int SaveToDir = 0X00;
        private const int SaveToImg = 0x01;
        private const int SaveToNpk = 0X02;

        private Album[] _array;
        private string _path;
        private int _type;
        public string Name => "SaveFile";

        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public void Action(params Album[] array) {
            switch (_type) {
                case SaveToDir:
                    NpkCoder.SaveToDirectory(_path, array);
                    break;
                case SaveToImg:
                    if (array.Length > 0) array[0].Save(_path);
                    break;
                case SaveToNpk:
                    NpkCoder.Save(_path, new List<Album>(array));
                    break;
            }
        }

        public void Do(params object[] args) {
            _array = args[0] as Album[];
            _path = args[1] as string;
            if (args.Length > 2) {
                _type = (int) args[2];
            }
            Action(_array);
        }

        public void Redo() { }

        public void Undo() { }
    }
}