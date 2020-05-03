using System.Drawing;
using System.IO;
using System.Linq;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    internal class ReplaceImage : ISingleAction, ICommandMessage {
        private Album Album { set; get; }

        private bool IsAdjust { set; get; } //是否校正坐标

        private int Mode { set; get; } //替换模式 0为替换贴图 1为替换gif 2为替换文件夹

        private Bitmap[] OldImages { set; get; }

        private string Path { set; get; }

        private ColorBits Type { set; get; }

        private ColorBits[] Types { set; get; }

        public int[] Indices { set; get; }

        public void Do(params object[] args) {
            Type = (ColorBits) args[0];
            IsAdjust = (bool) args[1];
            Mode = (int) args[2];
            Path = args[3] as string;
            Album = args[4] as Album;
            Indices = args[5] as int[];
            if (Indices == null || Indices.Length == 0)
            {
                Indices = Album.List.Select(x => x.Index).ToArray();
            }
            switch (Mode) {
                case 0:
                    if (Album.List.Count > 0) {
                        var image = Album[Indices[0]];
                        OldImages = new[] {image.Picture};
                        Types = new[] {image.Type};
                        image.ReplaceImage(Type, IsAdjust, Image.FromFile(Path) as Bitmap);
                    }

                    break;
                case 1:
                    var gifentry = Bitmaps.ReadGif(Path);
                    OldImages = new Bitmap[Indices.Length];
                    Types = new ColorBits[Indices.Length];
                    for (var i = 0; i < Indices.Length && i < gifentry.Length; i++) {
                        if (Indices[i] > Album.List.Count - 1 && Indices[i] < 0) {
                            continue;
                        }
                        var image = Album[Indices[i]];
                        OldImages[i] = image.Picture;
                        Types[i] = image.Type;
                        image.ReplaceImage(Type, IsAdjust, gifentry[i]);
                    }

                    break;
                case 2:
                    var images = GetImages(Album, Indices.Length);
                    OldImages = new Bitmap[Indices.Length];
                    Types = new ColorBits[Indices.Length];
                    for (var i = 0; i < Indices.Length && i < images.Length; i++) {
                        if (Indices[i] > Album.List.Count - 1 || Indices[i] < 0) {
                            continue;
                        }
                        var image = Album[Indices[i]];
                        OldImages[i] = image.Picture;
                        Types[i] = image.Type;
                        image.ReplaceImage(Type, IsAdjust, images[i]);
                    }

                    break;
                default:
                    break;
            }

            Album.Adjust();
            Album.Refresh();
        }

        public void Redo() {
            Do(Type, IsAdjust, Mode, Path, Album, Indices);
        }


        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] < Album.List.Count && Indices[i] > -1 && OldImages[i] != null) {
                    Album[Indices[i]].ReplaceImage(Types[i], IsAdjust, OldImages[i]);
                }
            }
        }

        public void Action(Album album, int[] indexes) {
            var path = Path;
            switch (Mode) {
                case 0:
                    if (album.List.Count > 0) {
                        album[indexes[0]].ReplaceImage(Type, IsAdjust, Image.FromFile(path) as Bitmap);
                    }
                    break;
                case 1:
                    var gifentry = Bitmaps.ReadGif(path);
                    for (var i = 0; i < indexes.Length && i < gifentry.Length; i++) {
                        if (indexes[i] > album.List.Count - 1 && indexes[i] < 0) {
                            continue;
                        }
                        album[indexes[i]].ReplaceImage(Type, IsAdjust, gifentry[i]);
                    }

                    break;
                case 2:
                    var images = GetImages(album, indexes.Length);
                    for (var i = 0; i < indexes.Length && i < images.Length; i++) {
                        if (indexes[i] > album.List.Count - 1 || indexes[i] < 0) {
                            continue;
                        }
                        album[indexes[i]].ReplaceImage(Type, IsAdjust, images[i]);
                    }

                    break;
                default:
                    break;
            }
        }

        public bool CanUndo => true;

        public bool IsChanged => true;

        public string Name => "ReplaceImage";

        public Bitmap[] GetImages(Album Album, int count) {
            var dir = Path;
            dir = dir.Replace(@"\", "/");
            dir = dir.Complete("/" + Album.Path); //补全img路径
            if (!Directory.Exists(dir)) {
                dir = Path + "/" + Album.Name;
            }
            if (!Directory.Exists(dir)) {
                dir = Path;
            }
            var bmps = new Bitmap[count]; //空贴图数组
            if (!Directory.Exists(dir)) {
                if (Directory.Exists(dir + "_")) {
                    dir += "_";
                } else {
                    return bmps;
                }
            }

            for (var i = 0; i < count; i++) {
                var path = dir;
                path += "/" + i;
                var png = path + ".png";
                var bmp = path + ".bmp";
                var jpg = path + ".jpg";
                var exist = false;
                if (File.Exists(png)) {
                    exist = true;
                    path = png;
                } else if (File.Exists(jpg)) {
                    exist = true;
                    path = jpg;
                } else if (File.Exists(bmp)) {
                    exist = true;
                    path = bmp;
                }
                if (exist) {
                    bmps[i] = Image.FromFile(path) as Bitmap;
                }
            }

            return bmps;
        }
    }
}