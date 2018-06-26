using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.View.Dialog {
    /// <summary>
    ///     修改坐标窗口
    /// </summary>
    internal partial class ChangePositonDialog : ESDialog {
        public ChangePositonDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            yesButton.Click += ChangePosition;
        }


        public override DialogResult Show(params object[] args) {
            x_box.Value = 0;
            x_radio.Checked = true;
            y_box.Value = 0;
            y_radio.Checked = true;
            max_width_box.Value = 0;
            max_height_box.Value = 0;
            return ShowDialog();
        }


        public void ChangePosition(object sender, EventArgs e) {
            var indexes = Connector.CheckedImageIndices;
            var album = Connector.SelectedFile;
            if (allImageCheck.Checked) {
                indexes = new int[album.List.Count];
                for (var i = 0; i < album.List.Count; i++) indexes[i] = i;
            }

            var ins = new[]
                {(int) x_box.Value, (int) y_box.Value, (int) max_width_box.Value, (int) max_height_box.Value};
            var checkes = new[] {
                x_radio.Checked, y_radio.Checked, max_width_radio.Checked, max_height_radio.Checked,
                realativePositionCheck.Checked
            };
            Connector.Do("changePosition", album, indexes, ins, checkes);
            DialogResult = DialogResult.OK;
        }
    }
}