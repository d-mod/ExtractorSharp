using System;
using System.IO;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Handle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class TestHandler {
        static TestHandler() {
            Handler.Regisity(ImgVersion.Ver5, typeof(FifthHandler));
        }

        [TestMethod]
        public void Test() {
            foreach (var f in Directory.GetFiles(@"D:\地下城与勇士\ImagePacks2")) {
                var list = NpkCoder.Load(f);
                foreach (var e in list) {
                    if (e.Version == ImgVersion.Ver5) {
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