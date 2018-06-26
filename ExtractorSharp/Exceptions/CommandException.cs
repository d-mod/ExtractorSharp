namespace ExtractorSharp.Exceptions {
    internal class CommandException : ProgramException {
        public CommandException() { }

        public CommandException(string msg) : base(msg) { }
    }
}