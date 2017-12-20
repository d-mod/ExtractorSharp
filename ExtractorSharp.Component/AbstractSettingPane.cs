using ExtractorSharp.Config;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Component {
    public abstract class AbstractSettingPane : Panel {
        public IConfig Config => Data.Config;

        public Language Language => Data.Language;

        public IConnector Data { set; get; }

        public AbstractSettingPane(IConnector Data) {
            this.Data = Data;
        }
        /// <summary>
        /// <para>节点名</para>
        /// <para>为null时则挂载到父节点上</para>
        /// <para>Language中的key,请勿使用中文</para>
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// <para>父节点</para>
        /// </summary>
        public new string Parent{ set; get; }
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// 保存
        /// </summary>
        public abstract void Save();
    }
}
