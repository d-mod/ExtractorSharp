using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Core {
    public enum QueueChangeMode {
        /// <summary>
        /// 新增
        /// </summary>
        Add,
        /// <summary>
        /// 新增多个
        /// </summary>
        AddRange,
        /// <summary>
        /// 移除
        /// </summary>
        Remove,
        /// <summary>
        /// 移除多个
        /// </summary>
        RemoveRange,
        /// <summary>
        /// 替换
        /// </summary>
        Replace,
        /// <summary>
        /// 互换
        /// </summary>
        Move,
        /// <summary>
        /// 排序
        /// </summary>
        Sort,
        /// <summary>
        /// 清空
        /// </summary>
        Clear,
        
    }
}
