using System;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core.Draw;

namespace ExtractorSharp.Composition.Draw {
    public interface IPaintMenuItem {

        string Name { get; }

        void OnClick(object sender, PaintMenuItemEventArgs e);

    }

    public class PaintMenuItemEventArgs : EventArgs {

        public PaintMenuItemEventArgs(Store store, IPaint paint) {
            this.Store = store;
            this.Paint = paint;
        }

        public Store Store { get; }


        public IPaint Paint { get; }

    }


}
