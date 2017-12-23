namespace ExtractorSharp.Plugin.Searcher{
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
            addItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchButton = new System.Windows.Forms.Button();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.allNameBox = new System.Windows.Forms.CheckBox();
            this.patternBox = new System.Windows.Forms.TextBox();
            this.paterrnLabel = new System.Windows.Forms.Label();
            this.useDicBox = new System.Windows.Forms.CheckBox();
            this.dispayModeLabel = new System.Windows.Forms.Label();
            this.displayModeBox = new System.Windows.Forms.ComboBox();
            this.ignoreModelBox = new System.Windows.Forms.CheckBox();
            this.resultList = new Component.EaseListBox<SearchResult>();
            this.addNPKItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(485, 87);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = Language["Search"];
            this.searchButton.UseVisualStyleBackColor = true;
            // 
            // bar
            // 
            this.bar.Location = new System.Drawing.Point(27, 143);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(533, 23);
            this.bar.TabIndex = 5;
            this.bar.Minimum = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = Language["GamePath"];
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(118, 39);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(296, 21);
            this.pathBox.TabIndex = 7;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(485, 42);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 8;
            this.browseButton.Text = Language["Browse"];
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // allNameBox
            // 
            this.allNameBox.AutoSize = true;
            this.allNameBox.Location = new System.Drawing.Point(27, 121);
            this.allNameBox.Name = "allNameBox";
            this.allNameBox.Size = new System.Drawing.Size(72, 16);
            this.allNameBox.TabIndex = 9;
            this.allNameBox.Text = Language["MatchAll"];
            this.allNameBox.UseVisualStyleBackColor = true;

            this.useDicBox.AutoSize = true;
            this.useDicBox.Location = new System.Drawing.Point(157, 121);
            this.useDicBox.Name = "useDicBox";
            this.useDicBox.Size = new System.Drawing.Size(72, 16);
            this.useDicBox.TabIndex = 9;
            this.useDicBox.Text = Language["UseDictionary"];
            this.useDicBox.Checked = true;
            this.useDicBox.UseVisualStyleBackColor = true;


            this.ignoreModelBox.AutoSize = true;
            this.ignoreModelBox.Location = new System.Drawing.Point(270, 121);
            this.ignoreModelBox.Name = "ignoreModelBox";
            this.ignoreModelBox.Size = new System.Drawing.Size(72, 16);
            this.ignoreModelBox.TabIndex = 9;
            this.ignoreModelBox.Text = Language["IgnoreModel"];
            this.ignoreModelBox.UseVisualStyleBackColor = true;

            this.dispayModeLabel.AutoSize = true;
            this.dispayModeLabel.Text = Language["DisplayMode"];
            this.dispayModeLabel.Location = new System.Drawing.Point(360, 122);

            //
            //
            //
            this.displayModeBox.Location = new System.Drawing.Point(420, 118);
            this.displayModeBox.Size = new System.Drawing.Size(72, 16);
            this.displayModeBox.TabIndex = 9;
            this.displayModeBox.Items.Add("Img");
            this.displayModeBox.Items.Add("NPK");
            this.displayModeBox.Items.Add(Language["SelectAll"]);
            this.displayModeBox.SelectedIndex = 0;
            this.displayModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // patternBox
            // 
            this.patternBox.Location = new System.Drawing.Point(118, 84);
            this.patternBox.Name = "patternBox";
            this.patternBox.Size = new System.Drawing.Size(296, 21);
            this.patternBox.TabIndex = 10;
            this.patternBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.patternBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            // 
            // paterrnLabel
            // 
            this.paterrnLabel.AutoSize = true;
            this.paterrnLabel.Location = new System.Drawing.Point(27, 92);
            this.paterrnLabel.Name = "paterrnLabel";
            this.paterrnLabel.Size = new System.Drawing.Size(41, 12);
            this.paterrnLabel.TabIndex = 11;
            this.paterrnLabel.Text = Language["Keyword"];
            // 
            // resultList
            // 
            this.resultList.FormattingEnabled = true;
            this.resultList.HorizontalScrollbar = true;
            this.resultList.Location = new System.Drawing.Point(27, 172);
            this.resultList.Name = "resultList";
            this.resultList.Size = new System.Drawing.Size(533, 132);
            this.resultList.TabIndex = 12;
            //
            //
            //
            //
            resultList.ContextMenuStrip.Items.Add(addItem);
            resultList.ContextMenuStrip.Items.Add(addNPKItem);

            addItem.Text = Language["AddList"];
            addNPKItem.Text = Language["AddNPK"];
            // 
            // SearchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 333);
            this.Controls.Add(this.resultList);
            this.Controls.Add(this.paterrnLabel);
            this.Controls.Add(this.patternBox);
            this.Controls.Add(this.allNameBox);
            this.Controls.Add(this.ignoreModelBox);
            this.Controls.Add(this.useDicBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.dispayModeLabel);
            this.Controls.Add(this.displayModeBox);
            this.Name = "searchModel";
            this.Text = Language["SearchModel"];
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.CheckBox allNameBox;
        private System.Windows.Forms.CheckBox useDicBox;
        private System.Windows.Forms.CheckBox ignoreModelBox;
        private System.Windows.Forms.Label dispayModeLabel;
        private System.Windows.Forms.ComboBox displayModeBox;

        private System.Windows.Forms.TextBox patternBox;
        private System.Windows.Forms.Label paterrnLabel;
        private Component.EaseListBox<SearchResult> resultList;
        private System.Windows.Forms.ToolStripMenuItem addItem;
        private System.Windows.Forms.ToolStripMenuItem addNPKItem;
    }
}