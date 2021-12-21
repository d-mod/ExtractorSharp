using System.Drawing;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {

    public class PencilDraw : ICommand {

        [CommandParameter]
        private Color Color;

        [CommandParameter]
        private Sprite Target;

        private Bitmap Image;

        [CommandParameter]
        private Point Location;

        public string Name => "Pencil";


        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this.Image = this.Target.Image;
            var width = this.Image.Width;
            var height = this.Image.Height;
            var x = 0;
            var y = 0;
            var _x = 0;
            var _y = 0;
            if(this.Location.X < 0) {
                x = -this.Location.X + 1;
                this.Location.X = 0;
                _x = x;
            } else if(this.Location.X > width - 1) {
                x = this.Location.X - width + 1;
            }

            if(this.Location.Y < 0) {
                y = -this.Location.Y + 1;
                this.Location.Y = 0;
                _y = y;
            } else if(this.Location.Y > height - 1) {
                y = this.Location.Y - height + 1;
            }

            width += x;
            height += y;
            var image = new Bitmap(width, height);
            using(var g = Graphics.FromImage(image)) {
                g.DrawImage(this.Image, _x, _y, this.Image.Width, this.Image.Height);
            }

            image.SetPixel(this.Location.X, this.Location.Y, this.Color);
            this.Target.Image = image;
            this.Target.Location = this.Target.Location.Minus(new Point(_x, _y));
        }

        public void Undo() {
            this.Target.Image = this.Image;
        }

        /*    public override string ToString() {
                return Language.Default["Pencil"];
            }*/
    }
}