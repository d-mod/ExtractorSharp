
using System.ComponentModel.Composition;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;

namespace ExtractorSharp.View.Image {
    /// <summary>
    /// ImagePropertiesDialog.xaml 的交互逻辑
    /// </summary>
    [ExportMetadata("Name", "ImageProperties")]
    [ExportMetadata("Title", "Properties")]
    [Export(typeof(IView))]
    public partial class ImagePropertiesDialog : BaseDialog {
        public override void OnImportsSatisfied() {
            this.InitializeComponent();
        }
    }
}
