using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    class DyeImage : ISingleAction {

        private Bitmap[] Image;

        private Sprite[] Array;

        private Color Color;

        public string Name => "Dye";

        public bool CanUndo => true;

        public bool IsFlush => false;

        public bool IsChanged => true;

        public int[] Indices { set; get; }

        public void Action(Album al, int[] indices) {
            foreach (var i in indices) {
                if (i > -1 && i < al.List.Count) {
                    al[i].Picture = al[i].Picture.Dye(Color);
                }
            }
        }

        public void Do(params object[] args) {
            Array = args[0] as Sprite[];
            Color = (Color)args[1];
            Image = new Bitmap[Array.Length];
            for (var i = 0; i < Array.Length; i++) {
                Image[i] = Array[i].Picture;
                Array[i].Picture = Image[i].Dye(Color);
            }
        }

        public void Redo() {
            Do(Array);
        }
        public void Undo() {
            for (var i = 0; i < Array.Length; i++) {
                Array[i].Picture = Image[i];
            }
        }

    }
}
