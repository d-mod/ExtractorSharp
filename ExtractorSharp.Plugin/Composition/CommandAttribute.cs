using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition {
    /// <summary>
    /// 绑定指令，用于button之类的控件之上
    /// </summary>
     class CommandAttribute :Attribute{
        public string Command { set; get; }
        public object[] Parameter { set; get; }

        public CommandAttribute(string Command) {
            this.Command = Command;
        }

        public CommandAttribute(string Command,params object[] args):this(Command){
            this.Parameter = args;
        }
    }
}
