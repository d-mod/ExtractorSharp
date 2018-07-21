using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class NewFile : IMutipleAciton, ICommandMessage, IFileFlushable {
        private Album album;

        private int count;

        private int index;

        private string path;

        private static IConnector Connector => Program.Connector;

        public void Do(params object[] args) {
            album = args[0] as Album ?? new Album();
            path = args[1] as string;
            index = album.List.Count;
            if (args.Length > 2) {
                count = (int)args[2];
            }
            if (args.Length > 3) {
                index = (int)args[3];
            }
            if (args.Length > 4) {
                album.Version = (ImgVersion)(args[4]);
            }
            album.Path = path;
            album.NewImage(count, ColorBits.LINK, -1);
            Connector.List.Insert(index + 1, album);
        }

        public void Undo() {
            Connector.List.Remove(album);
        }


        public bool IsChanged => true;

        public void Redo() {
            Do(album, path, count, index);
        }


        public void Action(params Album[] array) {
            foreach (var al in array) {
                if (path.EndsWith(".ogg")) album.Version = ImgVersion.Other;
                album.Path = path;
                album.NewImage(count, ColorBits.LINK, -1);
                Connector.List.Insert(index, album);
            }
        }

        public bool CanUndo => true;

        public string Name => "NewFile";
    }
}