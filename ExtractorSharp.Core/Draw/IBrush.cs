using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Draw.Brush{
    /// <summary>
    /// 画笔
    /// </summary>
    public interface IBrush {
        /// <summary>
        /// 鼠标图标
        /// </summary>
        /// 
        Cursor Cursor { get; }
        /// <summary>
        /// 作用半径
        /// </summary>
        int Radius { set; get; }
        /// <summary>
        /// 坐标
        /// </summary>
        Point Location { set; get; }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="map">图片</param>
        /// <param name="point">画笔经过的路径</param>
        /// <returns></returns>
        void Draw(IPaint layer,Point point,decimal scale);
    }
}
