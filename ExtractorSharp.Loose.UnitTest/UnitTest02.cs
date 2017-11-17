using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Loose.Unit {
    [TestClass]
    public class UnitTest02 {

        [TestMethod]
        public void Test15() {
            var entity = new Teacher();
            var root = new LSObject();
            root.Value = entity;
            root.GetValue(ref entity);
      
        }
    }
}
