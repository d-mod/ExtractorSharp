using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Data;

namespace ExtractorSharp.View {
    partial class CanvasDialog : EaseDialog{
        private Album Album;
        private int[] Indexes;
        private Size CavasSize => new Size((int)width_box.Value, (int)height_box.Value);
        public CanvasDialog(ICommandData Data) : base(Data) {
            InitializeComponent();
            CancelButton = cancelButton;
            yesButton.Click += Run;
        }

        public void Run(object sender, EventArgs e) {
            Program.Controller.Do("cavasImage", Album, CavasSize, Indexes);
            DialogResult = DialogResult.OK;
        }


        public override DialogResult Show(params object[] args) {
            Album = args[0] as Album;
            Indexes = args[1] as int[];
            var width = (int)width_box.Value;
            var height = (int)height_box.Value;
            foreach(var i in Indexes) {
                var entity = Album.List[i];
                if (entity.Width > width)
                    width = entity.Width;
                if (entity.Height > height)
                    height = entity.Height;
            }
            width_box.Value = width;
            height_box.Value = height;
            return ShowDialog();
        }

    }
}
