using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.MergeCommand {
    class RemoveMerge:ICommand{
        Merger Merger;
        Album[] Array;
        public void Do(params object[] args) {
            Array = args as Album[];
            Merger = Program.Merger;
            Merger.Remove(Array);
            Messager.ShowOperate("RemoveMerge");
        }
        
        public void Undo() => Merger.Add(Array);

        public void Redo() => Do(Array);

        public void Action(params Album[] Array) => Merger.Remove(Array);

        public bool CanUndo => true;

        public bool Changed => false;

        public string Name => "RemoveMerge";


    }
}
