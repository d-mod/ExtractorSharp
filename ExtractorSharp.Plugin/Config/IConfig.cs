using System.Collections.Generic;
using System.IO;

namespace ExtractorSharp.Config {
    public interface IConfig : IEnumerable<KeyValuePair<string, ConfigValue>>{
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ConfigValue this[string key] { set;get; }
        /// <summary>
        /// 载入
        /// </summary>
        /// <param name="filename"></param>
        void Load(string filename);
        /// <summary>
        /// 从流载入
        /// </summary>
        /// <param name="stream"></param>
        void Load(Stream stream);
        /// <summary>
        /// 从文本载入
        /// </summary>
        /// <param name="text"></param>
        void LoadConfig(string text);
        /// <summary>
        /// 保存
        /// </summary>
        void Save();
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="filename"></param>
        void Export(string filename);
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="config"></param>
        void Import(IConfig config);
        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
        /// <summary>
        /// 重载
        /// </summary>
        void Reload();
        
    }
}
