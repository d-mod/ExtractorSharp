using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class RulerPane : AbstractSettingPane {
        public RulerPane(IConnector connector) : base(connector) {
            InitializeComponent();
        }

        public override void Initialize() {
            displayCrosshairBox.Checked = Config["RulerCrosshair"].Boolean;
            displaySpanBox.Checked = Config["RulerSpan"].Boolean;
        }

        public override void Save() {
            Config["RulerCrosshair"] = new ConfigValue(displayCrosshairBox.Checked);
            Config["RulerSpan"] = new ConfigValue(displaySpanBox.Checked);
        }
    }
}