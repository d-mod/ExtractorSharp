using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Exceptions {
    class CommandException : ProgramException {
        public CommandException() {

        }
        public CommandException(string msg) : base(msg) {

        }
    }
}
