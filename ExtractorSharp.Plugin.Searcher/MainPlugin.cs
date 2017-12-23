using ExtractorSharp.Composition;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Plugin.Searcher {
    [ExportMetadata("Name", "ExtrctorSharp Searcher")]
    [ExportMetadata("Version", "1.7.0.0")]
    [ExportMetadata("Since", "1.6.5.0")]
    [ExportMetadata("Description", "模型搜索")]
    [ExportMetadata("Author", "Kritsu")]
    [ExportMetadata("Guid", "D72DF478-FAFF-43DF-B904-9EB338A08B54")]
    [Export(typeof(IPlugin))]
    class MainPlugin :IPlugin{
    }
}
