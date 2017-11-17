using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ExtractorSharp.Core;
using ExtractorSharp.Loose;
using ExtractorSharp.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest2 {

        private const string queues_url = @"D:\ES\ExtractorSharp\Resources\queues.json";
        private const string profession_url = "D:/Avatar/web/api/profession.json";
        private const string game_path = "d:/地下城与勇士/ImagePacks2/";
        private static Dictionary<string, int> queues;
        private static List<Profession> professiones;
        private string[] parts = { "hair", "cap", "face", "neck", "coat", "skin", "belt", "pants", "shoes" };

        static UnitTest2() {
            var builder = new LSBuilder();
            var qu = builder.Read(queues_url);
            queues = new Dictionary<string, int>();
            qu.GetValue(ref queues);
            professiones = new List<Profession>();
            qu = builder.Read(profession_url);
            qu.GetValue(ref professiones);

        }

        [TestMethod]
        public void TestMethod1() {
            FitRoomService service = new FitRoomService();
            var list=service.Init();
            var weapons = service.GetWeapon(0);
            Assert.IsNotNull(weapons);
        }

        [TestMethod]
        public void TestMethod2() {
            var album = new Album {
                Path = "0001.img"
            };
            var album2 = new Album() {
                Path = "0000.img"
            };
            var list = Tools.Find(new Album[] { album2 }, "0001.img");
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void Test03() {
            var path = "d:/地下城与勇士/ImagePacks2";
            foreach (var f in Directory.GetFiles(path)) {
                var albums = Tools.Load(f);
            }
        }

        [TestMethod]
        public void Test05() {
            Assert.IsNotNull(professiones);
            foreach(var p in professiones) {
                foreach(var pa in parts) {
                    Test04(p.Name, pa);
                }
            }
        }

        public void Test04(string Name, string s) {
            var f = $"{ game_path }sprite_character_{(Name.EndsWith("_at") ? Name : Name + "_") }equipment_avatar_{ s }.NPK";
            var arr = Tools.Load(f);
            foreach (var a in arr) {
                if (a.Name.Contains("-") || a.Name.Contains("mask") || a.Name.Contains("bmp") || a.Name.Contains("(18)") || a.Name.Contains("(tn)"))
                    continue;
                var merger = new Merger();
                var i = merger.IndexOf(a.Name);
                if (i < 0)
                    Assert.Fail();
            }
        }

        [TestMethod()]
        public void Test06() {
            var controller = new Controller();
            Album album = new Album();
            var image = new ImageEntity(album);
            image.X = 20;
            image.Y = 100;
            album.List.Add(image);
            controller.Do("changePosition", album,new int[] { 0 }, new int[] { 100, 0, 0, 0 }, new bool[] { true, false, false, false, true });
            Assert.IsTrue(image.X == 120);
        }
    }
}
