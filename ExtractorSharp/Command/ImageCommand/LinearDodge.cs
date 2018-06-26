using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    internal class LinearDodge : ISingleAction {
        private Sprite[] Array;

        private Bitmap[] Image;

        public string Name => "LinearDodge";

        public bool CanUndo => true;

        public bool IsChanged => true;

        public int[] Indices { set; get; }

        public void Action(Album album, int[] indices) {
            foreach (var i in indices) {
                if (i > -1 && i < album.List.Count) {
                    album[i].Picture = album[i].Picture.LinearDodge();
                }
            }
        }

        public void Do(params object[] args) {
            Array = args as Sprite[];
            Image = new Bitmap[Array.Length];
            for (var i = 0; i < Array.Length; i++) {
                Image[i] = Array[i].Picture;
                Array[i].Picture = Image[i].LinearDodge();
            }
        }

        public void Redo() {
            Do(Array);
        }

        public void Undo() {
            for (var i = 0; i < Array.Length; i++) Array[i].Picture = Image[i];
        }
    }
}