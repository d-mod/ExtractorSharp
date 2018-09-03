using System;

namespace ExtractorSharp.Exceptions {
    internal class ConfigException : ApplicationException {
        public ConfigException() { }

        public ConfigException(string msg) : base(msg) { }
    }
}