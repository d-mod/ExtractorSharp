using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class GridPane : AbstractSettingPane {
        public GridPane(IConnector Data) : base(Data) {
            InitializeComponent();
        }

        public override void Initialize() {
            gridGapBox.Value = Config["GridGap"].Decimal;
        }

        public override void Save() {
            Config["GridGap"] = new ConfigValue(gridGapBox.Value);
        }
    }
}