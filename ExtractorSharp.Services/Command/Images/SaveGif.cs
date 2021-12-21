using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {


    [ExportCommand("SaveGif")]
    internal class SaveGif : InjectService, ISingleMacro {

        [CommandParameter("File")]
        private Album album;

        [CommandParameter]
        private int delay = 75;

        [CommandParameter]
        private string path;

        [CommandParameter]
        private Color transparent = Color.Transparent;

        [CommandParameter]
        public int[] Indices { set; get; }

        public void Action(Album album, int[] indexes) {
            var w = 1;
            var h = 1;
            var x = 800;
            var y = 600;
            for(var i = 0; i < this.Indices.Length; i++) {
                var entity = album[this.Indices[i]];
                if(entity.Width + entity.X > w) {
                    w = entity.Width + entity.X;
                }
                if(entity.Height + entity.Y > h) {
                    h = entity.Height + entity.Y;
                }
                if(entity.X < x) {
                    x = entity.X;
                }
                if(entity.Y < y) {
                    y = entity.Y;
                }
            }

            w -= x;
            h -= y;
            var array = new Bitmap[this.Indices.Length];
            for(var i = 0; i < this.Indices.Length; i++) {
                var sprite = album[this.Indices[i]];
                array[i] = new Bitmap(w, h);
                using(var g = Graphics.FromImage(array[i])) {
                    g.DrawImage(sprite.Image, sprite.X - x, sprite.Y - y);
                }
            }

            Bitmaps.WriteGif(this.path, array, this.transparent, this.delay);
            this.Messager.Success(this.Language["<SaveGif><Success>"]);
        }

        public void Do(CommandContext context) {
            context.Export(this);

            this.Action(this.album, this.Indices);
        }

    }
}