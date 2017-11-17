using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;

namespace ExtractorSharp.Command.ImgCommand{
    class RenameImg : ICommand {
        Album Album;
        /// <summary>
        /// 原文件名
        /// </summary>
        string oldPath;
        /// <summary>
        /// 新文件名
        /// </summary>
        string newPath;
        public void Do(params object[] args) {
            Album = args[0] as Album;
            oldPath = Album.Path;
            Album.Path = newPath = args[1] as string;//修改文件名
            Messager.ShowOperate("Rename");
        }

        public void Undo() => Album.Path = oldPath;//还原文件名
        
        public void Redo() => Do(Album, newPath);

        public void Action(params Album[] Array) {
            foreach (var item in Array) 
                Album.Path = newPath;
        }

        public bool CanUndo => true;
        

        public bool Changed => true;

        public override string ToString() => Language.Default["Rename"];

    }
}
