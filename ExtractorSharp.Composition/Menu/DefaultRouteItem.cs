using System;

namespace ExtractorSharp.Composition.Menu {

    public class DefaultRouteItem : DefaultMenuItem, IRoute {

        public string Command { set; get; }

        public string ShortCutKey { set; get; }

        public event EventHandler CommandExecute;

        public delegate bool CanExecuteHandler();

        public CanExecuteHandler CanExecuteFunc = () => true;

        public DefaultRouteItem() {

        }

        public DefaultRouteItem(IMenuItem item) : base(item) {
            if(item is IRoute route) {
                this.Command = route.Command;
                this.ShortCutKey = route.ShortCutKey;
                this.CommandExecute += route.Execute;
                this.CanExecuteFunc = () => route.CanExecute();
            }
        }

        public DefaultRouteItem(string key, EventHandler click) : base(key) {
            this.CommandExecute += click;
        }


        public void Execute(object sender, EventArgs e) {
            this.CommandExecute?.Invoke(sender, e);
        }

        public bool CanExecute() {
            return this.CanExecuteFunc.Invoke();
        }
    }
}
