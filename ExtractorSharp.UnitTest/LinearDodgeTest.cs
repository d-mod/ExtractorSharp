using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class LinearDodgeTest {

        [TestMethod]
        public void Test01() {
            var dir = @"C:\Users\krits\Desktop\1\b03\b03.img\";
            var bmp0 = Image.FromFile(dir + "0.png") as Bitmap;
            var bmp2 = Image.FromFile(dir + "1.png") as Bitmap;
            var data0=bmp0.ToArray();
            var data1 = bmp2.ToArray();
            var ds = new FileStream("d:/test/0.data", FileMode.Create);
            ds.Write(data0);
            ds.Close();
            ds = new FileStream("d:/test/1.data", FileMode.Create);
            ds.Write(data1);
            ds.Close();
            for(var i = 0; i < data0.Length; i++) {
                data0[i] = (byte)(data1[i] - data0[i]);
            }
            ds = new FileStream("d:/test/2.data", FileMode.Create);
            ds.Write(data0);
            ds.Close();
        }
    }
}
