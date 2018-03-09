using ExtractorSharp.Composition;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Draw;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp {
    partial class MainForm {
        private class MainConnector : IConnector {
            internal MainForm MainForm { get; set; }

            internal MainConnector() {
                SaveChanged += (o, e) => OnSaveChanged();
            }

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
                set {
                    MainForm.imageList.SelectedIndex = value;
                }
                get {
                    return MainForm.imageList.SelectedIndex;
                }
            }

            public Album[] FileArray => MainForm.albumList.AllItems;

            public Album[] CheckedFiles => MainForm.albumList.SelectItems;

            public Album SelectedFile => MainForm.albumList.SelectedItem;

            public int[] CheckedFileIndices => MainForm.albumList.SelectIndexes;

            public int FileCount => MainForm.albumList.Items.Count;

            public int SelectedFileIndex {
                set {
                    MainForm.albumList.SelectedIndex = value;
                }
                get {
                    return MainForm.albumList.SelectedIndex;
                }
            }

            public List<Album> List => _list;

            public bool IsSave { set; get; } = true;

            public SpriteConverter OnSpriteSaving { set; get; }

            public List<ISpriteConverter> SpriteConverters { get; } = new List<ISpriteConverter>();

            public SpriteConverter SpirteConverter {
                get {
                    SpriteConverter result = null;
                    var arr = SpriteConverters.ToList();
                    arr.Sort((a, b) => a.Index - b.Index);
                    foreach (var converter in arr) {
                        if (converter.Enable) {
                            result += converter.Convert;
                        }
                    }
                    return result;
                }
            }

            private readonly List<Album> _list = new List<Album>();

            public event EventHandler SaveChanged;



            public void OnSaveChanged() {
                ImageListFlush();
                IsSave = false;
                if (Config["AutoSave"].Boolean) {
                    Save();
                }
            }

            public void CanvasFlush() => MainForm.CanvasFlush();

            public void ImageListFlush() => MainForm.ImageFlush();

            public void FileListFlush() => MainForm.ListFlush();

            public void AddFile(bool clear, params string[] args) {
                if (clear) {
                    SavePath = string.Empty;
                    IsSave = true;
                }
                if (SavePath.Length == 0) {
                    SavePath = args.Find(item => item.ToLower().EndsWith(".npk")) ?? string.Empty;
                }
                if (args.Length > 0) {
                    MainForm.Controller.Do("addImg", Npks.Load(args).ToArray(), clear);
                }
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
                    List.Remove(album);
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
                Npks.Save(file, List);
                IsSave = true;
                Messager.ShowOperate("SaveFile");
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
                dialog.Filter = "npk文件|*.npk";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    SavePath = dialog.FileName;
                    OnSaveChanged();
                }
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
        }
    }
}
