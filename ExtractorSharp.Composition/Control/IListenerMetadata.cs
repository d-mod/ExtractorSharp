using System.ComponentModel;

namespace ExtractorSharp.Composition.Control {
    public interface IListenerMetadata : IName {

        /// <summary>
        /// 是否是全局监听
        /// </summary>
        [DefaultValue(false)]
        bool IsGlobal { get; }

    }
}
