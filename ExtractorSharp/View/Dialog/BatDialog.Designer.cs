namespace ExtractorSharp.View {
    partial class BatDialog {
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
            this.panel1 = new System.Windows.Forms.GroupBox();
            this.allImageRadio = new System.Windows.Forms.RadioButton();
            this.selectImageRadio = new System.Windows.Forms.RadioButton();
            this.allAlbumRadio = new System.Windows.Forms.RadioButton();
            this.selectAlbumRadio = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.allImageRadio);
            this.panel1.Controls.Add(this.selectImageRadio);
            this.panel1.Controls.Add(this.allAlbumRadio);
            this.panel1.Controls.Add(this.selectAlbumRadio);
            this.panel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(22, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 115);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = false;
            this.panel1.Text = Language["HandleObject"];
            // 
            // allImageRadio
            // 
            this.allImageRadio.AutoSize = true;
            this.allImageRadio.Location = new System.Drawing.Point(23, 86);
            this.allImageRadio.Name = "allImageRadio";
            this.allImageRadio.Size = new System.Drawing.Size(71, 16);
            this.allImageRadio.TabIndex = 3;
            this.allImageRadio.TabStop = true;
            this.allImageRadio.Text = Language["AllImage"];
            this.allImageRadio.UseVisualStyleBackColor = true;
            // 
            // selectImageRadio
            // 
            this.selectImageRadio.AutoSize = true;
            this.selectImageRadio.Location = new System.Drawing.Point(23, 63);
            this.selectImageRadio.Name = "selectImageRadio";
            this.selectImageRadio.Size = new System.Drawing.Size(107, 16);
            this.selectImageRadio.TabIndex = 2;
            this.selectImageRadio.TabStop = true;
            this.selectImageRadio.Text =  Language["CheckImage"];
            this.selectImageRadio.UseVisualStyleBackColor = true;
            // 
            // allAlbumRadio
            // 
            this.allAlbumRadio.AutoSize = true;
            this.allAlbumRadio.Location = new System.Drawing.Point(23, 40);
            this.allAlbumRadio.Name = "allAlbumRadio";
            this.allAlbumRadio.Size = new System.Drawing.Size(65, 16);
            this.allAlbumRadio.TabIndex = 1;
            this.allAlbumRadio.TabStop = true;
            this.allAlbumRadio.Text = Language["AllImg"];
            this.allAlbumRadio.UseVisualStyleBackColor = true;
            // 
            // selectAlbumRadio
            // 
            this.selectAlbumRadio.AutoSize = true;
            this.selectAlbumRadio.Location = new System.Drawing.Point(23, 17);
            this.selectAlbumRadio.Name = "selectAlbumRadio";
            this.selectAlbumRadio.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.selectAlbumRadio.Size = new System.Drawing.Size(101, 16);
            this.selectAlbumRadio.TabIndex = 0;
            this.selectAlbumRadio.TabStop = true;
            this.selectAlbumRadio.Text = Language["CheckImg"];
            this.selectAlbumRadio.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(287, 29);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(200,33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = Language["HandleContent"];
            // 
            // bar
            // 
            this.bar.Location = new System.Drawing.Point(20, 151);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(386, 35);
            this.bar.TabIndex = 3;
            this.bar.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(287, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 34);
            this.button1.TabIndex = 4;
            this.button1.Text = Language["Run"];
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(20, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(317, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = Language["FunctionDeprecated"];
            // 
            // BatDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 198);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "BatDialog";
            this.Text = Language["Batch"];
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panel1;
        private System.Windows.Forms.RadioButton selectAlbumRadio;
        private System.Windows.Forms.RadioButton allAlbumRadio;
        private System.Windows.Forms.RadioButton selectImageRadio;
        private System.Windows.Forms.RadioButton allImageRadio;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
    }
}