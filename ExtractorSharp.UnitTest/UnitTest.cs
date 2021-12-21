using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtractorSharp.UnitTest {

    [TestClass]
    public class UnitTest {

        [TestMethod]
        public void Test() {
            var content = "<Name><0_01723sw>,<Test>HelloWorld<Message>";
            var dictionary = new Dictionary<string, string> {
                ["Name"] = "Java",
                ["Test"] = "Success",
                ["Message"] = "Thanks!",
                ["0_01723sw"] = "Paris"
            };
            var regex = new Regex(@"<[\w\s\d_]+>");
            var matches = regex.Matches(content);
            for(var i = 0; i < matches.Count; i++) {
                var match = matches[i];
                if(match.Success) {
                    var value = match.Value;
                    var key = value.Substring(1, value.Length - 2);
                    content = content.Replace(value, dictionary[key]);
                }
            }

            Assert.IsTrue(string.Equals(content, "JavaParis,SuccessHelloWorldThanks!"));


        }
    }
}
