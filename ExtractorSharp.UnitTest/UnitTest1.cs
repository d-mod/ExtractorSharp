using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using ExtractorSharp.Json;
using ExtractorSharp.UnitTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest1 {
        public string[] part_array = { "cap", "coat", "belt", "neck", "hair", "face", "skin", "pants", "shoes" };
        public static Dictionary<string, int> pairs = new Dictionary<string, int>();
        public static string GAME_PATH = "d:/地下城与勇士/ImagePacks2/";
        public static Dictionary<string, int> sorts = new Dictionary<string, int>();
        internal static Dictionary<string, WeaponInfo> weapons = new Dictionary<string, WeaponInfo>();
        static UnitTest1() {
            Handler.Regisity(Img_Version.Ver1, typeof(FirstHandler));
            Handler.Regisity(Img_Version.Ver2, typeof(SecondHandler));
            Handler.Regisity(Img_Version.Ver4, typeof(FourthHandler));
            Handler.Regisity(Img_Version.Ver5, typeof(FifthHandler));
            Handler.Regisity(Img_Version.Ver6, typeof(SixthHandler));
            pairs = new Dictionary<string, int>() {
                {"swordman",176 },
                {"swordman_at",0 },
                {"fighter",116 },
                {"fighter_at",0 },
                {"gunner",18 },
                {"gunner_at",8},
                {"mage",10 },
                {"mage_at",8 },
                {"priest",150 },
                {"priest_at",0 },
                {"thief",10 },
                {"knight",0 },
                {"demoniclancer",0 }
            };
            var builder = new LSBuilder();
            var obj = builder.ReadJson(Resources.queues);
            obj["Rules"].GetValue(ref sorts);
            var weapon = builder.ReadJson(Resources.weapon);
            weapon.GetValue(ref weapons);
        }
        

        [TestMethod]
        public void GetWeapon() {
            var dir = "d:/avatar/new";
            foreach (var prof in pairs.Keys) {
                foreach (var typed in Directory.GetDirectories($"{dir}/{prof.Replace("_at", "")}/weapon")) {
                    var type = typed.GetSuffix();
                    var file = $"{GAME_PATH}/sprite_character_{prof}{(prof.EndsWith("_at") ? "" : "_")}equipment_weapon_{GetType(prof, type)}.npk";
                    var list = Npks.Load(file);
                    var ps = $"{dir}/{prof}/weapon/{type}";
                    if (!Directory.Exists(ps)) {
                        continue;
                    }
                    var images = Directory.GetFiles(ps);

                    var path = $"d:/avatar/new_image/{prof}/weapon/{type}";
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                    for (var i = 0; i < images.Length; i++) {
                        var regex = new Regex("\\d+");
                        var match = regex.Match(images[i]);
                        if (match.Success) {
                            var code = match.Value;
                            var arr = list.Where(item => Npks.MatchCode(item.Name, code)).ToList();
                            var rs = FindImg(arr, int.Parse(code), type);
                            if (rs.Count() > 0) {
                                var img = rs.ElementAt(0);
                                var builder = new LSBuilder();
                                var image = img[pairs[prof]];
                                ImageToJson(path, prof, type, img.Name, match.Value, image);
                            }
                        }
                    }
                }
            }
        }

        public string GetType(string profession, string type) {
            var alias = weapons[profession.Replace("_at","")].Alias;
            if (alias.ContainsKey(type)) {
                return alias[type];
            }
            return type;
        }


        [TestMethod]
        public void TestMethod1() {
            var dir = "d:/nginx-1.12.2/avatar/";
            var prof = "priest";
            GetImage(dir, "d:/target/", prof);
        }

        public void GetImage(string source,string target,string prof) {
            foreach (var part in part_array) {
                var file = $"{GAME_PATH}/sprite_character_{prof}{(prof.EndsWith("_at") ? "" : "_")}equipment_avatar_{part}.NPK";
                var list = Npks.Load(file);
                var ps = $"{source}/icon/{prof}/{part}";
                if (!Directory.Exists(ps)) {
                    continue;
                }
                var images = Directory.GetFiles(ps);

                var path = $"{target}/image/{prof}/{part}";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                for (var i = 0; i < images.Length; i++) {
                    var regex = new Regex("\\d+");
                    var match = regex.Match(images[i].GetSuffix());
                    if (match.Success) {
                        var code = match.Value;
                        var arr = list.Where(item => Npks.MatchCode(item.Name, code)).ToList();
                        var rs = FindImg(arr, int.Parse(code), part.Equals("skin") ? "body" : part);
                        foreach (var img in rs) {
                            var builder = new LSBuilder();
                            var image = img[pairs[prof]];
                            ImageToJson(path, prof, part, img.Name, match.Value, image);
                        }
                    }
                }
            }
        }

        private IEnumerable<Album> FindImg(List<Album> list, int code, string part) {
            foreach (var album in list) {//二次过滤
                var s = int.Parse(Regex.Match(album.Name, @"\d+").Value);
                if (album.Name.Contains("mask"))//过滤掉染色图层
                    continue;
                if (Regex.IsMatch(album.Name, @"\(.*\)+")) //过滤掉和谐图层
                    continue;
                if ((code == s || code / 100 * 100 == s) && album.Name.Contains(part)) {//过滤掉非同部位的
                    album.TableIndex = code % 100;
                    yield return album;
                }
            }
        }

        public void ImageToJson(string path, string profession, string part, string name, string code, Sprite image) {
            var regex = new Regex("\\d+");
            name = name.Replace(".img", "");
            name = regex.Replace(name, code,1);

            
            var pattern = name.Substring(name.IndexOf("_")+1);
            pattern=regex.Replace(pattern, "");

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
            image.Picture.Save($"{path}/{name}.png");
            builder.WriteObject(dressImage, $"{path}/{name}.json");
        }


        public int IndexOf(string name) {
            if (sorts.ContainsKey(name)) {
                return sorts[name];
            }
            return -1;
        }
    }
}
