using ExtractorSharp.Component;
using ExtractorSharp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog {
    public partial class ChangeSizeDialog : ESDialog {
        public ChangeSizeDialog(IConnector Connector):base(Connector){
            InitializeComponent();
            group.Paint += (o, e) => e.Graphics.Clear(BackColor);
            yesButton.Click += Excecute;
            useDefaultBox.CheckedChanged += UseDefaultScale;
        }

        private void Excecute(object sender, EventArgs e) {
            if (customRadio.Checked) {
                var size = new Size((int)widthBox.Value, (int)heightBox.Value);
                Config["CanvasImageSize"] = new Config.ConfigValue(size);
                Connector.Do("canvasImage",Connector.SelectedFile,size,Connector.CheckedImageIndices);
            } else if(scaleRadio.Checked){
                var scale = scaleBox.Value / 100;
                Config["UseDefaultScale"] = new Config.ConfigValue(useDefaultBox.Checked);
                Connector.Do("changeSize", Connector.SelectedFile, Connector.CheckedImageIndices, scale);
            } else {
                Connector.Do("uncanvasImage", Connector.SelectedFile, Connector.CheckedImageIndices);
            }
            this.DialogResult = DialogResult.OK;
        }

        private void UseDefaultScale(object sender,EventArgs e) {
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
