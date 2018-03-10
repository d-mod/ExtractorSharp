using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Exceptions {
    class PluginExecption : ProgramException {

        public PluginExecption() {

        }
        public PluginExecption(string msg):base(msg){

        }
    }
}
