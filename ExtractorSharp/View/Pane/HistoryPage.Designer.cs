using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.View.Pane {
    partial class HistoryPage{


        public void InitializeComponent() {

            this.historyList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // list
            // 
            this.historyList.Location = new System.Drawing.Point(0, 0);
            this.historyList.Name = "list";
            this.historyList.Size = new System.Drawing.Size(190, 280);
            this.historyList.TabIndex = 0;

            Controls.Add(historyList);
            Text = Language.Default["History"];
        }
        private System.Windows.Forms.ListBox historyList;

    }
}
