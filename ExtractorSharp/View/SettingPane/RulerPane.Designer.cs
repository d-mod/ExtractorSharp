namespace ExtractorSharp.View.SettingPane {
    partial class RulerPane {
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
            this.displaySpanBox = new System.Windows.Forms.CheckBox();
            this.displayCrosshairBox = new System.Windows.Forms.CheckBox();
///
    //        displaySpanBox.Text = Language["DisplayRulerSpan"];
            displaySpanBox.AutoSize = true;
            displaySpanBox.Location = new System.Drawing.Point(20, 40);

       //     displayCrosshairBox.Text= Language["DisplayRulerCrosshair"];
            displayCrosshairBox.AutoSize = true;
            displayCrosshairBox.Location = new System.Drawing.Point(20, 100);

            this.Controls.Add(displaySpanBox);
            this.Controls.Add(displayCrosshairBox);
     //       this.Parent = "View";
            this.Name = "Ruler";
        }


        
        private System.Windows.Forms.CheckBox displaySpanBox;
        private System.Windows.Forms.CheckBox displayCrosshairBox;


        #endregion
    }
}
