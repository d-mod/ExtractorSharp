using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;

namespace ExtractorSharp.Components {

    public partial class BaseForm : Form {

        [Import]
        protected Language Language { set; get; }

        [Import]
        protected Store Store { set; get; }

        [Import]
        protected Controller Controller { set; get; }

        [Import]
        protected IConfig Config { set; get; }

        [Import]
        protected Messager Messager { set; get; }

        private Size BeforeSize;

        public BaseForm() {
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
            foreach(Control control in Controls) {
                if(control is DataGridView) {
                    continue;
                }
                control.Width = (int)(control.Width * percentWidth);
                control.Height = (int)(control.Height * percentHeight);
                var x = (int)(control.Location.X * percentWidth);
                var y = (int)(control.Location.Y * percentHeight);
                control.Location = new Point(x, y);
            }
        }
    }
}