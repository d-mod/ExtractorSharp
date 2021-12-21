using System;

namespace ExtractorSharp.Composition.Menu {

    /// <summary>
    /// 只有打开一个窗口功能的菜单项
    /// </summary>
    public abstract class ViewItem : InjectService, IRouteItem {

        public abstract string Key { get; set; }


        public abstract int Order { set; get; }

        public abstract string Command { set; get; }

        public virtual string ShortCutKey { set; get; }

        public virtual string ToolTip { set; get; }

        public virtual bool CanExecute() {
            return true;
        }

        public virtual void Execute(object sender, EventArgs e) {
            this.Viewer.Show(this.Command);
        }
    }
}
