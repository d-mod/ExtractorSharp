using System.Windows.Forms;

namespace ExtractorSharp.View {
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
            this.modeLabel = new System.Windows.Forms.Label();
            this.modeBox = new System.Windows.Forms.ComboBox();
            this.pathLabel = new System.Windows.Forms.Label();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.list = new ExtractorSharp.Component.ESListBox<string>();
            this.cleanButton = new System.Windows.Forms.Button();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.searhButton = new System.Windows.Forms.Button();
            this.backupCheck = new System.Windows.Forms.CheckBox();
            this.loadButton = new ExtractorSharp.Component.ESButton();
            this.SuspendLayout();
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Location = new System.Drawing.Point(26, 27);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(53, 12);
            this.modeLabel.TabIndex = 0;
            this.modeLabel.Text = Language["CleanMode"];
            // 
            // modeBox
            // 
            this.modeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeBox.FormattingEnabled = true;
            this.modeBox.Items.AddRange(new object[] {
            Language["SimpleClean"],
            Language["DepthClean"]});
            this.modeBox.Location = new System.Drawing.Point(116, 24);
            this.modeBox.Name = "modeBox";
            this.modeBox.Size = new System.Drawing.Size(212, 20);
            this.modeBox.TabIndex = 1;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(26, 59);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(53, 12);
            this.pathLabel.TabIndex = 2;
            this.pathLabel.Text = Language["GamePath"];
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(116, 56);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(212, 21);
            this.pathBox.TabIndex = 3;
            // 
            // list
            // 
            this.list.FormattingEnabled = true;
            this.list.HorizontalScrollbar = true;
            this.list.Location = new System.Drawing.Point(28, 89);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(400, 148);
            this.list.TabIndex = 4;
            // 
            // clearButton
            // 
            this.cleanButton.Location = new System.Drawing.Point(214, 257);
            this.cleanButton.Name = "cleanButton";
            this.cleanButton.Size = new System.Drawing.Size(129, 36);
            this.cleanButton.TabIndex = 5;
            this.cleanButton.Text = Language["Clean"];
            this.cleanButton.UseVisualStyleBackColor = true;
            // 
            // bar
            // 
            this.bar.Location = new System.Drawing.Point(28, 255);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(400, 38);
            this.bar.TabIndex = 6;
            this.bar.Visible = false;
            // 
            // searhButton
            // 
            this.searhButton.Location = new System.Drawing.Point(28, 257);
            this.searhButton.Name = "searhButton";
            this.searhButton.Size = new System.Drawing.Size(139, 36);
            this.searhButton.TabIndex = 7;
            this.searhButton.Text = Language["Search"];
            this.searhButton.UseVisualStyleBackColor = true;
            // 
            // backupCheck
            // 
            this.backupCheck.AutoSize = true;
            this.backupCheck.Location = new System.Drawing.Point(356, 250);
            this.backupCheck.Name = "backupCheck";
            this.backupCheck.Size = new System.Drawing.Size(72, 16);
            this.backupCheck.TabIndex = 8;
            this.backupCheck.Text = Language["Backup"];
            this.backupCheck.UseVisualStyleBackColor = true;
            // 
            // easeButton1
            // 
            this.loadButton.Location = new System.Drawing.Point(353, 54);
            this.loadButton.Name = "easeButton1";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 9;
            this.loadButton.Text = Language["Browse"];
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // ClearDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 309);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.cleanButton);
            this.Controls.Add(this.list);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.modeBox);
            this.Controls.Add(this.modeLabel);
            this.Controls.Add(this.searhButton);
            this.Controls.Add(this.backupCheck);
            this.Name = "cleanMod";
            this.Text = Language["CleanMod"];
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.ComboBox modeBox;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Button cleanButton;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Button searhButton;
        private System.Windows.Forms.CheckBox backupCheck;
        private Component.ESListBox<string> list;
        private Component.ESButton loadButton;
    }
}