using ExtractorSharp.Component;

namespace ExtractorSharp.View.Dialog {
    partial class ConvertDialog {
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
            this.combo = new System.Windows.Forms.ComboBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.yesButton = new ESButton();
            this.cancelButton = new ESButton();
            this.SuspendLayout();
            // 
            // combo
            // 
            this.combo.FormattingEnabled = true;
            this.combo.Location = new System.Drawing.Point(33, 54);
            this.combo.Name = "combo";
            this.combo.Size = new System.Drawing.Size(216, 20);
            this.combo.TabIndex = 0;
            this.combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(33, 54);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(216, 23);
            this.progress.TabIndex = 1;
            this.progress.Visible = false;
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(33, 117);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 2;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(174, 117);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            this.CancelButton = cancelButton;
            // 
            // ConvertDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 166);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.combo);
            this.Name = "ConvertDialog";
            this.Text = Language["ConvertVersion"];
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox combo;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button cancelButton;
    }
}