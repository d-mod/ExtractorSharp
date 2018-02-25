using ExtractorSharp.Core.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ExtractorSharp {
    /// <summary>
    /// 工具函数集
    /// </summary>
    public static class Tools {     

        

        public static void InsertRange(this CheckedListBox.ObjectCollection collection, int index, object[] array) {
            var i = 0;
            while (i < array.Length) {
                collection.Insert(index++, array[i++]);
            } 
        }

        public static void AddSeparator(this ToolStripItemCollection items) {
            items.Add(new ToolStripSeparator());
        }
     


        /// <summary>
        /// 根据type创建一个新的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">实例类型</param>
        /// <param name="args">构造参数</param>
        /// <returns></returns>
        public static object CreateInstance(this Type type, params object[] args) => type.Assembly.CreateInstance(type.FullName, true, BindingFlags.Default, null, args, null, null);

        

        /// <summary>
        /// 读取文件列表
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Dictionary<string, string> LoadFileLst(string file) {
            var dic = new Dictionary<string, string>();
            if (File.Exists(file)) {
                var fs = new StreamReader(file);
                while (!fs.EndOfStream) {
                    var str = fs.ReadLine();
                    str = str.Replace("\"", "");
                    var dt = str.Split(" ");
                    if (dt.Length < 1)
                        continue;
                    if (dt[0].EndsWith(".NPK"))
                        dic.Add(dt[0].GetSuffix(), dt[1]);
                }
                fs.Close();
            }
            return dic;
        }
    }
}
