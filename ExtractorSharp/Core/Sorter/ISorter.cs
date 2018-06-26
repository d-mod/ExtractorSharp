using System;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Sorter {
    public interface ISorter {
        string Name { set; get; }
        Type Type { get; }
        object Data { set; get; }
        int Comparer(Album a1, Album a2);
    }
}