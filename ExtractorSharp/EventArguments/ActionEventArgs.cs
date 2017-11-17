using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Command;
using ExtractorSharp.Core;

namespace ExtractorSharp.EventArguments {
    public class ActionEventArgs {
        public List<IAction> Queues { set; get; }
        /// <summary>
        /// 当前<seealso cref="IAction"/>
        /// </summary>
        public IAction Action { set; get; }
        /// <summary>
        /// 发生更改的类型
        /// </summary>
        public QueueChangeMode Mode { set; get; }
    }

}
