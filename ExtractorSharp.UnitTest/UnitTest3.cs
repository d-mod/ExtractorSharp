using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest3 {
        [TestMethod]
        public void Test01() {
            byte[] bs = { 0xda, 0xd6 };
            byte r = 0;
            byte g = 0;
            byte a = 0;
            byte b = 0;
            a = (byte)(bs[1] >> 7);
            r = (byte)((bs[1] >> 2) & 0x1f);
            g = (byte)((bs[0] >> 5) | (bs[1] & 3) << 3);
            b = (byte)(bs[0] & 0x1f);
            a = (byte)(a * 0xff);
            r = (byte)(r << 3 | r >> 2);
            g = (byte)(g << 3 | g >> 2);
            b = (byte)(b << 3 | b >> 2);

            Assert.Equals(r, 1);
           
        }
    }
}
