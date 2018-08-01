namespace ExtractorSharp.Exceptions {
    internal class CommandException : ApplicationException {
        public CommandException() { }

        public CommandException(string msg) : base(msg) { }
    }
}