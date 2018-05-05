using System;
using System.IO;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Handle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest.Command {
    [TestClass]
    public class UnitTest1 {

        static UnitTest1() {
            Handler.Regisity(Img_Version.Other, typeof(OtherHandler));
            Handler.Regisity(Img_Version.Ver1, typeof(FirstHandler));
            Handler.Regisity(Img_Version.Ver2, typeof(SecondHandler));
            Handler.Regisity(Img_Version.Ver4, typeof(FourthHandler));
            Handler.Regisity(Img_Version.Ver5, typeof(FifthHandler));
            Handler.Regisity(Img_Version.Ver6, typeof(SixthHandler));
        }


        /// <summary>
        /// 测试IMG保存
        /// </summary>
        [TestMethod]
        public void TestSaveImg() {
            var list = Npks.Load(@"D:\地下城与勇士\ImagePacks2\sprite_character_swordman_equipment_avatar_coat.NPK");
            Assert.IsTrue(list.Count > 0);
            var img = list[0];
            Assert.IsNotNull(img);
            var target = @"d:\test\v6\01.img";
            img.Save(target);
            Assert.IsTrue(File.Exists(target));
            list = Npks.Load(target);
            Assert.IsTrue(list.Count > 0);
            img = list[0];
            Assert.IsNotNull(img);
        }


        [TestMethod]
        public void TestSaveImage() {
            var list = Npks.Load(@"D:\test\v6\01.img");
            Assert.IsTrue(list.Count > 0);
            var img = list[0];
            Assert.IsNotNull(img);
            var dir = @"D:\test\v6\image\";
            if (Directory.Exists(dir)) {
                Directory.Delete(dir,true);
                Directory.CreateDirectory(dir);
            }
            img.List.ForEach(e => e.Picture.Save( dir+ e.Index + ".png"));
            Assert.IsTrue(Directory.GetFiles(dir, "*.png").Length == img.Count);
        }


        [TestMethod]
        public void TestDoCommand() {
            var controller = new Controller();
          
        }



    }
}
