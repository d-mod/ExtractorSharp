using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Command.MergeCommand {
    class ClearMerge:ICommand,ICommandMessage{
        private Merger Merger;
        private Album[] Array;
        public void Do(params object[] args) {
            Merger = Program.Merger;
            Array = Merger.Queues.ToArray();
            Merger.Clear();
        }

        public void Undo() => Merger.Add(Array);


        public void Redo() => Do();
        
        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "ClearMerge";
        
    }
}
