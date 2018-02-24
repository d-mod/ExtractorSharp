using ExtractorSharp.Component;
using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog {
    partial class MainDialog {
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
            this.fileList = new ExtractorSharp.Component.ESListBox<string>();
            this.serverList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.keywordBox = new System.Windows.Forms.TextBox();
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
            // comboBox1
            // 
            this.serverList.FormattingEnabled = true;
            this.serverList.Location = new System.Drawing.Point(120, 18);
            this.serverList.Name = "comboBox1";
            this.serverList.Size = new System.Drawing.Size(121, 20);
            this.serverList.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = Language["SelectServer"];
            // 
            // textBox2
            // 
            this.keywordBox.Location = new System.Drawing.Point(120, 59);
            this.keywordBox.Name = "textBox2";
            this.keywordBox.Size = new System.Drawing.Size(300, 21);
            this.keywordBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = Language["Search"];
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
            downloadItem.Text = Language["AddList"];
            // 
            // DownloadFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 509);
            this.Controls.Add(this.tipLabel);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.keywordBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverList);
            this.Controls.Add(this.fileList);
            this.Name = "fileDownload";
            this.Text = Language["FileDownload"];
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComboBox serverList;
        private Label label1;
        private TextBox keywordBox;
        private Label label2;
        private ESListBox<string> fileList;
        private ProgressBar bar;
        private Label tipLabel;
        private ToolStripMenuItem downloadItem;
    }
}