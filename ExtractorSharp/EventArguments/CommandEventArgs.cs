using System;
using ExtractorSharp.Core.Command;

namespace ExtractorSharp.EventArguments {
    public class CommandEventArgs : EventArgs {
        public string Name { set; get; }
        public ICommand Command { set; get; }
        public CommandEventType Type { set; get; }
    }

    public enum CommandEventType {
        Do,
        Undo,
        Redo,
        Clear
    }
}