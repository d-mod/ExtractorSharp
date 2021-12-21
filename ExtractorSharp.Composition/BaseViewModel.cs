using System.ComponentModel;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;

namespace ExtractorSharp {
    public class BaseViewModel : InjectService, INotifyPropertyChanged {


        public event PropertyChangedEventHandler PropertyChanged;
 

        public void OnPropertyChanged(params string[] propertyNames) {
            foreach(var propertyName in propertyNames) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }

    }
}