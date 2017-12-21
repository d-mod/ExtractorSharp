using ExtractorSharp.Composition;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Plugin.DressingRoom {
    [ExportMetadata("Name", "ExtrctorSharp Dressing Room")]
    [ExportMetadata("Version", "1.7.0.0")]
    [ExportMetadata("Since","1.6.5.0")]
    [ExportMetadata("Description", "试衣间")]
    [ExportMetadata("Guid", "57951442-F28C-4D84-8677-0DE4105BFBD1")]
    [ExportMetadata("Author","Kritsu")]
    [Export(typeof(IPlugin))]
    class MainPlugin : IPlugin {
    }
}
