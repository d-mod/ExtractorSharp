using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.ImageCommand {
    class LineDodge : ICommand ,SingleAction{
        private Bitmap[] Image;
        private ImageEntity[] Array;

        public string Name => "LineDodge";

        public bool CanUndo => true;

        public bool Changed => true;

        public int[] Indexes { set; get; }

        public void Action(Album Album, int[] Indexes) {
            foreach (var i in Indexes) {
                if (i > -1 && i < Album.List.Count) {
                    Album[i].Picture = Album[i].Picture.LinearDodge();
                }
            }
        }

        public void Do(params object[] args) {
            Array = args as ImageEntity[];
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
            for (var i = 0; i < Array.Length; i++) {
                Array[i].Picture = Image[i];
            }
        }

        public override string ToString() {
            return Language.Default["LineDodge"];
        }
    }
}
