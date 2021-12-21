using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.View.Model;

namespace ExtractorSharp.View.Merge {
    /// <summary>
    /// MergeResult.xaml 的交互逻辑
    /// </summary>
    [Export]
    public partial class MergeResultPage : Page, IPartImportsSatisfiedNotification {

        [Import]
        private MergeModel Model;

        [Import]
        private Controller Controller;

        [Import]
        private Store Store;




        public void OnImportsSatisfied() {
            this.DataContext = this.Model;
            this.InitializeComponent();
        }



        private void Execute(object sender, RoutedEventArgs e) {
            if(this.Model.ResultFile != null) {
                this.Controller.Do("replaceFile", this.Model.ReplaceFileContext);
                this.Close(sender, e);
            }
        }

        private void Close(object sender, RoutedEventArgs e) {
            this.Store.Set("/merge/close", true);
        }
    }
}
