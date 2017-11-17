namespace ExtractorSharp.View {
    partial class NewImageDialog {
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
            this.count_box = new System.Windows.Forms.NumericUpDown();
            this.count_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.index_box = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.link_radio = new System.Windows.Forms.RadioButton();
            this._1555_radio = new System.Windows.Forms.RadioButton();
            this._4444_radio = new System.Windows.Forms.RadioButton();
            this._8888_radio = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.count_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.index_box)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // count_box
            // 
            this.count_box.Location = new System.Drawing.Point(39, 58);
            this.count_box.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.count_box.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.count_box.Name = "count_box";
            this.count_box.Size = new System.Drawing.Size(200, 21);
            this.count_box.TabIndex = 0;
            this.count_box.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // count_Label
            // 
            this.count_Label.AutoSize = true;
            this.count_Label.Location = new System.Drawing.Point(37, 27);
            this.count_Label.Name = "count_Label";
            this.count_Label.Size = new System.Drawing.Size(53, 12);
            this.count_Label.TabIndex = 1;
            this.count_Label.Text = Language["ImageCount"];
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(39, 286);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 4;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(164, 286);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // index_box
            // 
            this.index_box.Location = new System.Drawing.Point(39, 124);
            this.index_box.Name = "index_box";
            this.index_box.Size = new System.Drawing.Size(200, 21);
            this.index_box.TabIndex = 6;
            this.index_box.Minimum = -1;
            this.index_box.Value = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = Language["Offset"];
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._8888_radio);
            this.groupBox1.Controls.Add(this._4444_radio);
            this.groupBox1.Controls.Add(this._1555_radio);
            this.groupBox1.Controls.Add(this.link_radio);
            this.groupBox1.Location = new System.Drawing.Point(39, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 116);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = Language["ColorBits"];
            // 
            // link_radio
            // 
            this.link_radio.AutoSize = true;
            this.link_radio.Checked = true;
            this.link_radio.Cursor = System.Windows.Forms.Cursors.Default;
            this.link_radio.Location = new System.Drawing.Point(7, 21);
            this.link_radio.Name = "link_radio";
            this.link_radio.Size = new System.Drawing.Size(83, 16);
            this.link_radio.TabIndex = 0;
            this.link_radio.TabStop = true;
            this.link_radio.Text = Language["Default"];
            this.link_radio.UseVisualStyleBackColor = true;
            // 
            // _1555_radio
            // 
            this._1555_radio.AutoSize = true;
            this._1555_radio.Location = new System.Drawing.Point(7, 44);
            this._1555_radio.Name = "_1555_radio";
            this._1555_radio.Size = new System.Drawing.Size(125, 16);
            this._1555_radio.TabIndex = 1;
            this._1555_radio.Text = "ARGB_1555";
            this._1555_radio.UseVisualStyleBackColor = true;
            // 
            // _4444_radio
            // 
            this._4444_radio.AutoSize = true;
            this._4444_radio.Location = new System.Drawing.Point(7, 66);
            this._4444_radio.Name = "_4444_radio";
            this._4444_radio.Size = new System.Drawing.Size(125, 16);
            this._4444_radio.TabIndex = 2;
            this._4444_radio.Text = "ARGB_4444";
            this._4444_radio.UseVisualStyleBackColor = true;
            // 
            // _8888_radio
            // 
            this._8888_radio.AutoSize = true;
            this._8888_radio.Location = new System.Drawing.Point(6, 88);
            this._8888_radio.Name = "_8888_radio";
            this._8888_radio.Size = new System.Drawing.Size(125, 16);
            this._8888_radio.TabIndex = 3;
            this._8888_radio.Text = "ARGB_8888";
            this._8888_radio.UseVisualStyleBackColor = true;
            // 
            // NewImageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 321);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.count_Label);
            this.Controls.Add(this.count_box);
            this.Controls.Add(this.index_box);
            this.Name = "NewImageDialog";
            this.Text = Language["NewImage"];
            ((System.ComponentModel.ISupportInitialize)(this.count_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.index_box)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown count_box;
        private System.Windows.Forms.NumericUpDown index_box;
        private System.Windows.Forms.Label count_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton link_radio;
        private System.Windows.Forms.RadioButton _1555_radio;
        private System.Windows.Forms.RadioButton _4444_radio;
        private System.Windows.Forms.RadioButton _8888_radio;
    }
}