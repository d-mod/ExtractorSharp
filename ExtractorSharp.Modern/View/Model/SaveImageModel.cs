using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Model {

    [Export]
    internal class SaveImageModel : BaseViewModel {

        private string _target;

        [CommandParameter]
        public string Target {
            set {
                this._target = value;
                this.OnPropertyChanged("Target");
            }
            get => this._target;
        }

        [CommandParameter]
        public Album File { set; get; }

        [CommandParameter]
        public int[] Indices { set; get; }
    }
}
