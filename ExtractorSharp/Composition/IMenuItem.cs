using System.Collections.Generic;

namespace ExtractorSharp.Composition {
    public interface IMenuItem {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 父菜单
        /// </summary>
        MenuItemType Parent { get; }
        /// <summary>
        /// 命令名
        /// 首字母大写:--通过控制器执行命令
        /// 首字母小写:--通过视图器调用窗口
        /// </summary>
        string Command { get; }
        /// <summary>
        /// 子菜单
        /// </summary>
        Dictionary<string, string> Children { get; }
    }
}
