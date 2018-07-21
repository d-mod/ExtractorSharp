using System;
using System.Reflection;
using System.Windows.Forms;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     工具函数集
    /// </summary>
    public static class Tools {
        public static void InsertRange(this CheckedListBox.ObjectCollection collection, int index, object[] array) {
            var i = 0;
            while (i < array.Length) collection.Insert(index++, array[i++]);
        }

        public static void AddSeparator(this ToolStripItemCollection items) {
            items.Add(new ToolStripSeparator());
        }


        /// <summary>
        ///     根据type创建一个新的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">实例类型</param>
        /// <param name="args">构造参数</param>
        /// <returns></returns>
        public static object CreateInstance(this Type type, params object[] args) {
            return type.Assembly.CreateInstance(
                type.FullName ?? throw new InvalidOperationException(), true, BindingFlags.Default, null, args, null,
                null);
        }
    }
}