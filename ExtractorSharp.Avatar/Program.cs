using ExtractorSharp.Loose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Avatar {
    class Program {

        static void Main(string[] args) {

        }

        public static void Load(string profession, string part) {
            var url = $"http://extractorsharp.kritsu.net/api/avtar/icon";
            var builder = new LSBuilder();
            var data = new Dictionary<string, object>() {
                 ["profession"]=profession,
                 ["part"]=part
            };
            var obj=builder.Get(url, data);
        }
    }
}
