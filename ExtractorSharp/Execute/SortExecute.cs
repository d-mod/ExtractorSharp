using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Core.Sorter;
using System;

namespace ExtractorSharp.Execute {
    class SortExecute : IExecutable {
        public string Name { set; get; } = "Sort";

        public ISorter Sorter { set; get; }

        public object Execute(params object[] args) {
            return new Comparison<Album>(Sorter.Comparer);
        }
    }
}
