using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Core.Composition {
    /// <summary>
    ///     菜单选项
    /// </summary>
    public interface IMenuItem {
        /// <summary>
        ///     名称
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     命令
        /// </summary>
        string Command { get; }

        /// <summary>
        ///     点击类型
        /// </summary>
        ClickType Click { get; }

        /// <summary>
        ///     按钮图片
        /// </summary>
        Image Image { get; }

        /// <summary>
        ///     快捷键
        /// </summary>
        Keys ShortcutKeys { get; }

        /// <summary>
        ///     根菜单
        /// </summary>
        MenuItemType Parent { get; }

        /// <summary>
        ///     子菜单
        /// </summary>
        List<IMenuItem> Children { get; }
    }
}