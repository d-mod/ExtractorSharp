using System.ComponentModel.Composition;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;

namespace ExtractorSharp.View.Dialog {


    [ExportMetadata("Name", "about")]
    [Export(typeof(IView))]
    public partial class AboutDialog : BaseDialog, IPartImportsSatisfiedNotification {

        public AboutDialog() {

        }

        public void OnImportsSatisfied() {
            InitializeComponent();
        }
    }
}