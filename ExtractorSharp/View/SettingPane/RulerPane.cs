using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class RulerPane : Panel {

        [Import]
        public IConfig Config;
        public RulerPane() {
            InitializeComponent();
        }

        public void Initialize() {
            displayCrosshairBox.Checked = Config["RulerCrosshair"].Boolean;
            displaySpanBox.Checked = Config["RulerSpan"].Boolean;
        }

        public void Save() {
            Config["RulerCrosshair"] = new ConfigValue(displayCrosshairBox.Checked);
            Config["RulerSpan"] = new ConfigValue(displaySpanBox.Checked);
        }
    }
}