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

namespace ExtractorSharp.View.SettingPane {
    public partial class GridPane : AbstractSettingPane {
        public GridPane(ICommandData Data):base(Data){
            InitializeComponent();
        }

        public override void Initialize() {
            gridGapBox.Value = Config["GridGap"].Decimal;
        }

        public override void Save() {
            Config["GridGap"]=new ConfigValue(gridGapBox.Value);
        }
    }
}
