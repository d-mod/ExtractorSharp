using ExtractorSharp.Core;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;

namespace ExtractorSharp.Command.ImgCommand {
    class NewFile : IMutipleAciton {
        private Controller Controller => Program.Controller;

        private Album Album;

        private string Path;

        private int Count;

        private int Index;

        private ICommandData Data => Program.Data;

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Path = args[1] as string;
            Index = Album.List.Count;
            if (args.Length > 2) {
                Count = (int)args[2];
            }
            if (args.Length > 3) {
                Index = (int)args[3];
            }
            if (Path.EndsWith(".ogg")) {
                Album.Version = Img_Version.OGG;
            }
            Album.Path = Path;
            Album.NewImage(Count, ColorBits.LINK, -1);
            Index = Data.List.Count;
            Data.List.Insert(Index, Album);
            Messager.ShowOperate("NewFile");
        }

        public void Undo() {
            Data.List.Remove(Album);
        }
        

        public bool IsChanged => true;

        public void Redo() => Do(Album, Path, Count, Index);
        

        public void Action(params Album[] array) {
            foreach(Album al in array) {
                if (Path.EndsWith(".ogg")) {
                    Album.Version = Img_Version.OGG;
                }
                Album.Path = Path;
                Album.NewImage(Count, ColorBits.LINK, -1);
                Data.List.Insert(Index, Album);
            }
        }

        public bool IsFlush => true;

        public bool CanUndo => true;

        public string Name => "NewFile";
        

    }
}
