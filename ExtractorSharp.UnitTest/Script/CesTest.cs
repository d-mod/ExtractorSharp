using ExtractorSharp.Script.Ces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest.Script {
    [TestClass]
    public class CesTest {

        [TestMethod]
        public void TestInvoke() {
            var parser = new CesParser();
            parser.Invoke("var a= 3;");

        }
    }
}
