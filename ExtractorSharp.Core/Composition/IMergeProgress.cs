using ExtractorSharp.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExtractorSharp.Core.Lib.Avatars;

namespace ExtractorSharp.Core.Composition {
    /// <summary>
    ///  拼合进度接口
    /// </summary>
    public interface IMergeProgress {


        /// <summary>
        ///     启动拼合
        /// </summary>
        event MergeHandler MergeStarted;

        /// <summary>
        ///     拼合进行
        /// </summary>
        event MergeHandler MergeProcessing;

        /// <summary>
        ///     拼合完成
        /// </summary>
        event MergeHandler MergeCompleted;

        void OnMergeStarted(MergeEventArgs e);

        void OnMergeProcessing(MergeEventArgs e);

        void OnMergeCompleted(MergeEventArgs e);
    }
}
