using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Data;

namespace ExtractorSharp.View.Pane {
    partial class ActionPage {
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
            this.recordButton = new Button();
            this.pauseButton = new Button();
            this.runButton = new Button();
            deleteButton = new Button();

            this.actionList = new Component.ESListBox<ActionItem>();
            this.actionList.Location = new System.Drawing.Point(0, 0);
            this.actionList.Name = "list";
            this.actionList.Size = new System.Drawing.Size(190, 240);
            this.actionList.TabIndex = 0;

            recordButton.Location = new System.Drawing.Point(0, 240);
            recordButton.UseVisualStyleBackColor = true;  
            recordButton.Image = Properties.Resources.record;
            recordButton.Size = new System.Drawing.Size(24, 24);

            pauseButton.Location = new System.Drawing.Point(36, 240);
            pauseButton.UseVisualStyleBackColor = true;
            pauseButton.Image = Properties.Resources.pause;
            pauseButton.Size = new System.Drawing.Size(24, 24);

            runButton.Location = new System.Drawing.Point(72, 240);
            runButton.UseVisualStyleBackColor = true;
            runButton.Image = Properties.Resources.run;
            runButton.Size = new System.Drawing.Size(24, 24);

            deleteButton.Location = new System.Drawing.Point(108, 240);
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Image = Properties.Resources.delete;
            deleteButton.Size = new System.Drawing.Size(24, 24);

            Controls.Add(actionList);
            Controls.Add(recordButton);
            Controls.Add(pauseButton);
            Controls.Add(runButton);
            Controls.Add(deleteButton);
            Text = Language["Action"];
            
        }

        #endregion

        private Component.ESListBox<ActionItem> actionList;
        private Button recordButton;
        private Button pauseButton;
        private Button deleteButton;
        private Button runButton;
    }
}
