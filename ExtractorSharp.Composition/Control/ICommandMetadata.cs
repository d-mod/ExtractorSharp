using System.ComponentModel;

namespace ExtractorSharp.Composition.Control {

    public interface ICommandMetadata : IName {

        [DefaultValue(RefreshMode.None)]
        RefreshMode RefreshMode { get; }

        [DefaultValue(new string[0])]
        string[] Listeners { get; }

    }

}
