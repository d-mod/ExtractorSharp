using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using ExtractorSharp.Loose;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest1 {
        private const string game_path = "d:/地下城与勇士/ImagePacks2/";
        private const string url = "d:/api/avatar/";
        private const string image_url = "d:/image/avatar/";
        private const string queues_url = @"E:\ES\ExtractorSharp\Resources\queues.json";
        private const string profession_url = "d:/api/profession.json";
        private string[] parts = { "hair", "cap", "face", "neck", "coat", "skin", "belt", "pants", "shoes" };
        private static Dictionary<string, int> queues;
        private static List<Profession> professiones;
        static UnitTest1() {
            var builder = new LSBuilder();
            var qu = builder.Read(queues_url);
            queues = new Dictionary<string, int>();
            qu.GetValue(ref queues);
            professiones = new List<Profession>();
            qu=builder.Read(profession_url);
            qu.GetValue(ref professiones);
        }



        [TestMethod]
        public void TestMethod1() {
            Convert("priest", 0);
        }


        public void Convert(string Name, int index) {
            var builder = new LSBuilder();
            foreach (var s in parts) {
                var path = url + Name + "/" + s + ".json";
                var o = builder.Read(path);
                var list = o[0].GetValue(typeof(Avatar[])) as Avatar[];
                path = $"{ game_path }sprite_character_{( Name.EndsWith("_at")?Name:Name+"_") }equipment_avatar_{ s }.NPK";
                List<Album> arr = Tools.Load(path);
                foreach (var a in arr) {
                    if (a.Name.Contains("-")||a.Name.Contains("mask") || a.Name.Contains("bmp") || a.Name.Contains("(18)") || a.Name.Contains("(tn)"))
                        continue;
                    foreach (var l in list) {
                        var m = Regex.Match(a.Name, "\\d+");
                        var code = m.Value;
                        if (code.Equals(l.Name)) {
                            var j = a[index];
                            var d = image_url + Name + "/" + s + "/";
                            if (!Directory.Exists(d))
                                Directory.CreateDirectory(d);
                            d += code + a.Name.Substring(m.Index + m.Length).Replace("img","png");
                            if (!File.Exists(d))
                                j.Picture.Save(d);
                            var pi = new Pictrue();
                            var n = a.Name;
                            n = n.Substring(n.IndexOf("_") + 1).Replace(".img", "");
                            n = n.Replace(code, "");
                            pi.Index = queues[n];
                            pi.Name = a.Name.Substring(m.Index).Replace("img", "png");
                            pi.X = j.X;
                            pi.Y = j.Y;
                            var r = new LSObject();
                            r.SetValue(pi);
                            builder.Write(r, d.Replace("png", "json"));
                        }
                    }
                }
            }
            var dir = image_url + Name + "/";
            foreach (var d in Directory.GetDirectories(dir)) {
                var new_url = image_url + Name + "/" + d.Substring(d.LastIndexOf("/") + 1) + "/";
                if (!Directory.Exists(new_url))
                    Directory.CreateDirectory(new_url);
                var dictory = new Dictionary<string, List<LSObject>>();
                foreach (var f in Directory.GetFiles(d, "*.json")) {
                    var m = Regex.Match(f, "\\d+");
                    if (m.Index < 1)
                        continue;
                    var name = m.Value + f.Substring(m.Index + m.Value.Length + 1);
                    if (!dictory.ContainsKey(name))
                        dictory.Add(name, new List<LSObject>());
                    var obj = builder.Read(f);
                    dictory[name].Add(obj);
                    File.Delete(f);
                    var i_name = f.Replace("json", "png");
                    name = f.Substring(m.Index);
                    name = new_url + name.Replace("json", "png");
                }

                foreach (var entry in dictory) {
                    var p = new_url + "/" + entry.Key;
                    var w = new StreamWriter(new FileStream(p, FileMode.Create));
                    w.Write("[");
                    for (var i = 0; i < entry.Value.Count; i++) {
                        var o = entry.Value[i];
                        w.Write(o);
                        if (i < entry.Value.Count - 1)
                            w.Write(",");
                        w.Write("\r\n");
                    }
                    w.Write("]");
                    w.Close();
                }
            }
        }
    }
}
