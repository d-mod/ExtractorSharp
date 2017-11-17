using ExtractorSharp.View.Pane;
using System.Windows.Forms;

namespace ExtractorSharp.View {
    partial class DropPanel {
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {

            // 
            // HistoryPanel
            //
            historyPanel = new HistoryPage();
            actionPanel = new ActionPage();
            
            this.Name = "DropPanel";
            this.Size = new System.Drawing.Size(200, 300);
            this.ResumeLayout(false);
            this.Visible = false;
            this.Location = new System.Drawing.Point(830,80);

        }

        private TabPage historyPanel, actionPanel;

        #endregion

    }
}
