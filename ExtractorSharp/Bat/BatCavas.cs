using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;
using ExtractorSharp.View;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Bat {
   partial class BatCavas : EaseDialog,Batch {
        public BatCavas(){
            InitializeComponent();
            CancelButton = cancelButton;
            yesButton.Click += Run;
        }

        public void Run(object sender,EventArgs e) {
            DialogResult = DialogResult.OK;
            Visible = false;
        }


        public override DialogResult Show(params object[] args) {
            return ShowDialog();
        }

        public bool Run(Controller Controller, ImageEntity[] array, ref bool running, ProgressBar bar) {
            if (Show() == DialogResult.OK) {
                var x = (int)wdith_box.Value;
                var y = (int)height_box.Value;
                bar.Maximum = array.Length;
                bar.Value = 0;
                foreach (var entity in array) {
                    if (!running)
                        return false;
                    entity.CavasImage(new Size(x, y));
                    bar.Value++;
                }
                Messager.ShowOperate("CavasImage");
                return true;
            }
            return false;
        }

        public override string ToString() {
            return Language["CavasImage"];
        }

    }   
}
