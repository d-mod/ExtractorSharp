using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition.Draw;
using ExtractorSharp.Core.Draw;

namespace ExtractorSharp.Draw.Paint {


    [Export(typeof(IPaint))]
    [ExportMetadata("Name", "Ruler")]
    public class Ruler : AbstractPaint {

        private readonly int rule_radius = 20;

        public bool FullCanvas { set; get; } = true;

        public bool DrawSpan { set; get; } = true;

        public bool DrawCrosshair { set; get; } = true;

        public int SmallSpan { set; get; } = 5;

        public int BigSpan { set; get; } = 200;

        public int Span { set; get; } = 50;


        public override string Name { set; get; } = "Ruler";

        public override object Tag { set; get; } = Point.Empty;

        public override bool Contains(Point point) {
            if(!this.DrawCrosshair) {
                return false;
            }
            var rp = this.Location.Minus(point);
            return rp.X * rp.X + rp.Y * rp.Y < this.rule_radius * this.rule_radius;
        }

        public override void Draw(Graphics g) {
            var rp = this.Location;
            var x = rp.X - this.rule_radius;
            var y = rp.Y - this.rule_radius;
            if(this.DrawSpan) {
                this.DrawSpans(g);
            }
            if(this.DrawCrosshair) {
                this.DrawCrosshairs(g, x, y);
            }
            g.DrawLine(Pens.White, new Point(rp.X, 0), new Point(rp.X, this.Size.Height));
            g.DrawLine(Pens.White, new Point(0, rp.Y), new Point(this.Size.Width, rp.Y));
        }

        private void DrawSpans(Graphics g) {
            var rp = this.Location;
            var font = SystemFonts.DefaultFont;

            for(var i = rp.X % this.SmallSpan; i < this.Size.Width; i += this.SmallSpan) {
                g.DrawLine(Pens.White, new Point(i, rp.Y), new Point(i, rp.Y - 5));
            }
            for(var i = 0; i < rp.X; i += this.Span) {
                var h = i % this.BigSpan == 0 ? 15 : 10;
                g.DrawString($"{-i}px", font, Brushes.White, new Point(rp.X - i, rp.Y));
                g.DrawLine(Pens.White, new Point(rp.X - i, rp.Y), new Point(rp.X - i, rp.Y - h));
            }

            for(var i = rp.X; i < this.Size.Width; i += this.Span) {
                var h = (i - rp.X) % this.BigSpan == 0 ? 15 : 10;
                g.DrawString($"{i - rp.X}px", font, Brushes.White, new Point(i, rp.Y));
                g.DrawLine(Pens.White, new Point(i, rp.Y), new Point(i, rp.Y - h));
            }

            for(var i = rp.Y % this.SmallSpan; i < this.Size.Height; i += this.SmallSpan) {
                g.DrawLine(Pens.White, new Point(rp.X, i), new Point(rp.X - 5, i));
            }
            for(var i = 0; i < rp.Y; i += this.Span) {
                var h = i % this.BigSpan == 0 ? 15 : 10;
                g.DrawString($"{-i}px", font, Brushes.White, new Point(rp.X, rp.Y - i));
                g.DrawLine(Pens.White, new Point(rp.X, rp.Y - i), new Point(rp.X - h, rp.Y - i));
            }

            for(var i = rp.Y; i < this.Size.Height; i += this.Span) {
                var h = (i - rp.Y) % this.BigSpan == 0 ? 15 : 10;
                g.DrawString($"{i - rp.Y}px", font, Brushes.White, new Point(rp.X, i));
                g.DrawLine(Pens.White, new Point(rp.X, i), new Point(rp.X - h, i));
            }
        }

        private void DrawCrosshairs(Graphics g, int x, int y) {
            g.DrawEllipse(Pens.WhiteSmoke, x, y, this.rule_radius * 2, this.rule_radius * 2);
        }

        public override string ToString() {
            var point = (Point)this.Tag;
            return $"<{this.Name}>,<Position>{this.Location.GetString()},<RealativePosition>{point.GetString()}";
        }
    }



    [Export]
    [ExportMetadata("Bind", "Ruler")]
    internal class ResetRulerItem : IPaintMenuItem {

        public string Name => "ResetRuler";

        public void OnClick(object sender, PaintMenuItemEventArgs e) {
            var layer = e.Store.Get<IPaint>("/draw/current-layer");
            e.Paint.Location = layer.Location;
        }
    }
}