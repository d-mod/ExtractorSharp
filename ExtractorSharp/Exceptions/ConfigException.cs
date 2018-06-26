namespace ExtractorSharp.Exceptions {
    internal class ConfigException : ProgramException {
        public ConfigException() { }

        public ConfigException(string msg) : base(msg) { }
    }
}