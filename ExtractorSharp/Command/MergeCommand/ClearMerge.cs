using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;

namespace ExtractorSharp.Command.SpliceCommand {
    class ClearMerge:ICommand{
        Merger Merger;
        Album[] Array;
        public void Do(params object[] args) {
            Merger = Program.Merger;
            Array = Merger.Queues.ToArray();
            Merger.Clear();
            Messager.ShowOperate("ClearMerge");
        }

        public void Undo() => Merger.Add(Array);


        public void Redo() => Do();
        
        public bool CanUndo => true;

        public bool Changed => false;

        public override string ToString() => Language.Default["ClearMerge"];
        
    }
}
