using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View{
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
            imageBox = new PictureBox();
            addMergeButton = new Button();
            addListButton = new Button();
            saveButton = new Button();
            loadButton = new Button();
            otherButton = new Button();
            weaponCheck = new CheckBox();
            weaponBox = new NumericUpDown();
            weaponCombo = new ComboBox();

            professionBox = new ComboBox();
            professionLabel = new Label();

            hideCheck = new CheckBox();
            clearCheck = new CheckBox();
            maskCheck = new CheckBox();
            banCheck = new CheckBox();
            addSkinCheck = new CheckBox();
            addWeaponCheck = new CheckBox();
            pathBox = new TextBox();
            pathLabel = new Label();

            addListGroup = new GroupBox();
            addMergeGroup = new GroupBox();
            otherGroup = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)(imageBox)).BeginInit();
            SuspendLayout();


            // 
            // box
            // 
            imageBox.Location = new Point(377, 28);
            imageBox.Name = "box";
            imageBox.Size = new Size(130, 180);
            imageBox.TabIndex = 8;
            imageBox.TabStop = false;
            // 
            // addMergeButton
            // 
            addMergeButton.Location = new Point(134, 410);
            addMergeButton.Name = "addMergeButton";
            addMergeButton.Size = new Size(97, 23);
            addMergeButton.TabIndex = 9;
            addMergeButton.Text = Language["AddMerge"];
            addMergeButton.UseVisualStyleBackColor = true;
            // 
            // addListButton
            // 
            addListButton.Location = new Point(8, 410);
            addListButton.Name = "addListButton";
            addListButton.Size = new Size(97, 23);
            addListButton.TabIndex = 10;
            addListButton.Text = Language["AddList"];
            addListButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(377, 410);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(97, 23);
            saveButton.TabIndex = 11;
            saveButton.Text = Language["SaveAs"];
            saveButton.UseVisualStyleBackColor = true;
            // 
            // loadButton
            // 
            loadButton.Location = new Point(258, 410);
            loadButton.Name = "loadButton";
            loadButton.Size = new Size(97, 23);
            loadButton.TabIndex = 12;
            loadButton.Text = Language["LoadModel"];
            loadButton.UseVisualStyleBackColor = true;

            weaponCheck.AutoSize = true;
            weaponCheck.Location = new Point(199,231);
            weaponCheck.Text = Language["Weapon"];

            weaponCombo.Location = new Point(271, 226);
            weaponCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            otherButton.Location = new Point(300, 265);
            otherButton.Text = Language["Other"];
            otherButton.UseVisualStyleBackColor = true;

            weaponBox.AutoSize = true;
            weaponBox.Location = new Point(400,226);
            weaponBox.Minimum = -1;
            weaponBox.Maximum = 9999;
            // 
            // professionBox
            // 
            professionBox.FormattingEnabled = true;
            professionBox.Location = new Point(84, 7);
            professionBox.Name = "professionBox";
            professionBox.Size = new Size(97, 20);
            professionBox.TabIndex = 25;
            professionBox.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // label1
            // 
            professionLabel.AutoSize = true;
            professionLabel.Location = new Point(20, 10);
            professionLabel.Name = "professionLabel";
            professionLabel.Size = new Size(29, 12);
            professionLabel.TabIndex = 26;
            professionLabel.Text = Language["Profession"];
            // 
            // hideCheck
            // 
            hideCheck.AutoSize = true;
            hideCheck.Location = new Point(20,30);
            hideCheck.Name = "hideCheck";
            hideCheck.Text = Language["AddListHideImage"];
            hideCheck.Checked = true;
            hideCheck.UseVisualStyleBackColor = true;

            clearCheck.AutoSize = true;
            clearCheck.Location = new Point(20, 30);
            clearCheck.Size = new Size(156, 16);
            clearCheck.Text = Language["ClearMerge"];
            clearCheck.Checked = true;
            clearCheck.UseVisualStyleBackColor = true;

            banCheck.AutoSize = true;
            banCheck.Location = new Point(20,30);
            banCheck.Text = Language["BanImg"];
            banCheck.UseVisualStyleBackColor = true;

            maskCheck.AutoSize = true;
            maskCheck.Location = new Point(20,50);
            maskCheck.Text = Language["MaskImg"];
            maskCheck.UseVisualStyleBackColor = true;


            pathBox.Text =Path;
            pathBox.Location = new Point(100, 265);
            pathBox.Size = new Size(175, 20);

            pathLabel.Text = Language["GamePath"];
            pathLabel.Location = new Point(13, 265);
            pathLabel.Size = new Size(80, 15);

            addMergeGroup.Text = Language["AddMerge"];
            addMergeGroup.Location = new Point(175, 300);
            addMergeGroup.Width = 150;
            addMergeGroup.Controls.Add(clearCheck);
            addMergeGroup.Controls.Add(addWeaponCheck);

            addWeaponCheck.Text = Language["AddWeapon"];
            addWeaponCheck.Location = new Point(20, 50);

            addSkinCheck.Text = Language["AddSkin"];
            addSkinCheck.Location = new Point(20, 50);

            addListGroup.Text = Language["AddList"];
            addListGroup.Location = new Point(13, 300);
            addListGroup.Width = 150;
            addListGroup.Controls.Add(hideCheck);
            addListGroup.Controls.Add(addSkinCheck);

            otherGroup.Text = Language["Other"];
            otherGroup.Location = new Point(335,300);
            otherGroup.Width = 150;
            otherGroup.Controls.Add(banCheck);
            otherGroup.Controls.Add(maskCheck);
            // 
            // FitRoomDialog
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Size = new Size(560, 500);
            Controls.Add(addMergeGroup);
            Controls.Add(addListGroup);
            Controls.Add(otherGroup);
            Controls.Add(professionLabel);
            Controls.Add(professionBox);
            Controls.Add(weaponCheck);
            Controls.Add(loadButton);
            Controls.Add(saveButton);
            Controls.Add(otherButton);
            Controls.Add(addListButton);
            Controls.Add(addMergeButton);
            Controls.Add(imageBox);
            Controls.Add(weaponCombo);
            Controls.Add(weaponBox);
            Controls.Add(pathLabel);
            Controls.Add(pathBox);
            Name = "dressingRoom";
            Text = Language["DressingRoom"];
            ((System.ComponentModel.ISupportInitialize)(imageBox)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private NumericUpDown[] partBoxes;
        private CheckBox[] partCheckes;
        private NumericUpDown weaponBox;
        private ComboBox weaponCombo;
        private PictureBox imageBox;
        private Button addMergeButton;
        private Button addListButton;
        private Button saveButton;
        private Button otherButton;
        private Button loadButton;
        private CheckBox weaponCheck;
        private ComboBox professionBox;
        private Label professionLabel;
        private CheckBox hideCheck;
        private CheckBox clearCheck;
        private CheckBox maskCheck;
        private CheckBox banCheck;
        private CheckBox addSkinCheck;          
        private CheckBox addWeaponCheck;
        private Label pathLabel;
        private TextBox pathBox;

        private GroupBox addListGroup;
        private GroupBox addMergeGroup;
        private GroupBox otherGroup;
    }
}