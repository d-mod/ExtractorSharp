using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog {
    partial class DownloadFileDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.fileList = new ExtractorSharp.UI.EaseListBox<string>();
            this.searchButton = new ExtractorSharp.UI.EaseButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.loadButton = new ExtractorSharp.UI.EaseButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bar = new ProgressBar();
            this.tipLabel = new Label();
            downloadItem = new ToolStripMenuItem();
            this.SuspendLayout();
            // 
            // fileList
            // 
            this.fileList.BackColor = System.Drawing.Color.White;
            this.fileList.Location = new System.Drawing.Point(25, 86);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(581, 340);
            this.fileList.TabIndex = 0;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(235, 16);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(93, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "检索文件列表";
            this.searchButton.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(94, 18);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "选择服务器";
            // 
            // textBox1
            // 
            this.pathBox.Location = new System.Drawing.Point(352, 15);
            this.pathBox.Name = "textBox1";
            this.pathBox.Size = new System.Drawing.Size(173, 21);
            this.pathBox.TabIndex = 5;
            // 
            // easeButton2
            // 
            this.loadButton.Location = new System.Drawing.Point(531, 13);
            this.loadButton.Name = "easeButton2";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 6;
            this.loadButton.Text = "浏览";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(94, 59);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(512, 21);
            this.textBox2.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "搜索";
            // 
            // progressBar1
            // 
            this.bar.Location = new System.Drawing.Point(25, 469);
            this.bar.Name = "progressBar1";
            this.bar.Size = new System.Drawing.Size(581, 23);
            this.bar.TabIndex = 9;
            this.bar.Minimum = 0;
            this.bar.Maximum = 100;
            this.bar.Value = 0;
            // 
            // tipLabel
            // 
            this.tipLabel.AutoSize = true;
            this.tipLabel.Location = new System.Drawing.Point(23, 439);
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Size = new System.Drawing.Size(0, 12);
            this.tipLabel.TabIndex = 10;


            var menu = fileList.ContextMenuStrip;
            menu.Items.Add(downloadItem);
            downloadItem.Text = "下载";
            // 
            // DownloadFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 509);
            this.Controls.Add(this.tipLabel);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.fileList);
            this.Name = "DownloadFileDialog";
            this.Text = "外服资源下载";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private UI.EaseButton searchButton;
        private ComboBox comboBox1;
        private Label label1;
        private TextBox pathBox;
        private UI.EaseButton loadButton;
        private TextBox textBox2;
        private Label label2;
        private CheckedListBox fileList;
        private ToolStripMenuItem Item;
        private ProgressBar bar;
        private Label tipLabel;
        private ToolStripMenuItem downloadItem;
    }
}