using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;

namespace ExtractorSharp.Command.ImgCommand {
    class NewFile : IMutipleAciton {
        private Album Album;

        private string Path;

        private int Count;

        private int Index;

        private IConnector Connector => Program.Connector;

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
            Index = Connector.List.Count;
            Connector.List.Insert(Index, Album);
            Messager.ShowOperate("NewFile");
        }

        public void Undo() {
            Connector.List.Remove(Album);
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
                Connector.List.Insert(Index, Album);
            }
        }

        public bool IsFlush => true;

        public bool CanUndo => true;

        public string Name => "NewFile";
        

    }
}
