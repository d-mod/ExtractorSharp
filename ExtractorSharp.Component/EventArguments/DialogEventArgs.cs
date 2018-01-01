using ExtractorSharp.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp {
    /// <summary>
    /// 窗口相关事件
    /// </summary>
    public class DialogEventArgs : EventArgs {
        public ESDialog Dialog { set; get; }

        public Type DialogType { set; get; }

    }
}
