using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core.Draw;

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

        public void Draw(IPaint layer, Point newPoint, decimal scale) {
            Program.Connector.Do("moveTools", layer, Location, newPoint);
        }
    }
}