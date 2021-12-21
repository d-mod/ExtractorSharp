using ExtractorSharp.Components;

namespace ExtractorSharp.View.Dialog {
    partial class NewFileDialog {
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
            this.pathLabel = new System.Windows.Forms.Label();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.countLabel = new System.Windows.Forms.Label();
            this.indexLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.countBox = new System.Windows.Forms.NumericUpDown();
            this.indexBox = new System.Windows.Forms.ComboBox();
            this.versionBox = new System.Windows.Forms.ComboBox();
            this.yesButton = new ExtractorSharp.Components.ESButton();
            this.cancelButton = new ExtractorSharp.Components.ESButton();
            ((System.ComponentModel.ISupportInitialize)(this.countBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(38, 50);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(0, 12);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = Language["FilePath"];
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(40, 80);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(320, 21);
            this.pathBox.TabIndex = 1;
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(38, 126);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(0, 12);
            this.countLabel.TabIndex = 2;
            this.countLabel.Text = Language["ImageCount"];
            // 
            // countBox
            // 
            this.countBox.Location = new System.Drawing.Point(40, 156);
            this.countBox.Maximum = 999;
            this.countBox.Name = "countBox";
            this.countBox.Size = new System.Drawing.Size(320, 21);
            this.countBox.TabIndex = 6;
            // 
            // indexLabel
            // 
            this.indexLabel.AutoSize = true;
            this.indexLabel.Location = new System.Drawing.Point(38, 212);
            this.indexLabel.Name = "indexLabel";
            this.indexLabel.Size = new System.Drawing.Size(0, 12);
            this.indexLabel.TabIndex = 3;
            this.indexLabel.Text = Language["Offset"];
            // 
            // indexBox
            // 
            this.indexBox.Location = new System.Drawing.Point(40, 242);
            this.indexBox.Name = "indexBox";
            this.indexBox.Size = new System.Drawing.Size(320, 21);
            this.indexBox.TabIndex = 7;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(38, 298);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 12);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = Language["FileVersion"];

            // 
            // versionBox
            // 
            this.versionBox.Location = new System.Drawing.Point(40, 328);
            this.versionBox.Name = "versionBox";
            this.versionBox.Size = new System.Drawing.Size(320, 21);
            this.versionBox.TabIndex = 7;
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(180, 385);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 25);
            this.yesButton.TabIndex = 8;
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Text = Language["OK"];
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(280, 385);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Text = Language["Cancel"];
            //  
            // NewFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(400, 440);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.indexBox);
            this.Controls.Add(this.countBox);
            this.Controls.Add(this.indexLabel);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.versionBox);
            this.Name = "NewFileDialog";
            ((System.ComponentModel.ISupportInitialize)(this.countBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label indexLabel;
        private System.Windows.Forms.NumericUpDown countBox;
        private System.Windows.Forms.ComboBox indexBox;
        private System.Windows.Forms.ComboBox versionBox;
        private System.Windows.Forms.Label versionLabel;
        private ESButton yesButton;
        private ESButton cancelButton;
    }
}