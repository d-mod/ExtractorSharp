using System.Drawing;
using System.Drawing.Drawing2D;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {


    public class EraserDraw : ICommand {

        [CommandParameter]
        private Color Color;

        [CommandParameter]
        private Sprite Target;

        private Bitmap Image;

        [CommandParameter]
        private Point Location;

        [CommandParameter]
        private int Radius;

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this.Image = this.Target.Image;
            var brush = new SolidBrush(this.Color);
            var point = this.Location.Minus(new Point(this.Radius, this.Radius));
            var rect = new Rectangle(point, new Size(this.Radius * 2, this.Radius * 2));
            var image = new Bitmap(this.Image.Width, this.Image.Height);
            using(var g = Graphics.FromImage(image)) {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.DrawImage(this.Image, 0, 0);
                g.FillEllipse(brush, rect);
            }

            this.Target.Image = image;
        }

        public void Undo() {
            this.Target.Image = this.Image;
        }
    }
}