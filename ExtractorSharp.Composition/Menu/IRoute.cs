using System;

namespace ExtractorSharp.Composition.Menu {
    public interface IRoute {

        string Command { set; get; }

        string ShortCutKey { set; get; }

        bool CanExecute();

        void Execute(object sender, EventArgs e);

    }
}
