using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;

namespace ExtractorSharp.Command {
    /// <summary>
    /// 加入拼合
    /// </summary>
    class AddMerge : IMutipleAciton,ICommandMessage{
        

        Album[] Array;
        public void Do(params object[] args) {
            Array = args as Album[];
            Program.Merger.Add(Array);
        }

        public void Undo() => Program.Merger.Remove(Array);


        public void Redo() => Do(Array);
        

        public void Action(Album[] array) => Program.Merger.Add(array);

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "AddMerge";
        
    }
}

