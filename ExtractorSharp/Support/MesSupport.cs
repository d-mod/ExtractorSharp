using ExtractorSharp.Core;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Script.Mes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Support {
    class MesSupport : IScriptSupport {
        public string Extension => ".ms";

        private MesParser mesParser;

        public MesSupport(IConnector Connector) {
            mesParser = new MesParser();
            mesParser.Executing += (sender, e) => Connector.Do(e.Name, e.Args);
            mesParser.Registry("loadFile", arg => new TokenInvokeResult { Ret = Connector.LoadFile(arg.CurrentArg as string).ToArray() });
        }


        public bool Execute(string file) {
            if (File.Exists(file)) {
                var text=File.ReadAllText(file);
                mesParser.InvokeToken(text);
                return true;
            }
            return false;
        }
    }
}
