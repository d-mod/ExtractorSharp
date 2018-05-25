using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using ExtractorSharp.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest3 {

        static UnitTest3() {
            Handler.Regisity(Img_Version.Other, typeof(OtherHandler));
            Handler.Regisity(Img_Version.Ver1, typeof(FirstHandler));
            Handler.Regisity(Img_Version.Ver2, typeof(SecondHandler));
            Handler.Regisity(Img_Version.Ver4, typeof(FourthHandler));
            Handler.Regisity(Img_Version.Ver5, typeof(FifthHandler));
            Handler.Regisity(Img_Version.Ver6, typeof(SixthHandler));

        }

        public string[] part_array = {"cap","coat","belt","neck","hair","face","skin","pants","shoes" };
        public const string API_HOST = "http://localhost:8080/dressing";
        public const string GAME_DIR = "D:/地下城与勇士";
        public const string SAVE_DIR = "D:/avatar_new";

        private List<string> GetProfession() {
            LSBuilder builder = new LSBuilder();
            var obj = builder.Get($"{API_HOST}/profession/list");
            List<Profession> list = new List<Profession>();
            obj.GetValue(ref list);
            return list.ConvertAll(profesion => profesion.Name);
        }

        private List<string> GetAvatar(string profession, string part) {
            LSBuilder builder = new LSBuilder();
            var obj = builder.Get($"{API_HOST}/avatar/list/{profession}/{part}");
            List<Avatar> list = new List<Avatar>();
            obj.GetValue(ref list);
            return list.ConvertAll(avatar => avatar.Code);
        }

        [TestMethod]
        public void Test01() {
            var prof_list = GetProfession();
            foreach (var prof in prof_list) {
                var dir = $"{SAVE_DIR}/image/{prof}";
                if (Directory.Exists(dir)) {
                    Directory.Delete(dir, true);
                }
                Directory.CreateDirectory(dir);
                foreach (var part in part_array) {
                    var file = $"{GAME_DIR}/ImagePacks2/sprite_character_{prof}{(prof.EndsWith("_at") ? "" : "_")}equipment_avatar_{part}.NPK";
                    var avatars = GetAvatar(prof, part);
                    var list = Npks.Load(file);
                    var arr = new List<Album>();
                    list = list.Where(item => {
                        var name = item.Name;
                        if (name.Contains("(tn)") || name.Contains("_mask")) {
                            return false;
                        }
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
                    if (!Directory.Exists(target)) {
                        Directory.CreateDirectory(target);
                    }
                    Npks.SaveToDirectory($"{dir}/{part}/", list);
                }
            }

        }






    }
}
