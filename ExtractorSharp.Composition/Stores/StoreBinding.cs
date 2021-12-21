using System;

namespace ExtractorSharp.Composition.Stores {

    /// <summary>
    /// 绑定属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StoreBinding : Attribute {

        public StoreBinding(string key) {
            this.Key = key;
        }

        /// <summary>
        ///  键名
        /// </summary>
        public string Key { set; get; }


        /// <summary>
        /// 双向绑定,当属性值变更时,是否更新Store中的数据
        /// </summary>
        public bool ToWay { set; get; } = false;



    }
}
