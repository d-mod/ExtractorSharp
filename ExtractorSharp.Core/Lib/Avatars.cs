using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Core.Lib {

    /// <summary>
    /// 提供时装所需的工具类
    /// </summary>
    public static class Avatars {


        public delegate void MergeHandler(object sender, MergeEventArgs e);


        public readonly static string[] Parts = {
                "cap","hair","face",
                "neck","coat","skin",
                "belt","pants","shoes"
        };


        /// <summary>
        /// 将代码导入为文件列表
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<Album> ImportCode(string gamePath, string text, bool mask, bool ban) {
            var list = new List<Album>();
            var index = text.IndexOf("?");
            if (index == -1) {
                return list;
            }
            var prof = text.Substring(0, index);
            text = text.Substring(index + 1);
            var arr = text.Split("&");
            var regex = new Regex("^-?\\d+$");
            for (var i = 0; i < arr.Length; i++) {
                var pair = arr[i].Split("=");
                if (pair.Length < 2) {
                    return list;
                }
                var part = pair[0];
                var codeStr = pair[1];
                if (!Parts.Contains(part)) {
                    continue;
                }
                if (!regex.IsMatch(codeStr)) {
                    throw new ApplicationException("CodeErrorTips");
                }
                var filename = GetAvatarFile(prof, part);
                var albums = NpkCoder.FindByCode($"{gamePath}\\{filename}.NPK", codeStr, mask, ban);
                list.AddRange(albums);
            }
            return list;
        }


        public static List<Album> ImportCode(string gamePath,string text) {
            return ImportCode(gamePath, text, false, false);
        }



        public static string GetAvatarFile(string profession, string part) {
            return $"sprite_character_{ profession }{(profession.EndsWith("_at") ? "" : "_")}equipment_avatar_{ part }";
        }

        public static void Merge(IEnumerable<Album> list, int targetVersion, IMergeProgress progress) {
            var e = new MergeEventArgs();
            list = list.Reverse(); //序列反转
            var count = 0;
            var version = targetVersion > 0 ? (ImgVersion)targetVersion : ImgVersion.Ver2;
            foreach (var al in list) {
                if (al.List.Count > count) {
                    count = al.List.Count;
                }
                if (targetVersion == 0 && al.Version > version) {
                    version = al.Version;
                }
            }
            var file = new Album {
                Version = version
            };
            file.InitHandle(null);
            e.Count = count;
            e.Album = file;
            progress.OnMergeStarted(e); //启动拼合事件
            var entitys = new List<Sprite>();
            for (var i = 0; i < count; i++) {
                var entity = new Sprite(file);
                var width = 1;
                var height = 1;
                var max_width = 0;
                var max_height = 0;
                var x = 800;
                var y = 600;
                var type = ColorBits.ARGB_1555;
                foreach (var al in list) {
                    if (i < al.List.Count) {
                        var source = al.List[i];
                        if (source.Type == ColorBits.LINK) {
                            source = source.Target;
                        }
                        if (source.CanvasWidth > max_width) {
                            max_width = source.CanvasHeight;
                        }
                        if (source.CanvasHeight > max_height) {
                            max_height = source.CanvasHeight;
                        }
                        if (source.Hidden) {
                            continue;
                        }
                        if (source.Width + source.X > width) {
                            width = source.Width + source.X;
                        }
                        if (source.Height + source.Y > height) {
                            height = source.Height + source.Y;
                        }
                        if (source.X < x) {
                            x = source.X;
                        }
                        if (source.Y < y) {
                            y = source.Y;
                        }
                        if (source.Type > type && source.Type < ColorBits.LINK) {
                            type = source.Type;
                        }
                    }
                }

                width -= x; //获得上下左右两端的差,即宽高
                height -= y;
                width = width > 1 ? width : 1; //防止宽高小于1
                height = height > 1 ? height : 1;
                if (width * height > 1) {
                    entity.CompressMode = CompressMode.ZLIB;
                }
                entity.Type = type;
                entity.Index = entitys.Count;
                entity.Location = new Point(x, y);
                entity.CanvasSize = new Size(max_width, max_height);
                var image = new Bitmap(width, height);
                using (var g = Graphics.FromImage(image)) {
                    foreach (var al in list) {
                        if (i < al.List.Count) {
                            var source = al.List[i];
                            if (source.Type == ColorBits.LINK) {
                                source = source.Target;
                            }
                            g.DrawImage(source.Picture, source.X - x, source.Y - y); //绘制贴图                       

                        }
                    }
                }
                entity.ReplaceImage(type, false, image); //替换贴图
                e.Progress++; //拼合进度自增
                progress.OnMergeProcessing(e);
                entitys.Add(entity);
            }
            file.List.AddRange(entitys);
            progress.OnMergeCompleted(e); //拼合完成
        }







        public static Bitmap Preview(Album[] array, int index) {

            var x = 800;
            var y = 600;
            var width = 1;
            var height = 1;
            foreach (var al in array) {
                if (index >= al.List.Count) {
                    continue;
                }
                var source = al.List[index];
                if (source.Type == ColorBits.LINK) {
                    source = source.Target;
                }
                if (source.Hidden) {
                    continue;
                }
                if (source.Width + source.X > width) {
                    width = source.Width + source.X;
                }
                if (source.Height + source.Y > height) {
                    height = source.Height + source.Y;
                }
                if (source.X < x) {
                    x = source.X;
                }
                if (source.Y < y) {
                    y = source.Y;
                }
            }
            width -= x;
            height -= y;
            width = Math.Max(1,width); //防止宽高小于1
            height = Math.Max(1, height);
            var bmp = new Bitmap(width, height);
            var g = Graphics.FromImage(bmp);
            foreach (var img in array) {
                if (index > img.List.Count - 1) {
                    continue;
                }
                var source = img[index];
                if (source.Type == ColorBits.LINK) {
                    source = source.Target;
                }
                if (source.CompressMode == CompressMode.NONE && source.Width * source.Height == 1) {
                    continue;
                }
                g.DrawImage(source.Picture, source.X - x, source.Y - y);
            }
            g.Dispose();
            return bmp;
        }

    }


}
