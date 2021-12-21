using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Script.Mes;

namespace ExtractorSharp.Services.Supports {

    [Export]
    public class MesSupport : InjectService, IOpenSupport {

        public string Extension => ".ms";

        private MesParser MesParser { get; }

        public MesSupport() {
            this.MesParser = new MesParser();
            this.MesParser.Executing += (object sender, ExcutingEventArgs e) => this.Controller.Do(e.Name, e.Args);
            this.MesParser.Registry("loadFile", arg => {
                this.Controller.Do("loadFile", arg.CurrentArg);
                this.Store.Get("loads", out List<Album> loads);
                return new TokenInvokeResult { Ret = loads };
            });
            this.MesParser.Registry("message", arg => {
                switch(arg.CurrentArg) {
                    case object[] argArray:
                        this.Messager.Send(string.Join(",", argArray.Select(x => x.ToString())));
                        break;
                    default:
                        this.Messager.Send(arg.ToString());
                        break;
                }
                return new TokenInvokeResult { Ret = arg.CurrentArg };
            });
        }


        public bool Open(string file) {
            if(File.Exists(file)) {
                var text = File.ReadAllText(file);
                this.MesParser.InvokeToken(text);
                return true;
            }
            return false;
        }
    }
}
