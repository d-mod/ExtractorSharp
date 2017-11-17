namespace ExtractorSharp.Loose.Test {
    partial class MainForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.rootNode = new System.Windows.Forms.TreeNode("$");
            this.tree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.插入数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textbox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.loadItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPropItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.ContextMenuStrip = this.contextMenuStrip1;
            this.tree.Location = new System.Drawing.Point(561, 29);
            this.tree.Name = "tree";
            rootNode.Name = "rootNode";
            rootNode.Text = "$";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            rootNode});
            this.tree.Size = new System.Drawing.Size(298, 329);
            this.tree.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.插入数据ToolStripMenuItem,
            this.修改数据ToolStripMenuItem,
            this.删除数据ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 70);
            // 
            // 插入数据ToolStripMenuItem
            // 
            this.插入数据ToolStripMenuItem.Name = "插入数据ToolStripMenuItem";
            this.插入数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.插入数据ToolStripMenuItem.Text = "插入数据";
            // 
            // 修改数据ToolStripMenuItem
            // 
            this.修改数据ToolStripMenuItem.Name = "修改数据ToolStripMenuItem";
            this.修改数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.修改数据ToolStripMenuItem.Text = "修改数据";
            // 
            // 删除数据ToolStripMenuItem
            // 
            this.删除数据ToolStripMenuItem.Name = "删除数据ToolStripMenuItem";
            this.删除数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除数据ToolStripMenuItem.Text = "删除数据";
            // 
            // textbox
            // 
            this.textbox.Location = new System.Drawing.Point(30, 28);
            this.textbox.Name = "textbox";
            this.textbox.Size = new System.Drawing.Size(503, 330);
            this.textbox.TabIndex = 4;
            this.textbox.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(906, 25);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadItem,
            this.loadPropItem,
            this.saveAsItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(44, 21);
            this.fileMenu.Text = "文件";
            // 
            // loadItem
            // 
            this.loadItem.Name = "loadItem";
            this.loadItem.Size = new System.Drawing.Size(161, 22);
            this.loadItem.Text = "载入";
            // 
            // loadPropItem
            // 
            this.loadPropItem.Name = "loadPropItem";
            this.loadPropItem.Size = new System.Drawing.Size(161, 22);
            this.loadPropItem.Text = "载入properties";
            // 
            // saveAsItem
            // 
            this.saveAsItem.Name = "saveAsItem";
            this.saveAsItem.Size = new System.Drawing.Size(161, 22);
            this.saveAsItem.Text = "另存为";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 524);
            this.Controls.Add(this.textbox);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TreeNode rootNode;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.RichTextBox textbox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem loadItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsItem;
        private System.Windows.Forms.ToolStripMenuItem loadPropItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 插入数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除数据ToolStripMenuItem;
    }
}

