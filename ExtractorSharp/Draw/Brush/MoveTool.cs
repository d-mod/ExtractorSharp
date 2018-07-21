using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Draw.Brush {
    /// <summary>
    ///     移动工具
    /// </summary>
    internal class MoveTool : IBrush {
        public Color Color { set; get; }
        public string Name => "MoveTool";

        public Cursor Cursor => Cursors.Default;
        public int Radius { get; set; }
        public Point Location { get; set; }
        private Drawer Drawer => Program.Drawer;

        private IConnector Connector => Program.Connector;

        public void Draw(IPaint paint, Point newPoint, decimal scale) {
            Connector.Do("moveTools", paint, Location, newPoint);
            if (Connector.Config["AutoChangePosition"].Boolean && paint.Equals(Drawer.CurrentLayer)) {
                var sprite = Drawer.CurrentLayer.Tag as Sprite;
                if (sprite != null && paint is ExtractorSharp.Core.Draw.Paint.Canvas canvas) {
                    var album = sprite.Parent;
                    var index = sprite.Index;
                    var location = canvas.Rectangle.Location;
                    if (canvas.RealPosition) {
                        location = location.Minus(sprite.Location);
                    }
                    var x = location.X;
                    var y = location.Y;
                    canvas.Location = Point.Empty;
                    Connector.Do("changePosition", album, new int[] { index }, new int[] { x, y, 0, 0 }, new bool[] { true, true, false, false, true });
                }
            }
        }
    }
}