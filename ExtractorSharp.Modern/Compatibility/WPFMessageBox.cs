using System.ComponentModel.Composition;
using System.Windows;
using ExtractorSharp.Composition.Compatibility;

namespace ExtractorSharp.Compatibility {

    [Export(typeof(ICommonMessageBox))]
    internal class WPFMessageBox : ICommonMessageBox {
        public CommonMessageBoxResult Show(string title, string message, CommonMessageBoxButton button, CommonMessageBoxIcon icon) {
            return (CommonMessageBoxResult)MessageBox.Show(Application.Current.MainWindow,message, title, (MessageBoxButton)button, (MessageBoxImage)icon);
        }
    }
}
