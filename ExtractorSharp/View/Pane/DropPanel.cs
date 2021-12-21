using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace ExtractorSharp.View.Pane {
    /// <summary>
    ///     历史记录/动作界面
    /// </summary>
    [Export(typeof(DropPanel))]
    public partial class DropPanel : TabControl, IPartImportsSatisfiedNotification {

        [ImportMany(typeof(DropPage))]
        private List<DropPage> pages;

        public DropPanel() {
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            TabPages.AddRange(pages.ToArray());
            this.VisibleChanged += Refresh;
            this.SelectedIndexChanged += Refresh;
        }

        private void Refresh(object sender, System.EventArgs e) {
            SelectedTab?.Refresh();
        }

    }

    public abstract class DropPage : TabPage {

    }
}