using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View{
    partial class EncryptDialog {
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
            nameBox = new TextBox();
            nameLabel = new Label();
            authorBox = new TextBox();
            authorLabel = new Label();
            remarkBox = new TextBox();
            pwdBox = new TextBox();
            pwdLabel = new Label();
            remarkLabel = new Label();
            checkBox = new CheckBox();
            submitButton = new Button();
            updatePicker = new DateTimePicker();
            expirePicker = new DateTimePicker();
            updateLabel = new Label();
            expireLabel = new Label();
            canExtractCheck = new CheckBox();
            canReadCheck = new CheckBox();
            SuspendLayout();
            // 
            // nameBox
            // 
            nameBox.Location = new Point(37, 34);
            nameBox.Name = "nameBox";
            nameBox.Size = new Size(315, 21);
            nameBox.TabIndex = 0;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(35, 9);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(29, 12);
            nameLabel.TabIndex = 18;
            nameLabel.Text = "模型名称";
            // 
            // authorBox
            // 
            authorBox.Location = new Point(37, 87);
            authorBox.Name = "authorBox";
            authorBox.Size = new Size(315, 21);
            authorBox.TabIndex = 1;
            // 
            // authorLabel
            // 
            authorLabel.AutoSize = true;
            authorLabel.Location = new Point(35, 72);
            authorLabel.Name = "authorLabel";
            authorLabel.Size = new Size(29, 12);
            authorLabel.TabIndex = 17;
            authorLabel.Text = "模型作者";
            // 
            // remarkBox
            // 
            remarkBox.Location = new Point(37, 335);
            remarkBox.Multiline = true;
            remarkBox.Name = "remarkBox";
            remarkBox.Size = new Size(315, 67);
            remarkBox.TabIndex = 5;
            // 
            // pwdBox
            // 
            pwdBox.Location = new Point(37, 140);
            pwdBox.Name = "pwdBox";
            pwdBox.PasswordChar = '*';
            pwdBox.Size = new Size(315, 21);
            pwdBox.TabIndex = 2;
            // 
            // pwdLabel
            // 
            pwdLabel.AutoSize = true;
            pwdLabel.Location = new Point(37, 122);
            pwdLabel.Name = "pwdLabel";
            pwdLabel.Size = new Size(29, 12);
            pwdLabel.TabIndex = 16;
            pwdLabel.Text = "密码";
            // 
            // remarkLabel
            // 
            remarkLabel.AutoSize = true;
            remarkLabel.Location = new Point(35, 309);
            remarkLabel.Name = "remarkLabel";
            remarkLabel.Size = new Size(29, 12);
            remarkLabel.TabIndex = 15;
            remarkLabel.Text = "备注";
            // 
            // checkBox
            // 
            checkBox.AutoSize = true;
            checkBox.Checked = true;
            checkBox.CheckState = CheckState.Checked;
            checkBox.Location = new Point(37, 450);
            checkBox.Name = "checkBox";
            checkBox.Size = new Size(168, 16);
            checkBox.TabIndex = 7;
            checkBox.Text = "确认此模型为原创";
            checkBox.UseVisualStyleBackColor = true;
            // 
            // submitButton
            // 
            submitButton.Location = new Point(277, 443);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(75, 23);
            submitButton.TabIndex = 6;
            submitButton.Text = "提交";
            submitButton.UseVisualStyleBackColor = true;
            // 
            // updatePicker
            // 
            updatePicker.Location = new Point(37, 201);
            updatePicker.Name = "updatePicker";
            updatePicker.Size = new Size(315, 21);
            updatePicker.TabIndex = 3;
            updatePicker.Format = DateTimePickerFormat.Long;
            // 
            // expirePicker
            // 
            expirePicker.Location = new Point(39, 276);
            expirePicker.Name = "expirePicker";
            expirePicker.Size = new Size(313, 21);
            expirePicker.TabIndex = 4;
            expirePicker.Value=DateTime.Now.AddYears(1);
            expirePicker.Format = DateTimePickerFormat.Long;
            // 
            // updateLabel
            // 
            updateLabel.AutoSize = true;
            updateLabel.Location = new Point(35, 179);
            updateLabel.Name = "updateLabel";
            updateLabel.Size = new Size(53, 12);
            updateLabel.TabIndex = 13;
            updateLabel.Text = "更新日期";
            // 
            // expireLabel
            // 
            expireLabel.AutoSize = true;
            expireLabel.Location = new Point(37, 248);
            expireLabel.Name = "expireLabel";
            expireLabel.Size = new Size(209, 12);
            expireLabel.TabIndex = 14;
            expireLabel.Text = "有效期至(早于更新日期则表示无限期)";
            // 
            // canExtractCheck
            // 
            canExtractCheck.AutoSize = true;
            canExtractCheck.Checked = true;
            canExtractCheck.CheckState = CheckState.Checked;
            canExtractCheck.Location = new Point(37, 419);
            canExtractCheck.Name = "canExtractCheck";
            canExtractCheck.Size = new Size(90, 16);
            canExtractCheck.TabIndex = 19;
            canExtractCheck.Text = "允许提取IMG";
            canExtractCheck.UseVisualStyleBackColor = true;
            // 
            // canReadCheck
            // 
            canReadCheck.AutoSize = true;
            canReadCheck.Location = new Point(151, 419);
            canReadCheck.Name = "canReadCheck";
            canReadCheck.Size = new Size(90, 16);
            canReadCheck.TabIndex = 20;
            canReadCheck.Text = "允许查看IMG";
            canReadCheck.UseVisualStyleBackColor = true;
            // 
            // EncryptDialog
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(389, 478);
            Controls.Add(canReadCheck);
            Controls.Add(canExtractCheck);
            Controls.Add(expireLabel);
            Controls.Add(updateLabel);
            Controls.Add(expirePicker);
            Controls.Add(updatePicker);
            Controls.Add(submitButton);
            Controls.Add(checkBox);
            Controls.Add(remarkLabel);
            Controls.Add(pwdLabel);
            Controls.Add(pwdBox);
            Controls.Add(remarkBox);
            Controls.Add(authorLabel);
            Controls.Add(authorBox);
            Controls.Add(nameLabel);
            Controls.Add(nameBox);
            Name = "EncryptDialog";
            Text = "密码保护";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private TextBox nameBox;
        private Label nameLabel;
        private TextBox authorBox;
        private Label authorLabel;
        private TextBox remarkBox;
        private TextBox pwdBox;
        private Label pwdLabel;
        private Label remarkLabel;
        private CheckBox checkBox;
        public Button submitButton;
        private DateTimePicker updatePicker;
        private DateTimePicker expirePicker;
        private Label updateLabel;
        private Label expireLabel;
        private CheckBox canExtractCheck;
        private CheckBox canReadCheck;
    }
}