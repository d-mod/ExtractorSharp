using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Handle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest.Command {
    [TestClass]
    public class UnitTest1 {
        static UnitTest1() {
            Handler.Regisity(ImgVersion.Other, typeof(OtherHandler));
            Handler.Regisity(ImgVersion.Ver1, typeof(FirstHandler));
            Handler.Regisity(ImgVersion.Ver2, typeof(SecondHandler));
            Handler.Regisity(ImgVersion.Ver4, typeof(FourthHandler));
            Handler.Regisity(ImgVersion.Ver5, typeof(FifthHandler));
            Handler.Regisity(ImgVersion.Ver6, typeof(SixthHandler));
        }


    /// <summary>
    ///     测试IMG保存
    /// </summary>
    [TestMethod]
        public void TestSaveImg() {
            var list = NpkCoder.Load(@"D:\地下城与勇士\ImagePacks2\sprite_character_swordman_equipment_avatar_coat.NPK");
            Assert.IsTrue(list.Count > 0);
            var img = list[0];
            Assert.IsNotNull(img);
            var target = @"d:\test\v6\01.img";
            img.Save(target);
            Assert.IsTrue(File.Exists(target));
            list = NpkCoder.Load(target);
            Assert.IsTrue(list.Count > 0);
            img = list[0];
            Assert.IsNotNull(img);
        }


        [TestMethod]
        public void TestSaveImage() {
            var list = NpkCoder.Load(@"D:\test\v6\01.img");
            Assert.IsTrue(list.Count > 0);
            var img = list[0];
            Assert.IsNotNull(img);
            var dir = @"D:\test\v6\image\";
            if (Directory.Exists(dir)) {
                Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);
            }
            img.List.ForEach(e => e.Picture.Save(dir + e.Index + ".png"));
            Assert.IsTrue(Directory.GetFiles(dir, "*.png").Length == img.Count);
        }


        [TestMethod]
        public void TestInvoke()
        {
            var commandParser = new CommandParser();
            Assert.AreEqual(commandParser.ParseInvoke("|asNull"), null);
            Assert.AreEqual(commandParser.ParseInvoke("12342134sdgwseg|asNull"), null);

            Assert.AreEqual(commandParser.ParseInvoke("true|toBool"), true);
            Assert.AreEqual(commandParser.ParseInvoke("123|toBool"), true);
            Assert.AreEqual(commandParser.ParseInvoke("false|toBool"), false);

            Assert.AreEqual(commandParser.ParseInvoke("1"), "1");
            Assert.AreEqual(commandParser.ParseInvoke("1|toInt"), 1);
            Assert.IsTrue(((commandParser.ParseInvoke("1|toList") as string[]) ?? Array.Empty<string>()).SequenceEqual(new[] { "1" }));
            Assert.IsTrue(((commandParser.ParseInvoke("1|toList|toInt") as int[]) ?? Array.Empty<int>()).SequenceEqual(new[] {1}));

            Assert.IsTrue(((commandParser.ParseInvoke("1,2,3|toList") as string[]) ?? Array.Empty<string>()).SequenceEqual(new[] { "1", "2", "3" }));
            Assert.IsTrue(((commandParser.ParseInvoke("1,2,3|toList|toInt") as int[]) ?? Array.Empty<int>()).SequenceEqual(new[] { 1, 2, 3 }));
            commandParser.ParseInvoke("1,2,3|toList|toInt|asVar|a");
            Assert.IsTrue((commandParser.ParseInvoke("|useVar|a") as int[] ?? Array.Empty<int>()).SequenceEqual(new[] { 1, 2, 3 }));

            commandParser.ParseInvoke("1,2,3|toList|asVar|a");
            Assert.IsTrue((commandParser.ParseInvoke("|useVar|a|toInt") as int[] ?? Array.Empty<int>()).SequenceEqual(new[] { 1, 2, 3 }));

            commandParser.ParseInvoke("|useVar|a|message");
        }
    }
}