using System.ComponentModel.Composition;
using System.Windows;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.View.Model;

namespace ExtractorSharp.View {
    /// <summary>
    /// NewImageDialog.xaml 的交互逻辑
    /// </summary>
    [ExportMetadata("Name", "NewImage")]
    [Export(typeof(IView))]
    public partial class NewImageDialog : BaseDialog {

        [Import]
        private NewImageModel Model;

        public override void OnImportsSatisfied() {
            this.DataContext = this.Model;
            this.InitializeComponent();
        }

        private void OnOk(object sender, RoutedEventArgs e) {
            this.Controller.Do("NewImage", this.Model.Context);
            this.DialogResult = true;
        }
    }
}
