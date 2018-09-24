using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.UnitTest {

    [TestClass]
    public class DictionaryTest {

        public const string GamePath = "D:\\地下城与勇士";

        [TestMethod]
        public void GetCategory() {
            var list = LoadFileLst($"{GamePath}\\auto.lst");
            var builder = new LSBuilder();
            var rs = new List<EntryInfo>();
            foreach (var f in list) {
                var file = $"{GamePath}\\{f}";
                var stream = File.OpenRead(file);
                var infos = NpkCoder.ReadInfo(stream);
                var array = infos.ConvertAll(e => new EntryInfo() {
                    path = e.Path,
                    length = e.Length
                });
                rs.AddRange(array);
                stream.Close();
            }
            builder.WriteObject(rs, "D:\\entries.json");
        }

        private static IEnumerable<string> LoadFileLst(string file) {
            if (File.Exists(file)) {
                var fs = new StreamReader(file);
                while (!fs.EndOfStream) {
                    var str = fs.ReadLine();
                    str = str.Replace("\"", "");
                    var dt = str.Split(" ");
                    if (dt.Length < 1) {
                        continue;
                    }
                    if (dt[0].EndsWith(".NPK")) {
                        yield return dt[0];
                    }
                }
                fs.Close();
            }
        }
    }
}
