using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Model {

    [Export]
    internal class NewFileModel : BaseViewModel {

        private List<Album> _files;

        [StoreBinding("/data/files")]
        public List<Album> Files {
            set {
                this._files = value?.ToList();
                this.OnPropertyChanged("Files");
            }

            get => this._files;
        }


        private int _insertOffset = 0;

        public int InsertOffset {
            set {

                this._insertOffset = Math.Min(value, this.Files.Count - 1);
                this._insertOffset = Math.Max(-1, this._insertOffset);
                this.OnPropertyChanged("InsertOffset");
            }
            get => this._insertOffset;
        }

        private string _filePath = string.Empty;

        public string FilePath {
            set {
                this._filePath = value;
                this.OnPropertyChanged("FilePath");
            }
            get => this._filePath;
        }

        private int _count = 0;

        public int Count {
            set {
                this._count = value;
                this.OnPropertyChanged("Count");
            }
            get => this._count;
        }

        public List<ImgVersion> Versions => Handler.Versions;

        private ImgVersion _version = ImgVersion.Ver2;


        public ImgVersion FileVersion {
            set {
                this._version = value;
                this.OnPropertyChanged("FileVersion");
            }
            get => this._version;
        }

    }
}
