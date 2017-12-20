using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Component;

namespace ExtractorSharp.Command.FileCommand {
    /// <summary>
    /// 移动文件
    /// </summary>
    class MoveFile : IMutipleAciton {
        public string Name => "MoveFile";

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => true;

        private int Index;

        private int Target;

        private List<Album> List;


        public void Action(params Album[] array) {
            for (var i = 0; i < array.Length; i++) {
                List.Remove(array[i]);
            }
            List.InsertRange(Target, array);
        }


        public void Do(params object[] args) {
            Index = (int)args[0];
            Target = (int)args[1];
            List = Program.Connector.List;
            Move(Index, Target);
        }

        public void Redo() {
            Do(Index, Target);
        }

        private void Move(int index, int target) {
            var item = List[index];
            List.RemoveAt(index);
            List.Insert(target, item);
        }

        public void Undo() {
            Move(Target, Index);
        }
    }
}
