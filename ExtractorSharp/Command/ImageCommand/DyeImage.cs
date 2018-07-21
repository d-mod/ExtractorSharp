using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    internal class DyeImage : ISingleAction {
        private Sprite[] _array;

        private Color _color;

        private Bitmap[] _image;

        public string Name => "Dye";

        public bool CanUndo => true;
       
        public bool IsChanged => true;

        public int[] Indices { set; get; }

        public void Action(Album album, int[] indices) {
            foreach (var i in indices) {
                if (i > -1 && i < album.List.Count) {
                    album[i].Picture = album[i].Picture.Dye(_color);
                }
            }
        }

        public void Do(params object[] args) {
            _array = args[0] as Sprite[];
            _color = (Color) args[1];
            _image = new Bitmap[_array.Length];
            for (var i = 0; i < _array.Length; i++) {
                _image[i] = _array[i].Picture;
                _array[i].Picture = _image[i].Dye(_color);
            }
        }

        public void Redo() {
            Do(_array);
        }

        public void Undo() {
            for (var i = 0; i < _array.Length; i++) {
                _array[i].Picture = _image[i];
            }
        }
    }
}