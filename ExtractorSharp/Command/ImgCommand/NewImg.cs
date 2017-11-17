using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using ExtractorSharp.View;

namespace ExtractorSharp.Command.ImgCommand {
    class NewImg : ICommand,MutipleAciton{
        Controller Controller => Program.Controller;
        Album Album;
        string Path;
        int Count;
        int Index;  
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Path = args[1] as string;
            Index = Album.List.Count;
            if (args.Length > 2)
                Count = (int)args[2];
            if (args.Length > 3)
                Index = (int)args[3];
            if (Path.EndsWith(".ogg"))
                Album.Version = Img_Version.OGG;
            Album.Path = Path;
            Album.NewImage(Count, ColorBits.LINK, -1);
            Index = Controller.List.Count;
            Controller.List.Insert(Index,Album);
            if (Index < Controller.AlbumList.Items.Count)
                Controller.AlbumList.Items.Insert(Index, Album);
            else
                Controller.AlbumList.Items.Add(Album);
            Messager.ShowOperate("NewFile");
        }

        public void Undo() {
            Controller.List.Remove(Album);
            Controller.AlbumList.Items.Remove(Album);
        }

        public void RunScript(string arg) { }

        public bool Changed => true;

        public void Redo() => Do(Album, Path, Count, Index);
        

        public void Action(params Album[] Array) {
            foreach(Album Album in Array) {
                if (Path.EndsWith(".ogg"))
                    Album.Version = Img_Version.OGG;
                Album.Path = Path;
                Album.NewImage(Count, ColorBits.LINK, -1);
                Controller.List.Insert(Index, Album);
                Controller.AlbumList.Items.Insert(Index, Album);
            }
        }

        public bool CanUndo => true;

        public override string ToString() => Language.Default["NewFile"];
        

    }
}
