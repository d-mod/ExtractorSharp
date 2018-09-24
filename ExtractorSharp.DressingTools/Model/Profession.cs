using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.DressingTools.Model {
    class Profession {
        public string Name { set; get; }
        public int Frame { set; get; }
        public Dictionary<string, string> Transfer { set; get; } = new Dictionary<string, string>();
        public List<string> Weapon { set; get; } = new List<string>();
    }
}
