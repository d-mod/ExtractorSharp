using ExtractorSharp.Data;

namespace ExtractorSharp.View.Dialog {
    partial class SplitFileDialog {
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.yesButton = new ExtractorSharp.Component.EaseButton();
            this.noButton = new ExtractorSharp.Component.EaseButton();
            this.check = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(44, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(316, 232);
            this.listBox1.TabIndex = 0;
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(202, 287);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(77, 23);
            this.yesButton.TabIndex = 1;
            this.yesButton.Text = Language["Yes"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // noButton
            // 
            this.noButton.Location = new System.Drawing.Point(285, 287);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(75, 23);
            this.noButton.TabIndex = 2;
            this.noButton.Text = Language["Cancel"];
            this.noButton.UseVisualStyleBackColor = true;
            // 
            // check
            // 
            this.check.AutoSize = true;
            this.check.Location = new System.Drawing.Point(44, 287);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(78, 16);
            this.check.TabIndex = 3;
            this.check.Text = Language["SelectAllAndNotTips"];
            this.check.UseVisualStyleBackColor = true;
            // 
            // SplitFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 338);
            this.Controls.Add(this.check);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.listBox1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private Component.EaseButton yesButton;
        private Component.EaseButton noButton;
        private System.Windows.Forms.CheckBox check;
    }
}