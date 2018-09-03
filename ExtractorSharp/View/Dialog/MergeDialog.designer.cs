using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Properties;

namespace ExtractorSharp.View.Dialog{
        partial class MergeDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.list = new ListBox();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownItem = new ToolStripMenuItem();
            this.moveUpItem = new ToolStripMenuItem();
            this.sortButton = new ESButton();
            this.mergerButton = new ESButton();
            this.addFileButton = new ESButton();
            this.prograss = new System.Windows.Forms.ProgressBar();
            this.targetLabel = new Label();
            this.targetBox = new ComboBox();
            this.priviewPanel = new Panel();
            this.paletteLabel = new Label();
            this.palatteBox = new ComboBox();
            this.frameLabel = new Label();
            this.frameBox = new ComboBox();
            this.lastButton = new ESButton();
            this.nextButton = new ESButton();
            this.completedHideCheck = new CheckBox();
            this.autoSortCheck = new CheckBox();
            this.autoTrimCheck = new CheckBox();
            this.versionLabel = new Label();
            this.versionBox = new ComboBox();
            this.glowLayerLabel = new Label();
            this.glowLayerBox = new ComboBox();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // list
            // 
            this.list.AllowDrop = true;
            this.list.ContextMenuStrip = this.menu;
            this.list.FormattingEnabled = true;
            this.list.ItemHeight = 12;
            this.list.Location = new System.Drawing.Point(32, 32);
            this.list.Name = "list";
            this.list.ScrollAlwaysVisible = true;
            this.list.Size = new System.Drawing.Size(259, 256);
            this.list.TabIndex = 0;
            // 
            // menu
            // 
            this.menu.Items.Add(deleteItem);
            this.menu.Items.Add(clearItem);
            this.menu.Items.Add(new ToolStripSeparator());
            this.menu.Items.Add(moveUpItem);
            this.menu.Items.Add(moveDownItem);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(149, 70);
            // 
            // deleteItem
            // 
            this.deleteItem.Name = "deleteItem";
            this.deleteItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteItem.Size = new System.Drawing.Size(148, 22);
            this.deleteItem.Text = Language["DeleteCheck"];
            // 
            // clearItem
            // 
            this.clearItem.Name = "clearItem";
            this.clearItem.Size = new System.Drawing.Size(148, 22);
            this.clearItem.Text = Language["ClearList"];

            this.moveUpItem.Text = Language["MoveUp"];
            this.moveUpItem.ShowShortcutKeys = true;

            this.moveDownItem.Text = Language["MoveDown"];
            this.moveDownItem.ShowShortcutKeys = true;
            // 
            // sortButton
            // 
            this.sortButton.Location = new System.Drawing.Point(410, 420);
            this.sortButton.Name = "sortButton";
            this.sortButton.Size = new System.Drawing.Size(75, 23);
            this.sortButton.TabIndex = 1;
            this.sortButton.Text = Language["Sort"];
            this.sortButton.UseVisualStyleBackColor = true;
            // 
            // MergeButton
            // 
            this.mergerButton.Location = new System.Drawing.Point(505, 420);
            this.mergerButton.Name = "MergeButton";
            this.mergerButton.Size = new System.Drawing.Size(75, 23);
            this.mergerButton.TabIndex = 2;
            this.mergerButton.Text = Language["Merge"];
            this.mergerButton.UseVisualStyleBackColor = true;


            this.addFileButton.Location = new Point(315,420);
            this.addFileButton.Text = Language["AddFile"];
            this.addFileButton.Size = new System.Drawing.Size(75, 23);
            this.addFileButton.UseVisualStyleBackColor = true;

            this.targetBox.Location = new System.Drawing.Point(120, 300);
            this.targetBox.Size = new System.Drawing.Size(170,23);
            this.targetBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.targetBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            // 
            // prograss
            // 
            this.prograss.Location = new System.Drawing.Point(32, 294);
            this.prograss.Name = "prograss";
            this.prograss.Size = new System.Drawing.Size(259, 38);
            this.prograss.TabIndex = 3;
            this.prograss.Visible = false;
            // 
            // useOtherCheck
            // 
            this.targetLabel.AutoSize = true;
            this.targetLabel.Location = new System.Drawing.Point(32, 302);
            this.targetLabel.TabIndex = 4;
            this.targetLabel.Text = Language["TargetFile"];

            this.priviewPanel.Location = new System.Drawing.Point(315, 32);
            this.priviewPanel.BackColor = Color.Gray;
            this.priviewPanel.Size = new Size(259, 256);

            this.palatteBox.Location = new Point(380, 300);
            this.palatteBox.Width = 200;
            this.palatteBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.frameBox.Location = new Point(380, 340);
            this.frameBox.Width = 200;
            this.frameBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.frameLabel.Location = new Point(315, 340);
            this.frameLabel.Text = Language["FrameCount"];
            this.frameLabel.AutoSize = true;

            this.paletteLabel.Location = new Point(315,300);
            this.paletteLabel.Text = Language["Palette"];
            this.paletteLabel.AutoSize = true;
           

            this.lastButton.Location = new Point(580, 340);
            this.lastButton.Size = new Size(20, 20);
            this.lastButton.Image = Resources.last;

            this.nextButton.Location = new Point(600, 340);
            this.nextButton.Size = new Size(20, 20);
            this.nextButton.Image = Resources.next;

            this.completedHideCheck.Location = new Point(500, 380);
            this.completedHideCheck.Text = Language["CompletedHide"];
            this.completedHideCheck.AutoSize = true;
            this.completedHideCheck.Checked = Config["MergeCompletedHide"].Boolean;


            this.autoSortCheck.Location = new Point(315, 380);
            this.autoSortCheck.Text = Language["AutoSort"];
            this.autoSortCheck.AutoSize = true;
            this.autoSortCheck.Checked = Config["AutoSort"].Boolean;


            this.autoTrimCheck.Location = new Point(410, 380);
            this.autoTrimCheck.Text = Language["AutoTrim"];
            this.autoTrimCheck.AutoSize = true;
            this.autoTrimCheck.Checked = Config["AutoTrim"].Boolean;

            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new Point(32, 340);
            this.versionLabel.Text = Language["TargetVersion"];

            this.versionBox.Location = new System.Drawing.Point(120, 340);
            this.versionBox.Size = new System.Drawing.Size(170, 23);
            this.versionBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.glowLayerLabel.Location = new Point(32,380);
            this.glowLayerLabel.Text = Language["GlowLayer"];
            this.glowLayerLabel.AutoSize = true;

            this.glowLayerBox.Location = new Point(120,380);
            this.glowLayerBox.Size = new System.Drawing.Size(170, 23);
            this.glowLayerBox.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // MergeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(650, 500);
            this.Controls.Add(this.prograss);
            this.Controls.Add(this.mergerButton);
            this.Controls.Add(this.sortButton);
            this.Controls.Add(this.addFileButton);
            this.Controls.Add(this.list);
            this.Controls.Add(targetBox);
            this.Controls.Add(targetLabel);
            this.Controls.Add(priviewPanel);
            this.Controls.Add(paletteLabel);
            this.Controls.Add(palatteBox);
            this.Controls.Add(frameLabel);
            this.Controls.Add(frameBox);
            this.Controls.Add(lastButton);
            this.Controls.Add(nextButton);
            this.Controls.Add(autoSortCheck);
            this.Controls.Add(autoTrimCheck);
            this.Controls.Add(completedHideCheck);
            this.Controls.Add(versionLabel);
            this.Controls.Add(versionBox);
            this.Controls.Add(glowLayerLabel);
            this.Controls.Add(glowLayerBox);
            this.Name = "MergeDialog";
            this.Text = Language["Merge"];
            this.AllowDrop = true;
            this.menu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox list;
        private ComboBox targetBox;
        private Button sortButton;
        private Button mergerButton;
        private ProgressBar prograss;
        private ContextMenuStrip menu;
        private ToolStripMenuItem deleteItem;
        private ToolStripMenuItem clearItem;
        private ToolStripMenuItem moveDownItem;
        private ToolStripMenuItem moveUpItem;
        private Panel priviewPanel;
        private Label targetLabel;
        private ComboBox frameBox;
        private ComboBox palatteBox;
        private Label glowLayerLabel;
        private ComboBox glowLayerBox;
        private ESButton lastButton;
        private ESButton nextButton;
        private ESButton addFileButton;
        private CheckBox completedHideCheck;
        private CheckBox autoTrimCheck;
        private CheckBox autoSortCheck;
        private CheckBox disableRepeatCheck;
        private ComboBox addFileCommbox;
        private Label versionLabel;
        private ComboBox versionBox;
        private Label paletteLabel;
        private Label frameLabel;
    }
}