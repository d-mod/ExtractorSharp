using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Draw.Brush;

namespace ExtractorSharp.Core {
    /// <summary>
    /// 绘制事件参数
    /// </summary>
    class DrawEventArgs {
        /// <summary>
        /// 当前选择的画笔
        /// </summary>
        public IBrush Brush { set; get; }

    }
}
