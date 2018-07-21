using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    /// <inheritdoc cref="" />
    /// <summary>
    ///     画布化
    ///     可撤销
    ///     可宏命令
    /// </summary>
    internal class CanvasImage : ISingleAction, ICommandMessage {
        private Album _album;

        private Bitmap[] _images;

        private Point[] _locations;

        private Size _size;

        public int[] Indices { set; get; }

        public void Do(params object[] args) {
            _album = args[0] as Album;
            _size = (Size) args[1];
            Indices = args[2] as int[];
            _images = new Bitmap[0];
            _locations = new Point[0];
            if (Indices == null) {
                return;
            }
            _images = new Bitmap[Indices.Length];
            _locations = new Point[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (_album == null || Indices[i] > _album.List.Count - 1 || Indices[i] < 0) {
                    continue;
                }
                var entity = _album.List[Indices[i]];
                _images[i] = entity.Picture;
                _locations[i] = entity.Location;
                entity.CanvasImage(_size);
            }
        }

        public void Redo() {
            Do(_album, _size, Indices);
        }


        public void Undo() {
            for (var i = 0; i < Indices.Length && i < _images.Length; i++) {
                var entity = _album.List[Indices[i]];
                entity.ReplaceImage(entity.Type, false, _images[i]);
                entity.Location = _locations[i];
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach (var i in indexes) {
                if (i < album.List.Count && i > -1) {
                    album.List[i].CanvasImage(_size);
                }
            }
        }       

        public bool CanUndo => true;

        public bool IsChanged => true;

        public string Name => "CanvasImage";
    }
}