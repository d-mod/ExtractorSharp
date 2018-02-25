using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.MergeCommand {
    class MoveMerge : ICommand {
        public string Name => "MoveMerge";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public Merger Merger => Program.Merger;

        public List<Album> Queues => Merger.Queues;

        public int Index;

        public int Target;

        public void Do(params object[] args) {
            Index = (int)args[0];
            Target = (int)args[1];
            Move(Index, Target);
        }

        /// <summary>
        /// 互换位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="target"></param>
        public void Move(int index, int target) {
            if (index > -1 && index != target) {
                var item = Merger.Queues[index];
                Queues.RemoveAt(index);
                Queues.InsertAt(target, new Album[] { item });//插入到指定位置
                Merger.OnMergeQueueChanged();//触发序列更改事件
            }
        }

        public void Redo() => Do(Index, Target);

        public void Undo() {
            Move(Target, Index);
        }
    }
}
