using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog {
    partial class SettingDialog {
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
            this.tree = new TreeView();
            this.panel = new Panel();
            this.applyButton = new Component.EaseButton();
            this.yesButton = new ExtractorSharp.Component.EaseButton();
            this.resetButton = new Component.EaseButton();
            this.cancelButton = new ExtractorSharp.Component.EaseButton();
            this.SuspendLayout();
            // 
            // easeTabPanel1
            // 
            this.tree.Location = new Point(20, 20);
            this.tree.Size = new System.Drawing.Size(150, 380);
            this.tree.TabIndex = 0;
            //
            //
            //
            this.panel.Size = new System.Drawing.Size(400, 340);
            this.panel.BackColor = this.BackColor;
            this.panel.BorderStyle = BorderStyle.FixedSingle;
            this.panel.Location = new System.Drawing.Point(200,20);
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(410, 370);
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 2;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(515, 370);
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            //
            //
            //
            this.resetButton.Location = new System.Drawing.Point(305, 370);
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.Text = Language["Reset"];
            this.resetButton.UseVisualStyleBackColor = true;
            //
            //
            //
            this.applyButton.Location = new System.Drawing.Point(200, 370);
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.Text = Language["Apply"];
            this.applyButton.UseVisualStyleBackColor = true;
            // 
            // SettingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 420);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.panel);
            this.Controls.Add(resetButton);
            this.Controls.Add(applyButton);
            this.Controls.Add(yesButton);
            this.Controls.Add(cancelButton);
            this.Text = Language["Setting"];
            this.ControlBox = false;
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView tree;
        private Component.EaseButton applyButton;
        private Component.EaseButton yesButton;
        private Component.EaseButton cancelButton;
        private Component.EaseButton resetButton;
        private Panel panel;
    }
}