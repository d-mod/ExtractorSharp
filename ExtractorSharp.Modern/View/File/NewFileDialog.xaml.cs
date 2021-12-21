using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.View.Model;

namespace ExtractorSharp.View {
    /// <summary>
    /// NewFileDialog.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IView))]
    [ExportMetadata("Name", "NewFile")]
    public partial class NewFileDialog : BaseDialog {

        [Import]
        private NewFileModel ViewModel;


        public override void OnImportsSatisfied() {
            this.DataContext = this.ViewModel;
            this.InitializeComponent();
        }

        public override object ShowView(params object[] args) {
            var index = 0;
            if(args.Length > 0) {
                index = (int)args[0];
            }
            this.ViewModel.InsertOffset = index;
            return base.ShowView(args);
        }


        private void OnOk(object sender, RoutedEventArgs e) {
            var path = this.ViewModel.FilePath;
            if(path.GetSuffix().Equals(string.Empty)) {
                this.Messager.Error("FileNameCannotEmpty");
                return;
            }
            var count = this.ViewModel.Count;
            var index = this.ViewModel.InsertOffset;
            var version = this.ViewModel.FileVersion;
            this.Controller.Do("NewFile", new CommandContext() {
                {"Path",path },
                {"Count",count },
                {"Index",index },
                {"Version",version }
            });
            this.ViewModel.FilePath = path.Replace(path.GetSuffix(), "");
            this.DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }
    }
}
