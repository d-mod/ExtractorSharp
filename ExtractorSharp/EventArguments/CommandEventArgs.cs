using ExtractorSharp.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp {
    public class CommandEventArgs : EventArgs {
        public string Name { set; get; }
        public ICommand Command { set; get; }
        public CommandEventType Type { set; get; }
    }

    public enum CommandEventType {
        Do, Undo, Redo
    }
}
