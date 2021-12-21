using System.Collections.Generic;

namespace ExtractorSharp.Composition.Menu {
    /// <summary>
    ///     菜单选项
    /// </summary>
    public interface IMenuItem {
        /// <summary>
        ///     关键字 
        ///     根据"/"分割生成菜单树
        ///     "_"开头表示展开的菜单
        ///     "[\d+]"结尾指定Order
        /// </summary>
        string Key { set; get; }

        /// <summary>
        /// 悬浮提示
        /// </summary>
        string ToolTip { set; get; }

        int Order { set; get; }

    }

    public interface IChildrenItem : IMenuItem {

        List<IMenuItem> Children { set; get; }

        bool IsTile { set; get; }
    }

}