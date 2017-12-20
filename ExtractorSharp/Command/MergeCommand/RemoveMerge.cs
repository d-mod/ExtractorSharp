using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Command.MergeCommand {
    class RemoveMerge:ICommand{
        private Merger Merger;
        private Album[] Array;
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

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "RemoveMerge";


    }
}
