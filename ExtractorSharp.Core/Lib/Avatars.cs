using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Lib {



    /// <summary>
    /// 提供时装所需的工具类
    /// </summary>
    public static class Avatars {



        public static readonly string[] Parts = {
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
            return ImportCode(gamePath, new string[0], text, mask, ban);
        }


        public static List<Album> ImportCode(string gamePath, string[] weapons, string text, bool mask, bool ban) {
            return ImportCode(gamePath, Parts, weapons, text, mask, ban);
        }

        public static List<Album> ImportCode(string gamePath, string[] parts, string[] weapons, string text, bool mask, bool ban) {
            var list = new List<Album>();
            var index = text.IndexOf("?");
            if(index == -1) {
                return list;
            }
            var prof = text.Substring(0, index);
            text = text.Substring(index + 1);
            var arr = text.Split('&');
            var regex = new Regex("^-?\\d+$");
            for(var i = 0; i < arr.Length; i++) {
                var pair = arr[i].Split('=');
                if(pair.Length < 2) {
                    return list;
                }
                var part = pair[0];
                var codeStr = pair[1];
                var filename = string.Empty;
                if(weapons.Contains(part)) {
                    filename = GetWeaponFile(prof, part);
                }
                if(parts.Contains(part)) {
                    filename = GetAvatarFile(prof, part);
                }
                if(!regex.IsMatch(codeStr)) {
                    throw new ApplicationException("CodeErrorTips");
                }
                if(filename.Length == 0) {
                    continue;
                }
                var albums = FindByCode($"{gamePath}\\{filename}.NPK", codeStr, mask, ban);
                list.AddRange(albums);
            }
            return list;
        }


        public static List<Album> ImportCode(string gamePath, string text) {
            return ImportCode(gamePath, text, false, false);
        }



        public static string GetAvatarFile(string profession, string part) {
            return $"sprite_character_{ profession }{(profession.EndsWith("_at") ? "" : "_")}equipment_avatar_{ part }";
        }

        public static string GetWeaponFile(string profession, string type) {
            return $"sprite_character_{ profession }{(profession.EndsWith("_at") ? "" : "_")}equipment_weapon_{ type }";
        }


        public static List<Album> FindByCode(string path, int code) {
            return FindByCode(path, CompleteCode(code));
        }

        public static List<Album> FindByCode(string path, string code) {
            return FindByCode(path, code, false, false);
        }

        public static List<Album> FindByCode(string path, int code, bool mask, bool ban) {
            return FindByCode(path, CompleteCode(code), mask, ban);
        }

        public static List<Album> FindByCode(string path, string code, bool mask, bool ban) {
            var regex = new Regex("\\d+");
            var index = int.Parse(code) % 100;
            var match1 = regex.Match(code);
            if(!match1.Success) {
                return new List<Album>();
            }
            var code1 = int.Parse(match1.Value);

            var stream = File.OpenRead(path);
            var list = NpkCoder.ReadInfo(stream);
            list = FindByCode(list, code, mask, ban);
            list = new List<Album>(list.Where(al => {
                stream.Seek(al.Offset, SeekOrigin.Begin);
                NpkCoder.ReadImg(stream, al, stream.Length);
                var code0 = int.Parse(regex.Match(al.Name).Value);
                if(code0 == code1 || ((code0 == code1 / 100 * 100) && (index < al.Palettes.Count))) {
                    al.PaletteIndex = index;
                    al.Name = regex.Replace(al.Name, code, 1);
                    return true;
                }
                return false;
            }));
            stream.Close();
            return list;
        }

        public static List<Album> FindByCode(IEnumerable<Album> array, string code) {
            return FindByCode(array, code, false, false);
        }

        public static List<Album> FindByCode(IEnumerable<Album> array, string code, bool mask, bool ban) {
            var regex = new Regex("\\d+");
            var list = new List<Album>(array.Where(item => {
                var match0 = regex.Match(item.Name);
                var match1 = regex.Match(code);
                if(match0.Success && match1.Success) {
                    var code0 = int.Parse(match0.Value);
                    var code1 = int.Parse(match1.Value);
                    return code0 / 100 == code1 / 100;
                }
                return false;
            }));
            list.RemoveAll(item => {
                if(!mask && item.Name.Contains("mask")) {
                    return true;
                }
                if(!ban && Regex.IsMatch(item.Name, @"^\(tn\)+")) {
                    return true;
                }
                return false;
            });
            return list;
        }

        public static List<Album> FindByCode(IEnumerable<Album> array, int codeNumber) {
            var code = CompleteCode(codeNumber);
            return FindByCode(array, code);
        }

        public static string CompleteCode(int code) {
            var str = code.ToString();
            if(code > -1) {
                while(str.Length < 4) {
                    str = string.Concat(0, str);
                }
            }
            return str;
        }


        public static bool MatchAvatar(Album album) {
            var regex = new Regex(@"^sprite/character/\w+/(at)?equipment/avatar/.*/\w+_\w+\d+\w\d?.img$");
            return regex.IsMatch(album.Path);
        }

        public static bool MatchWeapon(Album album) {
            var regex = new Regex(@"^sprite/character/\w+/(at)?equipment/weapon/.*/(\w+_)?\w+\d+\w\d?.img$");
            return regex.IsMatch(album.Path);
        }

        public static Album[] SplitFile(this Album file) {
            var arr = new Album[Math.Max(1, file.Palettes.Count)];
            var regex = new Regex("\\d+");
            var name = file.Name;
            var match = regex.Match(name);
            if(!match.Success) {
                arr[0] = file;
                return arr;
            }
            var code = int.Parse(match.Value);
            file.Adjust();
            var data = file.Data;
            for(var i = 0; i < arr.Length; i++) {
                arr[i] = new Album {
                    Path = regex.Replace(file.Path, CompleteCode(code + i), 1)
                };
                NpkCoder.ReadImg(data, arr[i], data.LongLength);
                arr[i].Palettes.Clear();
                if(file.Palettes.Count > 0) {
                    arr[i].Palettes.Add(file.Palettes[i]);
                }
            }
            return arr;
        }



        public static Bitmap Preview(IEnumerable<Album> array, int index) {

            var x = 800;
            var y = 600;
            var width = 1;
            var height = 1;
            foreach(var al in array) {
                if(index >= al.List.Count) {
                    continue;
                }
                var source = al.List[index];
                if(source.ColorFormat == ColorFormats.LINK) {
                    source = source.Target;
                }
                if(source.IsHidden) {
                    continue;
                }
                if(source.Width + source.X > width) {
                    width = source.Width + source.X;
                }
                if(source.Height + source.Y > height) {
                    height = source.Height + source.Y;
                }
                if(source.X < x) {
                    x = source.X;
                }
                if(source.Y < y) {
                    y = source.Y;
                }
            }
            width -= x;
            height -= y;
            width = Math.Max(1, width); //防止宽高小于1
            height = Math.Max(1, height);
            var bmp = new Bitmap(width, height);
            var g = Graphics.FromImage(bmp);
            foreach(var img in array) {
                if(index > img.List.Count - 1) {
                    continue;
                }
                var source = img[index];
                if(source.ColorFormat == ColorFormats.LINK) {
                    source = source.Target;
                }
                if(source.CompressMode == CompressMode.NONE && source.Width * source.Height == 1) {
                    continue;
                }
                g.DrawImage(source.Image, source.X - x, source.Y - y);
            }
            g.Dispose();
            return bmp;
        }

    }


}
