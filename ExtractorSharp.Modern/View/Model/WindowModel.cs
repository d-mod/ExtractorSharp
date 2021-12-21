using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;
using System.Windows;
using System.Windows.Input;
using ExtractorSharp.Compatibility;

namespace ExtractorSharp {

    [Export]
    internal class WindowModel : BaseViewModel {

        [Import]
        private MenuItemConverter MenuItemConverter;

        public override void OnImportsSatisfied() {
            base.OnImportsSatisfied();
            this.MenuItems = MenuItemConverter.CreateMenu(_menuItems, WindowCommandBindings);

            this.FileListMenu = MenuItemConverter.CreateMenu(_fileListMenu, FileListCommandBindings);

            this.ImageListMenu = MenuItemConverter.CreateMenu(_imageListMenu, ImageListCommandBindings);
        }

        public List<CommandBinding> WindowCommandBindings { set; get; } = new List<CommandBinding>();

        public List<CommandBinding> FileListCommandBindings { set; get; } = new List<CommandBinding>();

        public List<CommandBinding> ImageListCommandBindings { set; get; } = new List<CommandBinding>();



        #region MenuItem
        public object MenuItems { set; get; }

        [ImportMany(typeof(IMenuItem))]
        private List<IMenuItem> _menuItems = new List<IMenuItem>();



        [ImportMany("fileList", typeof(IMenuItem))]
        private List<IMenuItem> _fileListMenu = new List<IMenuItem>();


        public object FileListMenu { set; get; }


        [ImportMany("imageList", typeof(IMenuItem))]
        private List<IMenuItem> _imageListMenu = new List<IMenuItem>();


        public object ImageListMenu { set; get; }

        #endregion

        private List<Album> _files = new List<Album>();

        [StoreBinding(StoreKeys.FILES, ToWay = true)]
        public List<Album> Files {
            set {
                this._files = value?.ToList();
                this.SelectedFile = value?.FirstOrDefault();
                this.OnPropertyChanged("Files");
                this.OnPropertyChanged("DisplayList");
            }

            get => this._files;
        }


        [StoreBinding(StoreKeys.DISPLAY_LIST, ToWay = true)]
        [OnChanged("Keyword")]
        public List<Album> DisplayList {
            get {
                if(this.Keyword != null) {
                    return this.Files.FindAll(e => e.Path.Contains(this.Keyword));
                }
                return this.Files.ToList();
            }
        }


        private string _keyword;

        public string Keyword {
            set {
                this._keyword = value;
                this.OnPropertyChanged("Keyword");
                this.OnPropertyChanged("DisplayList");
            }
            get => this._keyword;
        }


        private Album _selectedFile;


        [StoreBinding(StoreKeys.SELECTED_FILE, ToWay = true)]
        public Album SelectedFile {
            set {
                if(value == null) {
                    value = this.Files?.FirstOrDefault();
                }

                this._selectedFile = value;
                this.OnPropertyChanged("SelectedFile");
                this.OnPropertyChanged("Sprites");
                this.SelectedImage = this.Sprites?.FirstOrDefault();
            }
            get => this._selectedFile;
        }


        private int _selectedFileIndex;


        [StoreBinding(StoreKeys.SELECTED_FILE_INDEX, ToWay = true)]
        public int SelectedFileIndex {
            set {
                this._selectedFileIndex = value;
                this.OnPropertyChanged("SelectedFileIndex");
            }
            get => this._selectedFileIndex;
        }


        private Sprite _selectedImage;


        [StoreBinding(StoreKeys.SELECTED_IMAGE, ToWay = true)]
        public Sprite SelectedImage {
            set {
                this._selectedImage = value;
                this.OnPropertyChanged("SelectedImage");
                this.OnPropertyChanged("ImageSource");
            }
            get => this._selectedImage;
        }

        private int _selectedImageIndex;


        [StoreBinding(StoreKeys.SELECTED_IMAGE_INDEX, ToWay = true)]
        public int SelectedImageIndex {
            set {
                this._selectedImageIndex = value;
                this.OnPropertyChanged("SelectedImageIndex");
                this.OnPropertyChanged("ImageSource");
            }
            get => this._selectedImageIndex;
        }


        public List<Sprite> Sprites => this.SelectedFile?.List.ToList();


        public ImageSource ImageSource => this.SelectedImage?.ImageData?.ToImageSource();

        private string _savePath;

        [StoreBinding(StoreKeys.SAVE_PATH, ToWay = true)]
        public string SavePath {

            set {
                this._savePath = value;

                this.OnPropertyChanged("SavePath");
            }

            get => this._savePath;
        }

        private int _imageX;

        [StoreBinding("/draw/image-x",ToWay =true)]
        public int ImageX {
            set {
                this._imageX = value;
                this.OnPropertyChanged("ImageX");
            }
            get {
                return this._imageX;
            }
        }
        private int _imageY;

        [StoreBinding("/draw/image-y", ToWay = true)]
        public int ImageY {
            set {
                this._imageY = value;
                this.OnPropertyChanged("ImageY");
            }
            get {
                return this._imageY;
            }
        }

        private bool _isAnimation;

        public bool IsAnimation {
            set {
                this._isAnimation = value;
                this.OnPropertyChanged("IsAnimation");
            }

            get => this._isAnimation;
        }

        [StoreBinding("/config/data/animation-speed")]
        public int AnimationSpeed { set; get; }

        private Album _renameFile;

        [StoreBinding("/app/rename-file")]
        public Album RenameFile {
            set {
                this._renameFile = value;
                this.OnPropertyChanged("RenameFile");
            }

            get => this._renameFile;
        }


        public void RefreshSprites() {
            var index = this.SelectedImageIndex;
            this.OnPropertyChanged("SelectedFile", "Sprites");
            this.SelectedImageIndex = index;
        }
    }
}
