using ExtractorSharp.Core.Lib;
using ExtractorSharp.Handle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Json;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class TestHandler {

        static TestHandler() {
            Handler.Regisity(Img_Version.Ver5, typeof(FifthHandler));

        }

        [TestMethod]
        public void Test() {
            foreach(var f in Directory.GetFiles(@"D:\地下城与勇士\ImagePacks2")) {
                var list=Npks.Load(f);
                foreach (var e in list) {
                    if (e.Version == Img_Version.Ver5) {
                        var handler = e.Handler as FifthHandler;
                        foreach (var a in handler.List) {
                            var image = a.Pictrue;
                        }
                    }

                    GC.Collect();
                }
            }
        }
    }
}
