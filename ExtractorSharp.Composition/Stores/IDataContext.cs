using System.ComponentModel;

namespace ExtractorSharp.Composition.Stores {
    internal interface IDataContext {
        INotifyPropertyChanged DataContext { set; get; }
    }
}
