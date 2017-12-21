using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition {
    public interface IMenuItem {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 命令
        /// </summary>
        string Command { get; }
        /// <summary>
        /// 根菜单
        /// </summary>
        MenuItemType Parent { get; }
        /// <summary>
        /// 子菜单
        /// </summary>
        List<IMenuItem> Childrens { get; }
    }
}
