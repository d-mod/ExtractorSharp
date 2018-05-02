using System.Windows.Forms;

namespace ExtractorSharp.Component {
    partial class ESListBox<T> {
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
            components = new System.ComponentModel.Container();
            ContextMenuStrip = new ContextMenuStrip();
            checkAllItem = new ToolStripMenuItem();
            reverseCheckItem = new ToolStripMenuItem();
            unCheckAllItem = new ToolStripMenuItem();
            deleteItem = new ToolStripMenuItem();
            clearItem = new ToolStripMenuItem();
            checkAllItem.Text = Language["CheckAll"];
            reverseCheckItem.Text = Language["ReverseCheck"];
            unCheckAllItem.Text = Language["UnCheckAll"];
            deleteItem.Text = Language["DeleteCheck"];
            clearItem.Text = Language["ClearList"];
            ContextMenuStrip.Items.Add(checkAllItem);
            ContextMenuStrip.Items.Add(reverseCheckItem);
            ContextMenuStrip.Items.Add(unCheckAllItem);
            CanClear = true;
            CanDelete = true;
            AllowDrop = true;
            ImeMode=ImeMode.Disable;
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
        }
        
        private ToolStripMenuItem checkAllItem;
        private ToolStripMenuItem reverseCheckItem;
        private ToolStripMenuItem unCheckAllItem;
        private ToolStripMenuItem deleteItem;
        private ToolStripMenuItem clearItem;

        #endregion
    }
}
