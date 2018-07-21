using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.View.Dialog {
    public partial class ChangeSizeDialog : ESDialog {
        public ChangeSizeDialog(IConnector connector) : base(connector) {
            InitializeComponent();
            group.Paint += (o, e) => e.Graphics.Clear(BackColor);
            yesButton.Click += Excecute;
            useDefaultBox.CheckedChanged += UseDefaultScale;
        }

        private void Excecute(object sender, EventArgs e) {
            if (customRadio.Checked) {
                var size = new Size((int) widthBox.Value, (int) heightBox.Value);
                Config["CanvasImageSize"] = new ConfigValue(size);
                Connector.Do("canvasImage", Connector.SelectedFile, size, Connector.CheckedImageIndices);
            } else if (scaleRadio.Checked) {
                var scale = scaleBox.Value / 100;
                Config["UseDefaultScale"] = new ConfigValue(useDefaultBox.Checked);
                Connector.Do("changeSize", Connector.SelectedFile, Connector.CheckedImageIndices, scale);
            } else {
                Connector.Do("uncanvasImage", Connector.SelectedFile, Connector.CheckedImageIndices);
            }

            DialogResult = DialogResult.OK;
        }

        private void UseDefaultScale(object sender, EventArgs e) {
            scaleBox.Enabled = !useDefaultBox.Checked;
            if (useDefaultBox.Checked) {
                scaleBox.Value = Config["CanvasScale"].Integer;
            }
        }

        public override DialogResult Show(params object[] args) {
            useDefaultBox.Checked = Config["UseDefaultScale"].Boolean;
            if (useDefaultBox.Checked) {
                scaleBox.Value = Config["CanvasScale"].Integer;
            }
            var size = Config["CanvasImageSize"].Size;
            widthBox.Value = size.Width;
            heightBox.Value = size.Height;
            return ShowDialog();
        }
    }
}