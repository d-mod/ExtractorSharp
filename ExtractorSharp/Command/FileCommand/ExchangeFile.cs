using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.FileCommand {
    class ExchangeFile : ICommand, ICommandMessage {
        public string Name => "ExchangeFile";

        public bool CanUndo => true;

        public bool IsChanged => true;

        public Album Source, Target;

        public void Do(params object[] args) {
            Source = args[0] as Album;
            Target = args[1] as Album;

            var temp = new Album();
            temp.Replace(Target);

            Target.Replace(Source);
            Source.Replace(temp);
        }

        public void Redo() {
            Do(Source, Target);
        }

        public void Undo() {
            Do(Source, Target);
        }
    }
}
