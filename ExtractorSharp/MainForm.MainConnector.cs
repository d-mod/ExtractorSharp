using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Exceptions;
using ExtractorSharp.Json;
using ExtractorSharp.Support;

namespace ExtractorSharp {
    partial class MainForm {
        private class MainConnector : IConnector {
            private List<string> _recent = new List<string>();

           

            public MainConnector() {
                SaveChanged += (o, e) => OnSaveChanged();
                FileSupports.Add(new ImgSupport());
                FileSupports.Add(new NpkSupport());
                FileSupports.Add(new AudioSupport());
                FileSupports.Add(new GifSupport());         
                var builder = new LSBuilder();
                var recentConfigPath = $@"{Config["RootPath"]}\conf\recent.json";
                if (File.Exists(recentConfigPath)) {
                    Recent = builder.Read(recentConfigPath).GetValue(typeof(List<string>)) as List<string>;
                }
                RecentChanged += (o, e) => { builder.WriteObject(Recent, recentConfigPath); };
            }

            internal MainForm MainForm { get; set; }

            public SpriteEffect OnSpriteSaving { set; get; }

            public List<IFileSupport> FileSupports { get; } = new List<IFileSupport>();

            public List<string> Recent {
                set {
                    _recent = value;
                    OnRecentChanged(new EventArgs());
                }
                get => _recent;
            }

            public event FileChangeEventHandler RecentChanged;

            public Language Language => Language.Default;

            public IConfig Config => Program.Config;

            public List<Language> LanguageList => Language.List;

            public string SavePath {
                set => MainForm.pathBox.Text = value;
                get => MainForm.pathBox.Text;
            }

            public Sprite[] ImageArray => MainForm.imageList.AllItems;

            public Sprite SelectedImage => MainForm.imageList.SelectedItem;

            public Sprite[] CheckedImages => MainForm.imageList.SelectItems;

            public int[] CheckedImageIndices => MainForm.imageList.SelectIndexes;

            public int ImageCount => MainForm.imageList.Items.Count;

            public int SelectedImageIndex {
                set => MainForm.imageList.SelectedIndex = value;
                get => MainForm.imageList.SelectedIndex;
            }

            public Album[] FileArray => MainForm.albumList.AllItems;

            public Album[] CheckedFiles => MainForm.albumList.SelectItems;

            public Album SelectedFile => MainForm.albumList.SelectedItem;

            public int[] CheckedFileIndices => MainForm.albumList.SelectIndexes;

            public int FileCount => MainForm.albumList.Items.Count;

            public int SelectedFileIndex {
                set => MainForm.albumList.SelectedIndex = value;
                get => MainForm.albumList.SelectedIndex;
            }

            public List<Album> List { get; } = new List<Album>();

            public bool IsSave { set; get; } = true;

            public List<IEffect> Effects { get; } = new List<IEffect>();

            public SpriteEffect Effect {
                get {
                    SpriteEffect result = null;
                    var arr = Effects.ToList();
                    arr.Sort((a, b) => a.Index - b.Index);
                    foreach (var converter in arr) {
                        if (converter.Enable) {
                            result += converter.Handle;
                        }
                    }
                    return result;
                }
            }

            public event FileChangeEventHandler SaveChanged;


            public void OnSaveChanged() {
                ImageListFlush();
                IsSave = false;
                if (Config["AutoSave"].Boolean) {
                    Save();
                }
            }

            public void CanvasFlush() {
                MainForm.CanvasFlush();
            }

            public void ImageListFlush() {
                MainForm.ImageFlush();
            }

            public void FileListFlush() {
                MainForm.ListFlush();
            }

            public void AddFile(bool clear, params string[] args) {
                if (clear) {
                    SavePath = string.Empty;
                    IsSave = true;
                }
                if (SavePath.Length == 0) {
                    SavePath = args.Find(item => item.ToLower().EndsWith(".npk")) ?? string.Empty;

                }
                if (args.Length < 1) {
                    return;
                }
                AddRecent(args);
                var list = LoadFile(args);
                if (list.Count > 0) {
                    Do("addImg", list.ToArray(), clear);
                } else {
                    ImageListFlush();
                }
            }

            public List<Album> LoadFile(params string[] args) {
                var list = new List<Album>();
                for (var i = 0; i < args.Length; i++) {
                    var support = FileSupports.Find(e => args[i].ToLower().EndsWith(e.Extension));
                    var arr = new List<Album>();
                    if (Directory.Exists(args[i])) {
                        arr = LoadFile(Directory.GetFiles(args[i]));
                    } else if (support != null) {
                        arr = support.Decode(args[i]);
                    } else {
                        throw new FileSupportException(Language["FileSupportException"]);
                    }
                    list.AddRange(arr);
                }
                return list;
            }

            public void AddFile(bool clear, params Album[] array) {
                if (clear) {
                    List.Clear();
                }
                if (array.Length > 0) {
                    if (FileCount > 0) {
                        SelectedFileIndex = FileCount - 1;
                    }
                    List.AddRange(array);
                }
            }


            public void RemoveFile(params Album[] array) {
                foreach (var album in array) {
                    if (album != null) {
                        List.Remove(album);
                    }
                }
            }

            public void Save() {
                if (SavePath.Trim().Length == 0) {
                    SelectSavePath();
                }
                if (SavePath.Trim().Length == 0) {
                    return;
                }
                Save(SavePath);
            }

            public void Save(string file) {
                Do("saveImg", List.ToArray(), file, 2);
                AddRecent(file);
                IsSave = true;
            }

            public void SelectPath() {
                var dialog = new FolderBrowserDialog();
                dialog.SelectedPath = Config["GamePath"].Value;
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Config["GamePath"] = new ConfigValue(dialog.SelectedPath);
                    Config.Save();
                }
            }

            public void Do(string name, params object[] args) {
                MainForm.Controller.Do(name, args);
            }

            public void Draw(IPaint paint, Point location, decimal scale) {
                MainForm.Drawer.Brush.Draw(paint, location, scale);
            }

            public void SendMessage(MessageType type, string msg) {
                MainForm.messager.ShowMessage(type, msg);
            }

            public void SendSuccess(string name) {
                MainForm.messager.ShowOperate(name);
            }

            public void SendError(string name) {
                MainForm.messager.ShowError(name);
            }

            public void SendWarning(string name) {
                MainForm.messager.ShowWarnning(name);
            }

            public event FileChangeEventHandler FileOpened;

            private void OnFileOpened(EventArgs e) {
                FileOpened?.Invoke(this, e);
            }

            public void OnRecentChanged(EventArgs e) {
                RecentChanged?.Invoke(this, e);
            }

            private void AddRecent(params string[] args) {
                Recent.InsertRange(0, args);
                Recent = Recent.Distinct().ToList();
            }


            public void SelectSavePath() {
                var dir = SavePath;
                var path = SavePath.GetSuffix();
                if (path != string.Empty) {
                    dir = dir.Replace(path, "");
                }
                var dialog = new SaveFileDialog();
                dialog.InitialDirectory = dir;
                dialog.FileName = path;
                dialog.Filter = "NPK|*.NPK";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    SavePath = dialog.FileName;
                    OnSaveChanged();
                }
            }


            public string GetPath(string path) {
                return $"{Config["RootPath"]}\\{path}";
            }

            public object Dispatch(string name, params object[] args) {
               return MainForm.Controller.Dispatch(name, args);
            }
        }
    }
}