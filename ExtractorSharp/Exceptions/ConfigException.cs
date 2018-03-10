using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Exceptions {
    class ConfigException : ProgramException {
        public ConfigException() {

        }
        public ConfigException(string msg) : base(msg) {

        }
    }
}
