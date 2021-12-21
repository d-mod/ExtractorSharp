using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class GridPane : Panel {

        [Import]
        public IConfig Config;

        public GridPane() {
            InitializeComponent();
        }

        public void Initialize() {
            gridGapBox.Value = Config["GridGap"].Decimal;
        }

        public void Save() {
            Config["GridGap"] = new ConfigValue(gridGapBox.Value);
        }
    }
}