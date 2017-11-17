using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;

namespace ExtractorSharp.Command {
    /// <summary>
    /// 加入拼合
    /// </summary>
    class AddMerge : ICommand, MutipleAciton {
        Album[] Array;
        public void Do(params object[] args) {
            Array = args as Album[];
            Program.Merger.Add(Array);
            Messager.ShowOperate("AddSplice");
        }

        public void Undo() => Program.Merger.Remove(Array);


        public void Redo() => Do(Array);
        

        public void Action(Album[] Array) => Program.Merger.Add(Array);

        public bool CanUndo => true;

        public bool Changed => false;

        public override string ToString() => Language.Default["AddSplice"];
    }
}

