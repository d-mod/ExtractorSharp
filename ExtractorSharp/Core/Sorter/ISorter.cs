using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Core.Sorter {
    public interface ISorter {
        string Name { set; get; }
        Type Type { get; }
        object Data { set; get; }
        int Comparer(Album a1, Album a2);
    }
}
