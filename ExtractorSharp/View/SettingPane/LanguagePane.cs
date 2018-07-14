using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.SettingPane {
    public partial class LanguagePane : AbstractSettingPane {
        public LanguagePane(IConnector Data) : base(Data) {
            InitializeComponent();
        }

        public override void Initialize() {
            languageBox.Items.Clear();
            foreach (var item in Connector.LanguageList) {
                languageBox.Items.Add(item);
                if (item.Lcid == Config["LCID"].Integer) {
                    languageBox.SelectedItem = item;
                }
            }
        }

        public override void Save() {
            Config["LCID"] = new ConfigValue((languageBox.SelectedItem as Language)?.Lcid);
        }
    }
}