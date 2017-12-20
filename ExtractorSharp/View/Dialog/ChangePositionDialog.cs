using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Config;

namespace ExtractorSharp.View {
    /// <summary>
    /// 修改坐标窗口
    /// </summary>
    internal partial class ChangePositonDialog : EaseDialog {
        private Controller Controller => Program.Controller;
        private MainForm MainForm => Program.Form;

        public ChangePositonDialog(ICommandData Data) : base(Data) {
            InitializeComponent();
            yesButton.Click += ChangePosition;
        }


        public override DialogResult Show(params object[] args) {
            var entitys = args as ImageEntity[];
            x_box.Value = 0;
            x_radio.Checked = true;
            y_box.Value = 0;
            y_radio.Checked = true;
            max_width_box.Value = 0;
            max_height_box.Value = 0;
            return ShowDialog();
        }


        public void ChangePosition(object sender, EventArgs e) {
            var indexes = Data.CheckedImageIndices;
            var album = Data.SelectedFile;
            if (allImageRadio.Checked) {
                indexes = new int[album.List.Count];
                for (var i = 0; i < album.List.Count; i++) {
                    indexes[i] = i;
                }
            }
            var ins = new int[] {(int)x_box.Value, (int)y_box.Value, (int)max_width_box.Value, (int)max_height_box.Value };
            var checkes = new bool[] {x_radio.Checked,y_radio.Checked,max_width_radio.Checked,max_height_radio.Checked,checkbox.Checked};
            Controller.Do("changePosition", album,indexes,ins,checkes);
            DialogResult = DialogResult.OK;
        }
        
    }
}
