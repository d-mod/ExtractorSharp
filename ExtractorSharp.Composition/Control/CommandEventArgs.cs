using System;

namespace ExtractorSharp.Composition.Control {

    public class CommandEventArgs : EventArgs {

        public string Name { set; get; }

        public ICommand Command { set; get; }

        public ICommandMetadata Metadata { set; get; }

        public CommandContext Context { set; get; }

        public CommandEventType Type { set; get; }

    }

    public enum CommandEventType {

        Do,

        Undo,

        Redo,

        Clear

    }
}