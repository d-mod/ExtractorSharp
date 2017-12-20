using System.ComponentModel;
using System.Windows.Forms;

namespace ExtractorSharp.View{
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
            this.list = new System.Windows.Forms.ListBox();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownItem = new ToolStripMenuItem();
            this.moveUpItem = new ToolStripMenuItem();
            this.sortButton = new System.Windows.Forms.Button();
            this.MergeButton = new System.Windows.Forms.Button();
            this.prograss = new System.Windows.Forms.ProgressBar();
            this.useOtherCheck = new System.Windows.Forms.CheckBox();
            albumList = new ComboBox();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // list
            // 
            this.list.AllowDrop = true;
            this.list.ContextMenuStrip = this.menu;
            this.list.FormattingEnabled = true;
            this.list.ItemHeight = 12;
            this.list.Location = new System.Drawing.Point(32, 12);
            this.list.Name = "list";
            this.list.ScrollAlwaysVisible = true;
            this.list.Size = new System.Drawing.Size(259, 256);
            this.list.TabIndex = 0;
            // 
            // menu
            // 
            this.menu.Items.Add(deleteItem);
            this.menu.Items.Add(clearItem);
            this.menu.Items.Add(addOutItem);
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
            // addOutItem
            // 
            this.addOutItem.Name = "addOutItem";
            this.addOutItem.Size = new System.Drawing.Size(148, 22);
            this.addOutItem.Text = Language["AddOutsideMerge"];
            // 
            // sortButton
            // 
            this.sortButton.Location = new System.Drawing.Point(32, 318);
            this.sortButton.Name = "sortButton";
            this.sortButton.Size = new System.Drawing.Size(100, 23);
            this.sortButton.TabIndex = 1;
            this.sortButton.Text = Language["Sort"];
            this.sortButton.UseVisualStyleBackColor = true;
            // 
            // MergeButton
            // 
            this.MergeButton.Location = new System.Drawing.Point(216, 318);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(100, 23);
            this.MergeButton.TabIndex = 2;
            this.MergeButton.Text = Language["Merge"];
            this.MergeButton.UseVisualStyleBackColor = true;

            this.albumList.Location = new System.Drawing.Point(160, 275);
            this.albumList.Size = new System.Drawing.Size(120,23);
            this.albumList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.albumList.AutoCompleteSource = AutoCompleteSource.ListItems;
            // 
            // prograss
            // 
            this.prograss.Location = new System.Drawing.Point(32, 274);
            this.prograss.Name = "prograss";
            this.prograss.Size = new System.Drawing.Size(259, 38);
            this.prograss.TabIndex = 3;
            this.prograss.Visible = false;
            // 
            // useOtherCheck
            // 
            this.useOtherCheck.AutoSize = true;
            this.useOtherCheck.Location = new System.Drawing.Point(32, 280);
            this.useOtherCheck.Name = "useOtherCheck";
            this.useOtherCheck.Size = new System.Drawing.Size(120, 16);
            this.useOtherCheck.TabIndex = 4;
            this.useOtherCheck.Text = Language["UseOutsideRule"];
            this.useOtherCheck.UseVisualStyleBackColor = true;
            // 
            // MergeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(324, 358);
            this.Controls.Add(this.prograss);
            this.Controls.Add(this.useOtherCheck);
            this.Controls.Add(this.MergeButton);
            this.Controls.Add(this.sortButton);
            this.Controls.Add(this.list);
            this.Controls.Add(albumList);
            this.Name = "MergeDialog";
            this.Text = Language["Merge"];
            this.menu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox list;
        private ComboBox albumList;
        private Button sortButton;
        private Button MergeButton;
        private ProgressBar prograss;
        private ContextMenuStrip menu;
        private ToolStripMenuItem deleteItem;
        private ToolStripMenuItem clearItem;
        private ToolStripMenuItem addOutItem;
        private ToolStripMenuItem moveDownItem;
        private ToolStripMenuItem moveUpItem;
        private CheckBox useOtherCheck;
    }
}