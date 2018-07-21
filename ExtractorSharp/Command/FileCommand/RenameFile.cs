using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class RenameFile : ICommand, ICommandMessage {
        private Album Album;

        /// <summary>
        ///     新文件名
        /// </summary>
        private string _newPath;

        /// <summary>
        ///     原文件名
        /// </summary>
        private string _oldPath;

        public void Do(params object[] args) {
            Album = args[0] as Album;
            if (Album == null) {
                return;
            }
            _oldPath = Album.Path;
            Album.Path = _newPath = args[1] as string; //修改文件名
        }

        public void Undo() {
            Album.Path = _oldPath;
        }

        public void Redo() {
            Do(Album, _newPath);
        }

        public bool CanUndo => true;


        public bool IsChanged => true;

        public string Name => "Rename";

        public void Action(params Album[] array) {
            foreach (var item in array) {
                item.Path = _newPath;
            }
        }
    }
}