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

        public int Index;

        public int Target;

        public void Do(params object[] args) {
            Index = (int)args[0];
            Target = (int)args[1];
            Merger.Move(Index, Target);
        }


        public void Redo() => Do(Index, Target);

        public void Undo() {
            Merger.Move(Target, Index);
        }
    }
}
