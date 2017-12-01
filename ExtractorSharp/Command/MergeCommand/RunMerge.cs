using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.MergeCommand {
    class RunMerge : ICommand {
        public bool CanUndo => false;

        public bool Changed => false;

        public void Do(params object[] args) =>Program.Merger.RunMerge();

        public void Redo() { }
        public void Undo() { }

        public string Name => "RunMerge";
    }
}
