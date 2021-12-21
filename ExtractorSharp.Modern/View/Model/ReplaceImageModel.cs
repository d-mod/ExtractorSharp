using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.View.Model {
    [Export]
    internal class ReplaceImageModel : BaseViewModel {

        private ColorFormats _colorFormat = ColorFormats.UNKNOWN;

        [CommandParameter("Type")]
        public ColorFormats ColorFormat {
            set {
                this._colorFormat = value;
                this.OnPropertyChanged("ColorFormat");
            }
            get => this._colorFormat;
        }

        private IEnumerable<Sprite> _targetImages;

        [StoreBinding(StoreKeys.SELECTED_IMAGE_RANGE, ToWay = false)]
        public IEnumerable<Sprite> TargetImages {
            set {
                this._targetImages = value;
                this.OnPropertyChanged("TargetImages");
            }
            get => this._targetImages;
        }

        [StoreBinding(StoreKeys.SELECTED_FILE, ToWay = false)]
        public Album TargetFile { set; get; }

        private bool _isFromGif = false;

        public bool IsFromGif {
            set {
                this._isFromGif = value;
                this.OnPropertyChanged("IsFromGif");
            }
            get => this._isFromGif;
        }

        public int[] Indices => this.TargetImages.Select(_ => _.Index).ToArray();

    }
}
