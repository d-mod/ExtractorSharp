namespace ExtractorSharp.View {
    partial class SaveImageDialog {
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
            this.pathBox = new System.Windows.Forms.TextBox();
            this.loadButton = new ExtractorSharp.UI.EaseButton();
            this.allPathCheck = new System.Windows.Forms.CheckBox();
            this.tipsCheck = new System.Windows.Forms.CheckBox();
            this.yesButton = new ExtractorSharp.UI.EaseButton();
            this.cancelButton = new ExtractorSharp.UI.EaseButton();
            this.SuspendLayout();
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(25, 26);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(192, 21);
            this.pathBox.TabIndex = 0;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(239, 23);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(68, 24);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = Language["Browse"];
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // tipsCheck
            // 
            this.tipsCheck.AutoSize = true;
            this.tipsCheck.Location = new System.Drawing.Point(25, 58);
            this.tipsCheck.Name = "tipsCheck";
            this.tipsCheck.Size = new System.Drawing.Size(108, 16);
            this.tipsCheck.TabIndex = 2;
            this.tipsCheck.Text = Language["NoTips"];
            this.tipsCheck.UseVisualStyleBackColor = true;
            //
            //
            //
            this.allPathCheck.AutoSize = true;
            this.allPathCheck.Location = new System.Drawing.Point(25,80);
            this.allPathCheck.Text = Language["SavePathTips"];
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(149, 70);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(68, 20);
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(239, 70);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(67, 20);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // SaveImageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 100);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.allPathCheck);
            this.Controls.Add(this.tipsCheck);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.pathBox);
            this.Name = "SaveImageDialog";
            this.Text = Language["SaveImage"];
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pathBox;
        private UI.EaseButton loadButton;
        private System.Windows.Forms.CheckBox tipsCheck;
        private System.Windows.Forms.CheckBox allPathCheck;
        private UI.EaseButton yesButton;
        private UI.EaseButton cancelButton;
    }
}