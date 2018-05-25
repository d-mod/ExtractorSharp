using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class UnitTest2 {
        public static Dictionary<string, int> pairs = new Dictionary<string, int>();
        public static string GAME_PATH = "d:/地下城与勇士/ImagePacks2/";
        public static Dictionary<string, int> sorts = new Dictionary<string, int>();
        public static Dictionary<string, string> p2 = new Dictionary<string, string>();
        static UnitTest2() {
            Handler.Regisity(Img_Version.Other, typeof(OtherHandler));
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
                {"demoniclancer",0 },
                {"gunblader",0 }
            };
            p2 = new Dictionary<string, string>() {
                {"swordman","sm"},
                {"swordman_at","sg"},
                {"fighter","ft" },
                {"fighter_at","fm" },
                {"gunner","gn" },
                {"gunner_at","gg"},
                {"mage","mg"},
                {"mage_at","mm" },
                {"priest","pr" },
                {"priest_at","pg" },
                {"thief","th"},
                {"knight","kn" },
                {"demoniclancer","dl"},
                {"gunblader" ,"gb"},
            };
            var builder = new LSBuilder();
            var obj = builder.ReadJson(Resources.queues);
            obj["Rules"].GetValue(ref sorts);
        }
        public string[] part_array = { "cap", "coat", "belt", "neck", "hair", "face", "skin", "pants", "shoes" };


        [TestMethod]
        public void LinearDodge() {
            var dir = @"D:\nginx-1.12.2\avatar\image";
            Dictionary<string, Bitmap> map = new Dictionary<string, Bitmap>();
            foreach (var prof in Directory.GetDirectories(dir)) {
                foreach (var part in Directory.GetDirectories(prof)) {
                    foreach (var file in Directory.GetFiles(part,"*f.png")) {
                        Bitmap bmp = Image.FromFile(file) as Bitmap;
                        map.Add(file.Replace(@"\nginx-1.12.2\avatar",@"\avatar_ex"), bmp.LinearDodge());
                    }
                }
            }
            foreach(var key in map.Keys) {
                map[key].Save(key);
            }
        }


        [TestMethod]
        public void TestMethod3() {
            var dir = @"D:\avatar_new\image";
            foreach (var prof in pairs.Keys) {
                foreach (var part in part_array) {
                    var file = $@"{dir}\{prof}\{part}";

                    var path = $"d:/avatar_ex/image/{prof}/{part}";
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                    var list = Npks.Load(file);

                    foreach (var img in list) {
                        var tables = img.Tables;
                        var match = Regex.Match(img.Name, "\\d+");
                        if (match.Success) {
                            var code = int.Parse(match.Value);
                            for (var i = 0; i < tables.Count; i++) {
                                img.TableIndex = i;
                                var image = img[pairs[prof]];
                                ImageToJson(path, prof, part, img.Name, Npks.CompleteCode(code + i), image);
                            }
                        }
                    }
                }
            }
        }




        [TestMethod]
        public void TestMethod1() {
            var dir = @"D:\avatar_ex\icon";
            foreach (var prof in pairs.Keys) {
                foreach (var part in part_array) {
                    var file = $"{GAME_PATH}/sprite_character_{prof}{(prof.EndsWith("_at") ? "" : "_")}equipment_avatar_{part}.NPK";
                    var list = Npks.Load(file);
                    var ps = $"{dir}/{prof}/{part}";
                    if (!Directory.Exists(ps)) {
                        continue;
                    }
                    var images = Directory.GetFiles(ps);

                    var path = $"d:/avatar_ex/image/{prof}/{part}";
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                    for (var i = 0; i < images.Length; i++) {
                        var regex = new Regex("\\d+");
                        var match = regex.Match(images[i]);
                        if (match.Success) {
                            var code = match.Value;
                            var arr = Npks.FindByCode(list, code);
                            var rs = FindImg(arr, int.Parse(code), part.Equals("skin") ? "body" : part);
                            var builder = new LSBuilder();
                            foreach (var img in rs) {
                                var image = img[pairs[prof]];
                                ImageToJson(path, prof, part, img.Name, match.Value, image);
                            }
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