using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Dialog {
    /// <summary>
    ///     修改坐标窗口
    /// </summary>
    [ExportMetadata("Name", "changePosition")]
    [Export(typeof(IView))]
    internal partial class ChangePositonDialog : BaseDialog, IPartImportsSatisfiedNotification {

        [Import]
        private Controller Controller;

        [Import]
        private Store Store;

        public ChangePositonDialog() {
        }

        public override object ShowView(params object[] args) {
            x_box.Value = 0;
            x_radio.Checked = true;
            y_box.Value = 0;
            y_radio.Checked = true;
            max_width_box.Value = 0;
            max_height_box.Value = 0;
            return ShowDialog();
        }


        public void ChangePosition(object sender, EventArgs e) {
            Store.Get("/imagelist/checked-indices", out int[] indices)
                .Get("/filelist/selected-item", out Album album);
            if(allImageCheck.Checked) {
                indices = new int[album.List.Count];
                for(var i = 0; i < album.List.Count; i++) {
                    indices[i] = i;
                }
            }

            Controller.Do("ChangePosition", new CommandContext {
                        { "File", album },
                        { "Indices",indices },
                        { "X" ,x_radio.Checked? (int?)x_box.Value:null},
                        { "Y" , y_radio.Checked? (int?)y_box.Value:null },
                        { "FrameWidth",max_width_radio.Checked? (int?)max_width_box.Value:null },
                        { "FrameHeight",max_height_radio.Checked? (int?)max_height_box.Value:null  },
                        { "Relative" , realativePositionCheck.Checked }
                    });
            DialogResult = DialogResult.OK;
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            yesButton.Click += ChangePosition;
        }
    }
}