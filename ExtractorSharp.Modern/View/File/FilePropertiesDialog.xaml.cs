using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.View.Model;

namespace ExtractorSharp.View {
    /// <summary>
    /// PropertiesDialog.xaml 的交互逻辑
    /// </summary>
    [ExportMetadata("Name", "FileProperties")]
    [ExportMetadata("Title", "Properties")]
    [Export(typeof(IView))]
    public partial class FilePropertiesDialog : BaseDialog {

        [Import]
        private FilePropertiesModel Model { set; get; }

        public override void OnImportsSatisfied() {
            this.DataContext = this.Model;
            this.Model.ConvertWorker.RunWorkerCompleted += (o, e) => this.Close();
            this.InitializeComponent();
        }

        public override object ShowView(params object[] args) {
            this.Model.RefreshFile();
            return base.ShowView(args);
        }

        private void OnOk(object sender, RoutedEventArgs e) {
            if(!this.Model.ConvertWorker.IsBusy) {
                this.Model.ConvertWorker.RunWorkerAsync();
            }
        }

        private void pathBox_TextChanged(object sender, TextChangedEventArgs e) {

        }


    }
}
