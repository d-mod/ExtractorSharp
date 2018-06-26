using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog {
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
            this.loadButton = new ExtractorSharp.Component.ESButton();
            this.fullPathCheck = new System.Windows.Forms.CheckBox();
            this.allImagesCheck = new System.Windows.Forms.CheckBox();
            this.tipsCheck = new System.Windows.Forms.CheckBox();
            this.yesButton = new ExtractorSharp.Component.ESButton();
            this.cancelButton = new ExtractorSharp.Component.ESButton();
            pathLabel = new Label();
            nameLabel = new Label();
            nameBox = new TextBox();
            this.SuspendLayout();

            pathLabel.Location = new System.Drawing.Point(100, 70);
            pathLabel.Text = Language["SaveImagePath"];
            

            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(200, 68);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(225, 21);
            this.pathBox.TabIndex = 0;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(430, 65);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 27);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = Language["Browse"];
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // tipsCheck
            // 
            this.tipsCheck.AutoSize = true;
            this.tipsCheck.Location = new System.Drawing.Point(100, 275);
            this.tipsCheck.Name = "tipsCheck";
            this.tipsCheck.Size = new System.Drawing.Size(108, 16);
            this.tipsCheck.TabIndex = 2;
            this.tipsCheck.Text = Language["NoTips"];
            this.tipsCheck.UseVisualStyleBackColor = true;
            //
            //
            //
            this.fullPathCheck.AutoSize = true;
            this.fullPathCheck.Location = new System.Drawing.Point(250,185);
            this.fullPathCheck.Text = Language["SavePathTips"];

            this.allImagesCheck.AutoSize = true;
            this.allImagesCheck.Location = new System.Drawing.Point(100, 185);
            this.allImagesCheck.Text = Language["AllImage"];


            nameLabel.Location = new System.Drawing.Point(100,140);
            nameLabel.Text = Language["AutoIncrement"];

            nameBox.Location = new System.Drawing.Point(200, 138);
            nameBox.Size = new System.Drawing.Size(200, 21);

            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(247, 275);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(362, 275);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // SaveImageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 330);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.fullPathCheck);
            this.Controls.Add(this.allImagesCheck);
            this.Controls.Add(this.tipsCheck);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameBox);
            this.Name = "SaveImageDialog";
            this.Text = Language["SaveImage"];
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pathBox;
        private Component.ESButton loadButton;
        private System.Windows.Forms.CheckBox tipsCheck;
        private System.Windows.Forms.CheckBox fullPathCheck;
        private System.Windows.Forms.CheckBox allImagesCheck;
        private Component.ESButton yesButton;
        private Component.ESButton cancelButton;
        private Label pathLabel;
        private Label nameLabel;
        private TextBox nameBox;
    }
}