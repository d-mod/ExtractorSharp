using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;

namespace ExtractorSharp.Command.ImageCommand {
    class SaveGif : ISingleAction, ICommandMessage {
        public int[] Indices { set; get; }

        public string Name => "SaveGif";

        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        private Album Album;
        private string Path;
        private Color Transparent = Color.Transparent;
        private int Delay = 75;

        public void Action(Album Album, int[] indexes) {
            var w = 1;
            var h = 1;
            var x = 800;
            var y = 600;
            for (var i = 0; i < Indices.Length; i++) {
                var entity = Album[Indices[i]];
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
                var sprite = Album[Indices[i]];
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
                Transparent = (Color)args[3];
                Delay = (int)args[4];
            }
            Action(Album, Indices);
        }

        public void Redo() {

        }
        public void Undo() {
            //empty
        }
    }
}
