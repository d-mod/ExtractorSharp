using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    ///     新建贴图
    ///     可撤销
    ///     可宏命令
    /// </summary>
    internal class NewImage : IMutipleAciton, ICommandMessage {
        private Album Album;

        private int Count;

        private int Index;

        private ColorBits Type;

        public string Name => "NewImage";

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Count = (int) args[1];
            Type = (ColorBits) args[2];
            Index = (int) args[3];
            Album.NewImage(Count, Type, Index);
        }

        public void Action(params Album[] array) {
            foreach (var al in array) {
                al.NewImage(Count, Type, Index);
            }
        }

        /// <summary>
        ///     撤销新建贴图
        /// </summary>
        public void Undo() {
            Album.List.RemoveRange(Index, Count);
        }


        public void Redo() {
            Do(Album, Count);
        }

        public bool CanUndo => true;

        public bool IsChanged => true;
    }
}