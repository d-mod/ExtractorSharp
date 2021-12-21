using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.View.Model {

    [Export]
    internal class NewImageModel : BaseViewModel {

        private Album _targetFile;

        [StoreBinding(StoreKeys.SELECTED_FILE, ToWay = false)]
        public Album TargetFile {
            set {
                this._targetFile = value;
                this.OnPropertyChanged("TargetFile");
                this.OnPropertyChanged("MaxCount");
            }
            get => this._targetFile;
        }

        public int MaxCount => this.TargetFile.List.Count;

        private int _count = 1;

        public int ImageCount {
            set {
                this._count = value;
                this.OnPropertyChanged("ImageCount");
            }
            get => this._count;
        }

        private int _offset = -1;

        public int Offset {
            set {
                this._offset = value;
                this.OnPropertyChanged("Offset");
            }
            get => this._offset;
        }


        private ColorFormats _colorFormat = ColorFormats.LINK;

        public ColorFormats ColorFormat {
            set {
                this._colorFormat = value;
                this.OnPropertyChanged("ColorFormat");
            }
            get => this._colorFormat;
        }

        public CommandContext Context {
            get {
                var targetFile = this.Store.Get<Album>(StoreKeys.SELECTED_FILE);
                return new CommandContext {
                    {"File",targetFile },
                    {"Count",this.ImageCount },
                    {"Type",this.ColorFormat },
                    {"Index",this.Offset }
                };
            }
        }

    }
}
