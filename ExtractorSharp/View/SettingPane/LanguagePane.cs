using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Data;
using ExtractorSharp.Core;

namespace ExtractorSharp.View.SettingPane {
    public partial class LanguagePane : AbstractSettingPane {
        public LanguagePane(IConnector Data) :base(Data){
            InitializeComponent();
        }

        public override void Initialize() {
            languageBox.Items.Clear();
            foreach (var item in Data.LanguageList) {
                languageBox.Items.Add(item);
                if (item.LCID == Config["LCID"].Integer) {
                    languageBox.SelectedItem = item;
                }
            }
        }
        public override void Save() {
            Config["LCID"] = new ConfigValue((languageBox.SelectedItem as Language).LCID);
        }
    }
}
