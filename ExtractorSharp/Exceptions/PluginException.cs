namespace ExtractorSharp.Exceptions {
    internal class PluginExecption : ProgramException {
        public PluginExecption() { }

        public PluginExecption(string msg) : base(msg) { }
    }
}