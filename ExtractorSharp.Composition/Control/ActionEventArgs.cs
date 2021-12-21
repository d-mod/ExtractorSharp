using System.Collections.Generic;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;

namespace ExtractorSharp.EventArguments {
    public class ActionEventArgs {
        public List<IMacro> Queues { set; get; }

        /// <summary>
        ///     当前<seealso cref="IMacro" />
        /// </summary>
        public IMacro Action { set; get; }

        /// <summary>
        ///     发生更改的类型
        /// </summary>
        public QueueChangeMode Mode { set; get; }
    }
}