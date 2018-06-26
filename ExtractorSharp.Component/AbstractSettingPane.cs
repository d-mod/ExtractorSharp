using System.Windows.Forms;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Component {
    public abstract class AbstractSettingPane : Panel {
        public AbstractSettingPane(IConnector Connector) {
            this.Connector = Connector;
        }

        public IConfig Config => Connector.Config;

        public Language Language => Connector.Language;

        public IConnector Connector { set; get; }

        /// <summary>
        ///     <para>节点名</para>
        ///     <para>为null时则挂载到父节点上</para>
        ///     <para>Language中的key,请勿使用中文</para>
        /// </summary>
        public new string Name { set; get; }

        /// <summary>
        ///     <para>父节点</para>
        /// </summary>
        public new string Parent { set; get; }

        /// <summary>
        ///     初始化
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        ///     保存
        /// </summary>
        public abstract void Save();
    }
}