using ExtractorSharp.Config;
using ExtractorSharp.Data;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Component {
    public partial class EaseForm : Form {
        private Size BeforeSize;
        public IConfig Config { get; }
        public Language Language { get; }
        public ICommandData Data { get; }

        public EaseForm(ICommandData Data) {
            this.Data = Data;
            this.Config = Data.Config;
            this.Language = Data.Language;
            InitializeComponent();
       }

        protected override void OnResizeBegin(EventArgs e) {
            base.OnResizeBegin(e);
            BeforeSize = Size;
        }

        protected override void OnResizeEnd(EventArgs e) {
            base.OnResizeEnd(e);
            //窗口resize之后的大小
            var endResizeSize = Size;
            //获得变化比例
            var percentWidth = (float)endResizeSize.Width / BeforeSize.Width;
            var percentHeight = (float)endResizeSize.Height / BeforeSize.Height;
            foreach (Control control in Controls) {
                if (control is DataGridView)
                    continue;
                control.Width = (int)(control.Width * percentWidth);
                control.Height = (int)(control.Height * percentHeight);
                var x = (int)(control.Location.X * percentWidth);
                var y = (int)(control.Location.Y * percentHeight);
                control.Location = new Point(x, y);
            }
        }
    }
}
