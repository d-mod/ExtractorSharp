using System.ComponentModel;

namespace ExtractorSharp.Composition {
    public interface ISettingMetadata {

        [DefaultValue(null)]
        string Name { get; }

        [DefaultValue(null)]
        string Parent { get; }

        [DefaultValue(-1)]
        int Index { get; }

    }
}
