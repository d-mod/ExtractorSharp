using ExtractorSharp.Core.Lib;
using ExtractorSharp.Handle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class TestHandler {

        static TestHandler() {
            Handler.Regisity(Img_Version.Other, typeof(OtherHandler));
            Handler.Regisity(Img_Version.Ver1, typeof(FirstHandler));
            Handler.Regisity(Img_Version.Ver2, typeof(SecondHandler));
            Handler.Regisity(Img_Version.Ver4, typeof(FourthHandler));
            Handler.Regisity(Img_Version.Ver5, typeof(FifthHandler));
            Handler.Regisity(Img_Version.Ver6, typeof(SixthHandler));

        }

        [TestMethod]
        public void Test() {
            foreach(string f in Directory.GetFiles(@"D:\地下城与勇士\ImagePacks2")) {
                Npks.Load(f);
            }
        }
    }
}
