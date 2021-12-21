using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.View.Model;

namespace ExtractorSharp.View.Merge {
    /// <summary>
    /// MergePageQueue.xaml 的交互逻辑
    /// </summary>
    [Export]
    public partial class MergeQueuePage : Page, IPartImportsSatisfiedNotification {

        [Import]
        private MergeModel Model;

        [Import]
        private Controller Controller;

        [Import]
        private MergeResultPage MergeResultPage;

        public void OnImportsSatisfied() {
            this.DataContext = this.Model;
            this.InitializeComponent();
        }

        private void Sort(object sender, RoutedEventArgs e) {
            this.Controller.Do("SortMerge");
        }

        private void Merge(object sender, RoutedEventArgs e) {
            this.Model.MergeWorker.RunWorkerAsync();
            this.NavigationService.Navigate(this.MergeResultPage);

        }

        private void AddFile(object sender, RoutedEventArgs e) {

        }
    }
}
