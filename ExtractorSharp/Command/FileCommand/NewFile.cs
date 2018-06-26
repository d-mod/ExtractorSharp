using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class NewFile : IMutipleAciton, ICommandMessage, IFileFlushable {
        private Album _album;

        private int _count;

        private int _index;

        private string _path;

        private static IConnector Connector => Program.Connector;

        public void Do(params object[] args) {
            _album = args[0] as Album ?? new Album();
            _path = args[1] as string;
            _index = _album.List.Count;
            if (args.Length > 2) {
                _count = (int) args[2];
            }
            if (args.Length > 3) {
                _index = (int) args[3];
            }
            if (_path != null && _path.EndsWith(".ogg")) {
                _album.Version = ImgVersion.Other;
            }
            _album.Path = _path;
            _album.NewImage(_count, ColorBits.Link, -1);
            _index = Connector.List.Count;
            Connector.List.Insert(_index, _album);
        }

        public void Undo() {
            Connector.List.Remove(_album);
        }


        public bool IsChanged => true;

        public void Redo() {
            Do(_album, _path, _count, _index);
        }


        public void Action(params Album[] array) {
            foreach (var al in array) {
                if (_path.EndsWith(".ogg")) _album.Version = ImgVersion.Other;
                _album.Path = _path;
                _album.NewImage(_count, ColorBits.Link, -1);
                Connector.List.Insert(_index, _album);
            }
        }

        public bool CanUndo => true;

        public string Name => "NewFile";
    }
}