using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    /// <summary>
    ///  
    /// </summary>
    [ExportCommand("ReplaceImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class ReplaceImage : InjectService, IRollback, ISingleMacro {

        [CommandParameter("File", IsDefault = true)]
        private Album album;

        [CommandParameter(IsRequired = false)]
        private bool isAdjust = false;//是否校正坐标

        [CommandParameter]
        private int mode; //替换模式 0为替换贴图 1为替换gif 2为替换文件夹

        private Bitmap[] oldImages;

        [CommandParameter]
        private string path;

        [CommandParameter(IsRequired = false)]
        private ColorFormats type = ColorFormats.UNKNOWN;

        private ColorFormats[] types;

        [CommandParameter]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            if(this.Indices == null || this.Indices.Length == 0) {
                this.Indices = this.album.List.Select(x => x.Index).ToArray();
            }
            this.Redo();
        }

        public void Redo() {
            switch(this.mode) {
                case 0:
                    if(this.album.List.Count > 0) {
                        var image = this.album[this.Indices[0]];
                        this.oldImages = new[] { image.Image };
                        this.types = new[] { image.ColorFormat };
                        image.ReplaceImage(this.type, Image.FromFile(this.path) as Bitmap);
                    }

                    break;
                case 1:
                    var gifentry = Bitmaps.ReadGif(this.path);
                    this.oldImages = new Bitmap[this.Indices.Length];
                    this.types = new ColorFormats[this.Indices.Length];
                    for(var i = 0; i < this.Indices.Length && i < gifentry.Length; i++) {
                        if(this.Indices[i] > this.album.List.Count - 1 && this.Indices[i] < 0) {
                            continue;
                        }
                        var image = this.album[this.Indices[i]];
                        this.oldImages[i] = image.Image;
                        this.types[i] = image.ColorFormat;
                        image.ReplaceImage(this.type, gifentry[i]);
                    }

                    break;
                case 2:
                    var images = this.GetImages(this.album, this.Indices.Length);
                    this.oldImages = new Bitmap[this.Indices.Length];
                    this.types = new ColorFormats[this.Indices.Length];
                    for(var i = 0; i < this.Indices.Length && i < images.Length; i++) {
                        if(this.Indices[i] > this.album.List.Count - 1 || this.Indices[i] < 0) {
                            continue;
                        }
                        var image = this.album[this.Indices[i]];
                        this.oldImages[i] = image.Image;
                        this.types[i] = image.ColorFormat;
                        image.ReplaceImage(this.type, images[i]);
                    }

                    break;
                default:
                    break;
            }

            //   album.Adjust();
            //   album.Refresh();
            this.Messager.Success(this.Language["<ReplaceImage><Success>"]);
        }


        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this.Indices[i] < this.album.List.Count && this.Indices[i] > -1 && this.oldImages[i] != null) {
                    this.album[this.Indices[i]].ReplaceImage(this.types[i], this.oldImages[i]);
                }
            }
        }

        public void Action(Album album, int[] indexes) {
            var path = this.path;
            switch(this.mode) {
                case 0:
                    if(album.List.Count > 0) {
                        album[indexes[0]].ReplaceImage(this.type, Image.FromFile(path) as Bitmap);
                    }
                    break;
                case 1:
                    var gifentry = Bitmaps.ReadGif(path);
                    for(var i = 0; i < indexes.Length && i < gifentry.Length; i++) {
                        if(indexes[i] > album.List.Count - 1 && indexes[i] < 0) {
                            continue;
                        }
                        album[indexes[i]].ReplaceImage(this.type, gifentry[i]);
                    }

                    break;
                case 2:
                    var images = this.GetImages(album, indexes.Length);
                    for(var i = 0; i < indexes.Length && i < images.Length; i++) {
                        if(indexes[i] > album.List.Count - 1 || indexes[i] < 0) {
                            continue;
                        }
                        album[indexes[i]].ReplaceImage(this.type, images[i]);
                    }

                    break;
                default:
                    break;
            }
        }


        private static string TryBySuffix(string path, string[] suffixs) {
            foreach(var suffix in suffixs) {
                var p = string.Concat(path, suffix);
                if(File.Exists(p)) {
                    return p;
                }
            }
            return null;
        }

        private Bitmap[] GetImages(Album Album, int count) {
            var dir = this.path;
            dir = dir.Replace(@"\", "/");
            dir = dir.Complete("/" + Album.Path); //补全img路径
            if(!Directory.Exists(dir)) {
                dir = this.path + "/" + Album.Name;
            }
            if(!Directory.Exists(dir)) {
                dir = this.path;
            }
            var bmps = new Bitmap[count]; //空贴图数组
            if(!Directory.Exists(dir)) {
                if(Directory.Exists(dir + "_")) {
                    dir += "_";
                } else {
                    return bmps;
                }
            }

            for(var i = 0; i < count; i++) {
                var path = dir;
                path += "/" + i;
                path = TryBySuffix(path, new string[] { ".png", ".bmp", ".jpg" });
                if(!string.IsNullOrEmpty(path)) {
                    bmps[i] = Image.FromFile(path) as Bitmap;
                }
            }

            return bmps;
        }
    }
}
