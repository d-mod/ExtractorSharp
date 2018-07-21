using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.View.SettingPane {
    public partial class MovePane : AbstractSettingPane {
        public MovePane(IConnector Connector):base(Connector){
            InitializeComponent();
            moveStepBar.ValueChanged += MoveStepBarChanged;
            moveStepBox.ValueChanged += MoveStepBoxChanged;
        }

        private void MoveStepBarChanged(object sender, EventArgs e) {
            if (moveStepBox.Value != moveStepBar.Value) {
                moveStepBox.Value = moveStepBar.Value;
            }
        }

        private void MoveStepBoxChanged(object sender, EventArgs e) {
            if (moveStepBox.Value != moveStepBar.Value) {
                moveStepBar.Value = (int)moveStepBox.Value;
            }
        }

        public override void Initialize() {
            moveStepBox.Value = Config["MoveStep"].Integer;
            autoChangePositionCheck.Checked = Config["AutoChangePosition"].Boolean;
        }

        public override void Save() {
            Config["MoveStep"] = new Core.Config.ConfigValue(moveStepBox.Value);
            Config["AutoChangePosition"] = new Core.Config.ConfigValue(autoChangePositionCheck.Checked);
        }

    }
}
