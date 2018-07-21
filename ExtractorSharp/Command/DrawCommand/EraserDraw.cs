using System.Drawing;
using System.Drawing.Drawing2D;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.DrawCommand {
    internal class EraserDraw : ICommand {
        private Color Color;
        private Sprite Entity;

        private Bitmap Image;

        private Point Location;

        private int Radius;

        public string Name => "Eraser";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public void Do(params object[] args) {
            Entity = args[0] as Sprite;
            Location = (Point) args[1];
            Color = (Color) args[2];
            Radius = (int) args[3];
            Image = Entity.Picture;
            var brush = new SolidBrush(Color);
            var point = Location.Minus(new Point(Radius, Radius));
            var rect = new Rectangle(point, new Size(Radius * 2, Radius * 2));
            var image = new Bitmap(Image.Width, Image.Height);
            using (var g = Graphics.FromImage(image)) {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.DrawImage(Image, 0, 0);
                g.FillEllipse(brush, rect);
            }

            Entity.Picture = image;
            Program.Connector.CanvasFlush();
        }

        public void Redo() {
            Do(Entity, Location, Color, Radius);
        }

        public void Undo() {
            Entity.Picture = Image;
        }

        public override string ToString() {
            return Language.Default["Eraser"];
        }
    }
}