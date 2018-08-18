using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    internal class ChangeSize : ICommand,ICommandMessage{
        private Album _album;

        private Bitmap[] _array;

        private int[] _indexes;

        private decimal _scale;
        public bool CanUndo => true;

        public bool IsChanged => true;
       
        public string Name => "CanvasScale";

        public void Do(params object[] args) {
            _album = args[0] as Album;
            _indexes = args[1] as int[];
            _scale = (decimal) args[2];
            _array = new Bitmap[_indexes.Length];
            for (var i = 0; i < _array.Length; i++) {
                var entity = _album[_indexes[i]];
                var image = entity.Picture;
                var point = entity.Location;
                _array[i] = image;
                entity.Picture = image.Star(_scale);
                entity.Location = entity.Location.Divide(_scale);
            }
        }

        public void Redo() {
            Do(_album, _indexes, _scale);
        }

        public void Undo() {
            for (var i = 0; i < _array.Length; i++) {
                var entity = _album[_indexes[i]];
                entity.Picture = _array[i];
                entity.Location = entity.Location.Star(_scale);
            }
        }
    }
}