using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    internal class SaveGif : ISingleAction, ICommandMessage {
        private Album Album;
        private int Delay = 75;
        private string Path;
        private Color Transparent = Color.Transparent;
        public int[] Indices { set; get; }

        public string Name => "SaveGif";

        public bool CanUndo => false;

        public bool IsChanged => false;

        public void Action(Album album, int[] indexes) {
            var w = 1;
            var h = 1;
            var x = 800;
            var y = 600;
            for (var i = 0; i < Indices.Length; i++) {
                var entity = album[Indices[i]];
                if (entity.Width + entity.X > w) {
                    w = entity.Width + entity.X;
                }
                if (entity.Height + entity.Y > h) {
                    h = entity.Height + entity.Y;
                }
                if (entity.X < x) {
                    x = entity.X;
                }
                if (entity.Y < y) {
                    y = entity.Y;
                }
            }

            w -= x;
            h -= y;
            var array = new Bitmap[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                var sprite = album[Indices[i]];
                array[i] = new Bitmap(w, h);
                using (var g = Graphics.FromImage(array[i])) {
                    g.DrawImage(sprite.Picture, sprite.X - x, sprite.Y - y);
                }
            }

            Bitmaps.WriteGif(Path, array, Transparent, Delay);
        }

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indices = args[1] as int[];
            Path = args[2] as string;
            if (args.Length > 4) {
                Transparent = (Color) args[3];
                Delay = (int) args[4];
            }

            Action(Album, Indices);
        }

        public void Redo() { }

        public void Undo() {
            //empty
        }
    }
}