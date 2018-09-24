using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Core.Sorter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Execute {
    class SortIndexOfExecute : IExecutable {
        public string Name { set; get; } = "SortIndexOf";

        public ISorter Sorter { set; get; }

        public object Execute(params object[] args) {
            return new Converter<Album, int>(Sorter.IndexOf);
        }
    }
}
