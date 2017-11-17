using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;

namespace ExtractorSharp.View.Pane {
    partial class ColorChartPanel {
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


        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();

            this.list = new ColorList();
            this.list.Location = new System.Drawing.Point(0, 0);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(190, 240);
            this.list.TabIndex = 0;

            combo = new ComboBox();
            combo.Location = new System.Drawing.Point(50, 250);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            Controls.Add(list);
            Controls.Add(combo);
            Text = Language["ColorChart"];

            ContextMenuStrip = new ContextMenuStrip();
            menu = new ContextMenu();
            list.ContextMenu = menu;

            changeColorItem = new MenuItem();
            changeColorItem.Text = Language["Change"];
            changeToCurrentItem = new MenuItem();
            changeToCurrentItem.Text = Language["ChangeToCurrentColor"];
            menu.MenuItems.Add(changeColorItem);
            menu.MenuItems.Add(changeToCurrentItem);
        }

        #endregion

        private ColorList list;
        private ComboBox combo;
        private MenuItem changeColorItem;
        private MenuItem changeToCurrentItem;
        private ContextMenu menu;
        #endregion
    }
}
