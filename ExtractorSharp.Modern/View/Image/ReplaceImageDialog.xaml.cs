using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.View.Model;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.View {
    /// <summary>
    /// ReplaceImageDialog.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IView))]
    [ExportMetadata("Name", "ReplaceImage")]
    public partial class ReplaceImageDialog : BaseDialog {

        [Import]
        private ReplaceImageModel Model;

        public override void OnImportsSatisfied() {
            this.DataContext = this.Model;
            this.InitializeComponent();
        }

        private void OnOk(object sender, RoutedEventArgs e) {


            var dialog = new CommonOpenFileDialog {
            };
            var mode = 0;
            if(this.Model.IsFromGif) {
                mode = 1;
                dialog.Filters.Add(new CommonFileDialogFilter("GIF", ".gif"));
            } else if(this.Model.TargetImages.Count() == 1) {
                dialog.Filters.Add(new CommonFileDialogFilter(this.Language["ImageResources"], ".png;.jpg;.bmp"));
            } else {
                mode = 2;
                dialog.IsFolderPicker = true;
            }
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                var context = new CommandContext {
                    { "Mode", mode },
                    { "Path", dialog.FileName }
                };
                context.Import(this.Model);
                this.Controller.Do("ReplaceImage", context);
                this.DialogResult = true;
            }
        }
    }
}
