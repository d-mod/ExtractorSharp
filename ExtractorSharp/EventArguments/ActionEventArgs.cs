using System.Collections.Generic;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;

namespace ExtractorSharp.EventArguments {
    public class ActionEventArgs {
        public List<IAction> Queues { set; get; }

        /// <summary>
        ///     当前<seealso cref="IAction" />
        /// </summary>
        public IAction Action { set; get; }

        /// <summary>
        ///     发生更改的类型
        /// </summary>
        public QueueChangeMode Mode { set; get; }
    }
}