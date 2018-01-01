using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Draw.Brush {
    /// <summary>
    /// 移动工具
    /// </summary>
    class MoveTool : IBrush {
        public Cursor Cursor => Cursors.Default;

        public Color Color { set; get; }
        public int Radius { get; set; }
        public Point Location { get; set; }

        public void Draw(IPaint layer, Point newPoint, decimal scale) {
            Program.Connector.Do("moveTools", layer, Location, newPoint);
        }


    }
}
