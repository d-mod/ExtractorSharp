using System;

namespace ExtractorSharp.Exceptions {
    public class CommandException : ApplicationException {
        public CommandException() { }

        public CommandException(string msg) : base(msg) { }
    }
}