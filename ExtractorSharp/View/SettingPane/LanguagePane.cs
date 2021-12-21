using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.SettingPane {
    public partial class LanguagePane : Panel {

        [Import]
        public IConfig Config;

        public LanguagePane() {
            InitializeComponent();
        }

        public void Initialize() {
            languageBox.Items.Clear();
            /*            foreach (var item in Language.List) {
                            languageBox.Items.Add(item);
                            if (item.Lcid == Config["LCID"].Integer) {
                                languageBox.SelectedItem = item;
                            }
                        }*/
        }

        public void Save() {
            Config["LCID"] = new ConfigValue((languageBox.SelectedItem as Language)?.Lcid);
        }
    }
}