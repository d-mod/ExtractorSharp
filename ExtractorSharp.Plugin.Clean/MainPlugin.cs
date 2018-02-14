using ExtractorSharp.Composition;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Plugin.Clean {

    [ExportMetadata("Name", "ExtrctorSharp Clean")]
    [ExportMetadata("Version", "1.7.0.0")]
    [ExportMetadata("Since", "1.6.7.0")]
    [ExportMetadata("Description", "模型清理")]
    [ExportMetadata("Author", "Kritsu")]
    [ExportMetadata("Guid", "a4bb046f-acab-44b5-a0f0-2dd84278b43d")]
    [Export(typeof(IPlugin))]
    public class MainPlugin : IPlugin {
    }
}
