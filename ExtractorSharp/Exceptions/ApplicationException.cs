using System;

namespace ExtractorSharp.Exceptions {
    internal abstract class ProgramException : Exception {
        public ProgramException() { }

        public ProgramException(string msg) : base(msg) { }
    }
}