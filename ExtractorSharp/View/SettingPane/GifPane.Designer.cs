using ExtractorSharp.Components;

namespace ExtractorSharp.View.SettingPane {
    partial class GifPane {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            delayLabel = new System.Windows.Forms.Label();
            delayBox = new System.Windows.Forms.NumericUpDown();

            backgroundLabel = new System.Windows.Forms.Label();
            backgroundPanel = new ColorPanel();
            backgroundBox = new System.Windows.Forms.TextBox();


            delayLabel.Location = new System.Drawing.Point(50, 42);
            delayLabel.Size = new System.Drawing.Size(60, 30);
            delayLabel.Text = Language["Delay"];

            delayBox.AutoSize = true;
            delayBox.Location = new System.Drawing.Point(120, 40);
            delayBox.Minimum = 1;
            delayBox.Maximum = 10000;

            backgroundLabel.Location = new System.Drawing.Point(50, 122);
            backgroundLabel.Text = Language["BackgroundColor"];
            backgroundLabel.Size = new System.Drawing.Size(100,30);

            backgroundBox.Location = new System.Drawing.Point(220, 120);

            backgroundPanel.Location = new System.Drawing.Point(175, 120);

            Controls.Add(delayLabel);
            Controls.Add(delayBox);
           // Controls.Add(backgroundLabel);
           // Controls.Add(backgroundPanel);
           // Controls.Add(backgroundBox);
           // this.Name = "GIF";
           // this.Parent = "File";
        }


        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.NumericUpDown delayBox;

        private System.Windows.Forms.Label backgroundLabel;
        private System.Windows.Forms.TextBox backgroundBox;

        private ColorPanel backgroundPanel;
        #endregion
    }
}
