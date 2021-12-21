using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.View.Dialog {


    /// <summary>
    /// 修改贴图大小窗口
    /// </summary>
    [ExportMetadata("Name", "changeSize")]
    [Export(typeof(IView))]
    public partial class ChangeSizeDialog : BaseDialog, IPartImportsSatisfiedNotification {



        public ChangeSizeDialog() {
        }

        private void Excecute(object sender, EventArgs e) {

            Store.Get(StoreKeys.SELECTED_FILE, out Album file)
                .Get("/imagelist/checked-indices", out int[] indices);
            if(customRadio.Checked) {
                var size = new Size((int)widthBox.Value, (int)heightBox.Value);
                Config["CanvasImageSize"] = new ConfigValue(size);
                Controller.Do("CanvasImage", new CommandContext {
                    { "File" ,file },
                    { "Indices", indices },
                    { "Size", size }
                });
            } else if(scaleRadio.Checked) {
                var scale = scaleBox.Value / 100;
                Config["UseDefaultScale"] = new ConfigValue(useDefaultBox.Checked);
                Controller.Do("ChangeSize", new CommandContext {
                    { "File" ,file },
                    { "Indices", indices },
                    { "Scale", scale }
                });
            } else {
                Controller.Do("TrimImage", new CommandContext {
                    { "File" ,file },
                    { "Indices", indices }
                });
            }

            DialogResult = DialogResult.OK;
        }

        private void GetPoint() {

        }

        private void UseDefaultScale(object sender, EventArgs e) {
            scaleBox.Enabled = !useDefaultBox.Checked;
            if(useDefaultBox.Checked) {
                scaleBox.Value = Config["CanvasScale"].Integer;
            }
        }

        public override object ShowView(params object[] args) {
            useDefaultBox.Checked = Config["UseDefaultScale"].Boolean;
            if(useDefaultBox.Checked) {
                scaleBox.Value = Config["CanvasScale"].Integer;
            }
            var size = Config["CanvasImageSize"].Size;
            widthBox.Value = size.Width;
            heightBox.Value = size.Height;
            return ShowDialog();
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            group.Paint += (o, e) => e.Graphics.Clear(BackColor);
            yesButton.Click += Excecute;
            useDefaultBox.CheckedChanged += UseDefaultScale;
        }
    }
}