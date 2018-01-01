using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition {
    public enum ClickType {

        /// <summary>
        /// 命令，可以进行撤销和回撤
        /// </summary>
        Command,
        /// <summary>
        /// 打开一个新窗口
        /// </summary>
        View,
        /// <summary>
        /// 切换画笔
        /// </summary>
        Brush,
        /// <summary>
        /// 显示物体
        /// </summary>
        Paint
    }
}
