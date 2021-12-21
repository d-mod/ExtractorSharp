using System.ComponentModel;

namespace ExtractorSharp.Composition.View {

    public interface IViewMetadata : IName {
        [DefaultValue(null)]
        string Title { get; }
    }

}
