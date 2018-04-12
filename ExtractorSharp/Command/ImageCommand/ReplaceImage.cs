using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Command.ImageCommand {
    class ReplaceImage : ISingleAction,ICommandMessage{
        
        public int[] Indices { set; get; }

        private Album Album;

        private string Path;

        private Bitmap[] oldImages;

        private ColorBits type;

        private ColorBits[] types;

        private bool isAdjust;//是否校正坐标

        private int Mode;//替换模式 0为替换贴图 1为替换gif 2为替换文件夹

        public void Do(params object[] args) {
            type = (ColorBits)args[0];
            isAdjust = (bool)args[1];
            Mode = (int)args[2];
            Path = args[3] as string;
            Album = args[4] as Album;
            Indices = args[5] as int[];
            switch (Mode) {
                case 0:
                    if (Album.List.Count > 0) {
                        var image = Album[Indices[0]];
                        oldImages = new Bitmap[] { image.Picture };
                        types = new ColorBits[] { image.Type };
                        image.ReplaceImage(type, isAdjust, Image.FromFile(Path) as Bitmap);
                    }
                    break;
                case 1:
                    var gifentry = Bitmaps.ReadGif(Path);
                    oldImages = new Bitmap[Indices.Length];
                    types = new ColorBits[Indices.Length];
                    for (int i = 0; i < Indices.Length && i < gifentry.Length; i++) {
                        if (Indices[i] > Album.List.Count - 1 && Indices[i] < 0) {
                            continue;
                        }
                        var image = Album[Indices[i]];
                        oldImages[i] = image.Picture;
                        types[i] = image.Type;
                        image.ReplaceImage(type, isAdjust, gifentry[i]);
                    }
                    break;
                case 2:
                    var images = GetImages(Album,Indices.Length);
                    oldImages = new Bitmap[Indices.Length];
                    types = new ColorBits[Indices.Length];
                    for (var i = 0; i < Indices.Length && i < images.Length; i++) {
                        if (Indices[i] > Album.List.Count - 1 || Indices[i] < 0)
                            continue;
                        var image = Album[Indices[i]];
                        oldImages[i] = image.Picture;
                        types[i] = image.Type;
                        image.ReplaceImage(type, isAdjust, images[i]);
                    }
                    break;
                default:
                    break;
            }
            Album.Adjust();
            Album.Refresh();
        }

        public Bitmap[] GetImages(Album Album,int count) {
            var dir = Path;
            dir=dir.Replace(@"\","/");
            dir = dir.Complete("/" + Album.Path);//补全img路径
            if (!Directory.Exists(dir)) {//当不存在和img路径匹配的文件夹时。直接使用路径
                dir = Path + "/" + Album.Name;
            }
            if (!Directory.Exists(dir)) {
                dir = Path;
            }
            var bmps = new Bitmap[count];//空贴图数组
            if (!Directory.Exists(dir)) {
                if (Directory.Exists(dir + "_")) {//允许路径的下划线后缀
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

        public void Redo() => Do(type, isAdjust, Mode, Path, Album, Indices);
        

        public void Undo() {
            for (int i = 0; i < Indices.Length; i++) {
                if (Indices[i] < Album.List.Count && Indices[i] > -1 && oldImages[i] != null) {
                    Album[Indices[i]].ReplaceImage(types[i], isAdjust, oldImages[i]);
                }
            }
        }

        public void Action(Album Album,int[] indexes) {
            var path = Path;
            switch (Mode) {
                case 0:
                    if (Album.List.Count > 0) 
                         Album[indexes[0]].ReplaceImage(type, isAdjust, Image.FromFile(path) as Bitmap);               
                    break;
                case 1:
                    var gifentry = Bitmaps.ReadGif(path);
                    for (var i = 0; i < indexes.Length && i < gifentry.Length; i++) {
                        if (indexes[i] > Album.List.Count - 1 && indexes[i] < 0)
                            continue;
                        Album[indexes[i]].ReplaceImage(type, isAdjust, gifentry[i]);
                    }
                    break;
                case 2:
                    var images = GetImages(Album,indexes.Length);
                    for (var i = 0; i < indexes.Length && i < images.Length; i++) {
                        if (indexes[i] > Album.List.Count - 1 || indexes[i] < 0)
                            continue;
                        Album[indexes[i]].ReplaceImage(type, isAdjust, images[i]);
                    }
                    break;
                default:
                    break;
            }
        }

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public string Name => "ReplaceImage";
      
    }
}
