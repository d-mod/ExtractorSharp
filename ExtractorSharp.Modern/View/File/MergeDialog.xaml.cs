using System.ComponentModel.Composition;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.View.Merge;

namespace ExtractorSharp.View {
    /// <summary>
    /// MergeWindow.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IView))]
    [ExportMetadata("Name", "Merge")]
    public partial class MergeDialog : BaseDialog {

        [Import]
        private MergeQueuePage MergeQueuePage;


        public override void OnImportsSatisfied() {
            this.InitializeComponent();
            this.Store.Watch<bool>("/merge/close", value => {
                if(value) {
                    this.Close();
                }
            });
        }

        public override object ShowView(params object[] args) {
            this.mainFrame.Content = this.MergeQueuePage;
            return base.ShowView(args);
        }

    }
}
