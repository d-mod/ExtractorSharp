using System.ComponentModel.Composition;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;

namespace ExtractorSharp.View.Dialog {


    //TODO
    /*    [ExportMetadata("Name", "properties")]
        [Export(typeof(IView))]*/
    public partial class PropertiesDialog : BaseDialog, IPartImportsSatisfiedNotification {
        public void OnImportsSatisfied() {
            InitializeComponent();
        }

        public object Show(params object[] args) {
            return ShowDialog();
        }
    }
}
