using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.View.Model {

    [Export]
    internal class FilePropertiesModel : BaseViewModel, IProgress<ProgressEventArgs> {

        private Album _targetFile;

        [StoreBinding(StoreKeys.SELECTED_FILE)]
        public Album TargetFile {
            set {
                this._targetFile = value;
                this.OnPropertyChanged("TargetFile", "FileName", "NeedConvert");
                this.RefreshFile();
            }
            get => this._targetFile;
        }

        private IEnumerable<Album> _files;

        [StoreBinding(StoreKeys.SELECTED_FILE_RANGE)]
        public IEnumerable<Album> Files {
            set {
                this._files = value;
                this.OnPropertyChanged("Files");
            }
            get => this._files;
        }


        public void RefreshFile() {
            this.FilePath = this.TargetFile?.Path;
            this.FileVersion = this.TargetFile?.Version ?? ImgVersion.Ver2;
        }


        public string FileName => this.TargetFile?.Name;

        public int ImageCount => this.TargetFile?.List.Count ?? 0;

        private string _filePath;

        public string FilePath {
            set {
                this._filePath = value;
                this.OnPropertyChanged("FilePath");
            }
            get => this._filePath;
        }

        public List<ImgVersion> Versions => Handler.Versions;


        private ImgVersion _fileVersion;

        public ImgVersion FileVersion {
            set {
                this._fileVersion = value;
                this.OnPropertyChanged("FileVersion", "NeedConvert", "ShowConvertAll");
            }
            get => this._fileVersion;
        }

        public bool NeedConvert => this.TargetFile?.Version != this.FileVersion;


        public bool ShowConvertAll => this.NeedConvert && this.Files?.Count() > 1;

        private bool _convertAll = false;

        public bool ConvertAll {
            set {
                this._convertAll = value;
                this.OnPropertyChanged("ConvertAll");
            }
            get => this._convertAll;
        }

        private int _convertProgress;

        public int ConvertProgress {
            set {
                this._convertProgress = value;
                this.OnPropertyChanged("ConvertProgress");
            }
            get => this._convertProgress;
        }

        private int _convertMaximum;

        public int ConvertMaximum {
            set {
                this._convertMaximum = value;
                this.OnPropertyChanged("ConvertMaximum");
            }
            get => this._convertMaximum;
        }

        private bool _showConvert = false;


        public bool ShowConvert {
            set {
                if(value != this._showConvert) {
                    this._showConvert = value;
                    this.OnPropertyChanged("ShowConvert");
                }
            }
            get => this._showConvert;
        }

        public void Report(ProgressEventArgs e) {
            this.ConvertProgress = e.Value;
            this.ConvertMaximum = e.Maximum;
            this.ShowConvert = !e.IsCompleted;
        }

        public BackgroundWorker ConvertWorker;

        public override void OnImportsSatisfied() {
            base.OnImportsSatisfied();
            this.ConvertWorker = new BackgroundWorker();
            this.ConvertWorker.DoWork += this.DoWork;
            this.ConvertWorker.RunWorkerCompleted += this.Completed;
        }

        private void Completed(object sender, RunWorkerCompletedEventArgs e) {
            this.ConvertAll = false;
            this.OnPropertyChanged("TargetFile", "NeedConvert");
        }

        private void DoWork(object sender, EventArgs e) {
            if(this.NeedConvert) {
                this.Controller.Do("ConvertFile", new CommandContext {
                    { "Progress", this },
                    { "Files", this.ConvertAll ? this.Files: new[] { this.TargetFile } },
                    { "Version", this.FileVersion }
                });

            }
        }


    }
}
