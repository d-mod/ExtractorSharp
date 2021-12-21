﻿using ExtractorSharp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.View.Pane {
    partial class HistoryPage {


        public void InitializeComponent() {

            this.historyList = new System.Windows.Forms.ListBox();
            this.menu = new System.Windows.Forms.ContextMenuStrip();
            this.clearItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SuspendLayout();
            // 
            // list
            // 
            this.historyList.Location = new System.Drawing.Point(0, 0);
            this.historyList.Name = "list";
            this.historyList.Size = new System.Drawing.Size(290, 270);
            this.historyList.TabIndex = 0;

            this.historyList.ContextMenuStrip = menu;

            gotoItem.Text = Language["Goto"];
            addItem.Text = Language["AddAction"];
            clearItem.Text = Language["ClearList"];

            menu.Items.Add(clearItem);
            menu.Items.Add(gotoItem);
            menu.Items.Add(addItem);
            Controls.Add(historyList);
            Text = Language["History"];
        }
        private System.Windows.Forms.ListBox historyList;

        private System.Windows.Forms.ContextMenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem clearItem;
        private System.Windows.Forms.ToolStripMenuItem gotoItem;
        private System.Windows.Forms.ToolStripMenuItem addItem;

    }
}
