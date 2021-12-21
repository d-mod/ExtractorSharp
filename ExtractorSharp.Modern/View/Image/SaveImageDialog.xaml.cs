using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExtractorSharp.Components;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.View;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;
using ExtractorSharp.View.Model;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.View.Image {
    /// <summary>
    /// SaveImageDialog.xaml 的交互逻辑
    /// </summary>
    [ExportView("SaveImage")]
    public partial class SaveImageDialog : BaseDialog {

        [Import]
        SaveImageModel Model;

        public override void OnImportsSatisfied() {
            this.DataContext = this.Model;
            this.InitializeComponent();
        }

        

        private void Browse(object sender, RoutedEventArgs e) {
            var dialog = new CommonOpenFileDialog {
                IsFolderPicker = true
            };
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                this.Model.Target = dialog.FileName;
            }
        }

        protected override void OnShowing(params object[] args) {
            this.Model.File = this.Store.Get<Album>(StoreKeys.SELECTED_FILE);
            this.Model.Indices = this.Store.Get<int[]>(StoreKeys.SELECTED_FILE_INDICES);
        }

        private void OnOk(object sender, RoutedEventArgs e) {
            this.Controller.Do("SaveImage", CommandContext.CreateFrom(this.Model));
        }
    }
}
