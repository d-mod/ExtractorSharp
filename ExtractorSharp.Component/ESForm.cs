using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Component {
    public partial class ESForm : Form {
        private Size BeforeSize;

        public ESForm(IConnector Connector) {
            this.Connector = Connector;
            if (Connector != null) {
                Config = Connector.Config;
                Language = Connector.Language;
            }
            InitializeComponent();
        }

        public IConfig Config { get; }
        public Language Language { get; }
        public IConnector Connector { get; }

        protected override void OnResizeBegin(EventArgs e) {
            base.OnResizeBegin(e);
            BeforeSize = Size;
        }

        protected override void OnResizeEnd(EventArgs e) {
            base.OnResizeEnd(e);
            //窗口resize之后的大小
            var endResizeSize = Size;
            //获得变化比例
            var percentWidth = (float) endResizeSize.Width / BeforeSize.Width;
            var percentHeight = (float) endResizeSize.Height / BeforeSize.Height;
            foreach (Control control in Controls) {
                if (control is DataGridView) {
                    continue;
                }
                control.Width = (int) (control.Width * percentWidth);
                control.Height = (int) (control.Height * percentHeight);
                var x = (int) (control.Location.X * percentWidth);
                var y = (int) (control.Location.Y * percentHeight);
                control.Location = new Point(x, y);
            }
        }
    }
}