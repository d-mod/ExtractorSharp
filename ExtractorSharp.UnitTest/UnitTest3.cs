using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest3 {
        public const string API_HOST = "http://localhost:8080/api/dressing";
        public const string GAME_DIR = "D:/地下城与勇士";
        public const string SAVE_DIR = "D:/avatar_new";

        public string[] part_array = {"cap", "coat", "belt", "neck", "hair", "face", "skin", "pants", "shoes"};

        public static Dictionary<string, WeaponInfo> Dictionary;

        static UnitTest3() {
            Handler.Regisity(ImgVersion.Other, typeof(OtherHandler));
            Handler.Regisity(ImgVersion.Ver1, typeof(FirstHandler));
            Handler.Regisity(ImgVersion.Ver2, typeof(SecondHandler));
            Handler.Regisity(ImgVersion.Ver4, typeof(FourthHandler));
            Handler.Regisity(ImgVersion.Ver5, typeof(FifthHandler));
            Handler.Regisity(ImgVersion.Ver6, typeof(SixthHandler));
            Dictionary=new Dictionary<string, WeaponInfo>();
            var builder=new LSBuilder();;
        }

        private List<Profession> GetProfession() {
            var builder = new LSBuilder();
            var obj = builder.Get($"{API_HOST}/profession/list");
            var list = new List<Profession>();
            obj.GetValue(ref list);
            return list;
        }

        private List<string> GetAvatar(string profession, string part) {
            var builder = new LSBuilder();
            var obj = builder.Get($"{API_HOST}/avatar/list/{profession}/{part}");
            var list = new List<Avatar>();
            obj.GetValue(ref list);
            return list.ConvertAll(avatar => avatar.Code);
        }


        [TestMethod]
        public void GetWeapon() {
            var prof_list = GetProfession();
            foreach (var prof in prof_list) {
                var dir = $"{SAVE_DIR}/image/{prof.Name}/weapon";
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);

                foreach (var part in prof.WeaponNames) {
                    var file =
                        $"{GAME_DIR}/ImagePacks2/sprite_character_{prof}{(prof.Name.EndsWith("_at") ? "" : "_")}equipment_weapon_{ part }.NPK";
                    var list = NpkCoder.Load(file);
                    list = list.Where(item => {
                        var name = item.Name;
                        if (name.Contains("(tn)") || name.Contains("_mask")) return false;
                        var regex = new Regex("\\d+");
                        var match = regex.Match(name);
                        return match.Success;
                    }).ToList();
                    NpkCoder.Save($"{dir}/{part}.NPK", list);
                }
            }
        }

        [TestMethod]
        public void Test01() {
            var prof_list = GetProfession();
            foreach (var prof in prof_list) {
                var dir = $"{SAVE_DIR}/image/{prof}";
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);
                foreach (var part in part_array) {
                    var file =
                        $"{GAME_DIR}/ImagePacks2/sprite_character_{prof}{(prof.Name.EndsWith("_at") ? "" : "_")}equipment_avatar_{part}.NPK";
                    var avatars = GetAvatar(prof.Name, part);
                    var list = NpkCoder.Load(file);
                    var arr = new List<Album>();
                    list = list.Where(item => {
                        var name = item.Name;
                        if (name.Contains("(tn)") || name.Contains("_mask")) return false;
                        var regex = new Regex("\\d+");
                        var match = regex.Match(name);
                        if (match.Success) {
                            var codeStr = match.Value;
                            var code = int.Parse(codeStr);
                            var rs = false;
                            for (var i = 0; i < item.Tables.Count; i++) {
                                rs = rs || !avatars.Contains((code + i).ToString());
                            }
                            return rs;
                        }
                        return false;
                    }).ToList();
                    var target = $"{dir}/{part}/";
                    if (!Directory.Exists(target)) Directory.CreateDirectory(target);
                    NpkCoder.Save($"{dir}/{part}.NPK", list);
                }
            }
        }
    }
}