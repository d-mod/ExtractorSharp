﻿using System.Drawing;

namespace ExtractorSharp.Core.Draw {
    /// <summary>
    /// 可被吸引的图层
    /// </summary>
    public interface IAttractable {
        /// <summary>
        /// 被吸引的最大距离
        /// </summary>
        int Range { get; }
        /// <summary>
        /// 图层范围
        /// </summary>
        Rectangle Rectangle { get; }
    }
}
