namespace ExtractorSharp.View {
    partial class BugDialog {
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
            this.box = new System.Windows.Forms.TextBox();
            this.submitCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contactLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // box
            // 
            this.box.Location = new System.Drawing.Point(33, 51);
            this.box.Multiline = true;
            this.box.Name = "box";
            this.box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.box.Size = new System.Drawing.Size(226, 153);
            this.box.TabIndex = 0;
            // 
            // submitCheck
            // 
            this.submitCheck.AutoSize = true;
            this.submitCheck.Location = new System.Drawing.Point(33, 276);
            this.submitCheck.Name = "submitCheck";
            this.submitCheck.Size = new System.Drawing.Size(96, 16);
            this.submitCheck.TabIndex = 1;
            this.submitCheck.Text = Language["SubmitBugLog"];
            this.submitCheck.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(143, 272);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(56, 23);
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(206, 272);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(53, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(98, 230);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(161, 21);
            this.textBox2.TabIndex = 5;
            // 
            // contactLabel
            // 
            this.contactLabel.AutoSize = true;
            this.contactLabel.Location = new System.Drawing.Point(31, 233);
            this.contactLabel.Name = "contactLabel";
            this.contactLabel.Size = new System.Drawing.Size(53, 12);
            this.contactLabel.TabIndex = 6;
            this.contactLabel.Text = Language["Contact"];
            // 
            // BugDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 307);
            this.Controls.Add(this.contactLabel);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.submitCheck);
            this.Controls.Add(this.box);
            this.Name = "BugDialog";
            this.Text = Language["FeedBack"];
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox box;
        private System.Windows.Forms.CheckBox submitCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label contactLabel;
    }
}