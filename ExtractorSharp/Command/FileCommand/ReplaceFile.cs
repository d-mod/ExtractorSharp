using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;

namespace ExtractorSharp.Command.ImgCommand {
    /// <summary>
    /// 替换IMG文件
    /// </summary>
    class ReplaceFile : IMutipleAciton,ICommandMessage{
        private Album OldSource,Source,Target;
        public void Do(params object[] args) {
            Target=args[0] as Album;
            Source = args[1] as Album;
            if (Target == null || Source == null) {
                return;
            }
            OldSource = new Album();
            OldSource.Replace(Target);
            Target.Replace(Source);
        }

        public void Undo() => Target.Replace(OldSource);
        

        public void Redo() => Do(Target, Source);        


        public void Action(params Album[] array){ 
            foreach (var al in array) {
                al.Replace(Source);
            }
        }

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public string Name => "ReplaceFile";
        

    }
}
