namespace ExtractorSharp.Exceptions {
    internal class PluginExecption : ApplicationException {
        public PluginExecption() { }

        public PluginExecption(string msg) : base(msg) { }
    }
}