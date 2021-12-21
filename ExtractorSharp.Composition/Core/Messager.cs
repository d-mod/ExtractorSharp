
using System.ComponentModel.Composition;

namespace ExtractorSharp.Composition.Core {


    [Export]
    public class Messager {

        public event MessageEventHandler Sent;

        public event MessageEventHandler OnError;

        public event MessageEventHandler OnSuccess;

        public event MessageEventHandler OnWarning;

        public void Send(string message) {
            this.Send(message, MessageType.None);
        }


        public void Send(string message, MessageType type) {
            var e = new MessageEventArgs() {
                Type = type,
                Message = message
            };
            Sent?.Invoke(e);
            switch(type) {
                case MessageType.Success:
                    OnSuccess?.Invoke(e);
                    break;
                case MessageType.Error:
                    OnError?.Invoke(e);
                    break;
                case MessageType.Warning:
                    OnWarning?.Invoke(e);
                    break;
            }
        }


        public void Success(string message) {
            this.Send(message, MessageType.Success);
        }

        public void Error(string message) {
            this.Send(message, MessageType.Error);
        }

        public void Warning(string message) {
            this.Send(message, MessageType.Error);
        }



        public delegate void MessageEventHandler(MessageEventArgs e);

    }
}
