using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.DressingTools.Model;
using ExtractorSharp.DressingTools.Properties;
using ExtractorSharp.Json;
using ExtractorSharp.UnitTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtractorSharp.DressingTools {
    class Program {
        private static Dictionary<string, int> Sorts;
        public const string API_HOST = "https://kritsu.net/api/dressing";

        static void Main(string[] args) {
            Sorts = new Dictionary<string, int>();
            var builder = new LSBuilder();
            var obj = builder.ReadJson(Resources.queues);
            obj["Rules"].GetValue(ref Sorts);
            var date = DateTime.Now;
            Console.WriteLine("Start...");
            GetAvatars();
            var time = DateTime.Now - date;
            Console.WriteLine($"Completed...{time.TotalSeconds}s");
            Console.ReadKey();
        }

        public static void GetAvatars() {
            var professions = new List<Profession>();
            var builder = new LSBuilder();
            builder.ReadJson(Resources.profession).GetValue(ref professions);
            var gamePath = "D:\\地下城与勇士";
            var parts = new string[] { "hair", "cap", "face", "neck", "coat", "skin", "belt", "pants", "shoes" };

            var regex = new Regex("\\d+");
            foreach (var prof in professions) {
                foreach (var part in parts) {
                    var file = $"{gamePath}\\{NpkCoder.IMAGE_DIR}\\{Avatars.GetAvatarFile(prof.Name, part)}.NPK";
                    if (!File.Exists(file)) {
                        continue;
                    }
                    var array = GetAvatar(prof.Name, part);
                    var list = NpkCoder.LoadAll(file, e => MatchAvatar(e, array));
                    var path = $"d:\\avatar\\new_image\\{prof.Name}\\{part}";
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                    foreach (var img in list) {
                        var image = img[prof.Frame];
                        var match = regex.Match(img.Name);
                        ImageToJson(path, prof.Name, part, img.Name, match.Value, image);
                    }
                }
            }
        }

        private static bool MatchAvatar(Album al, List<string> list) {
            if (Avatars.MatchAvatar(al)) {
                var match = Regex.Match(al.Name, "\\d+");
                if (match.Success) {
                    return !list.Contains(match.Value);
                }
            }
            return false;
        }


        private static List<string> GetAvatar(string profession, string part) {
            var url = $"{API_HOST}/avatar/list/{profession}/{part}.json";
            var builder = new LSBuilder();
            var obj = builder.Get(url);
            var list = new List<Avatar>();
            obj.GetValue(ref list);
            Console.WriteLine("GET ... " + url);
            return list.ConvertAll(avatar => avatar.Code);
        }

        public static void GetWeapons() {
            var professions = new List<Profession>();
            var builder = new LSBuilder();
            builder.ReadJson(Resources.profession).GetValue(ref professions);
            var gamePath = "D:\\地下城与勇士";

            var regex = new Regex("\\d+");
            foreach (var prof in professions) {
                foreach (var type in prof.Weapon) {
                    var file = $"{gamePath}\\{NpkCoder.IMAGE_DIR}\\{Avatars.GetWeaponFile(prof.Name, type)}.NPK";
                    if (!File.Exists(file)) {
                        continue;
                    }
                    var list = NpkCoder.LoadAll(file, Avatars.MatchWeapon);
                    var path = $"d:\\avatar\\new_image\\{prof.Name}\\{type}";
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                    foreach (var img in list) {
                        var image = img[prof.Frame];
                        var match = regex.Match(img.Name);
                        ImageToJson(path, prof.Name, type, img.Name, match.Value, image);
                    }
                }
            }
        }

        public static void ImageToJson(string path, string profession, string part, string name, string code, Sprite image) {
            var regex = new Regex("\\d+");
            name = name.Replace(".img", "");
            name = regex.Replace(name, code, 1);

            var pattern = name.Substring(name.IndexOf("_") + 1);
            pattern = regex.Replace(pattern, "");

            var i = regex.Match(name).Index;
            name = name.Substring(i);

            var dressImage = new DressImage();
            dressImage.hash = $"{profession}&{part}&{code}";
            dressImage.name = $"{name}.png";
            dressImage.width = image.Width;
            dressImage.height = image.Height;
            dressImage.x = image.X;
            dressImage.y = image.Y;
            dressImage.z = IndexOf(pattern);
            var builder = new LSBuilder();
            image.Picture.Save($"{path}\\{name}.png");
            builder.WriteObject(dressImage, $"{path}\\{name}.json");
            Console.WriteLine($"Extract {profession} >> {part} >> {code}");
        }


        public static int IndexOf(string name) {
            if (Sorts.ContainsKey(name)) return Sorts[name];
            return -1;
        }

    }
}
