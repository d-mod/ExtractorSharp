using ExtractorSharp.Composition;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Plugin.Searcher {
    [ExportMetadata("Name", "ExtrctorSharp 文件下载")]
    [ExportMetadata("Version", "1.7.0.0")]
    [ExportMetadata("Since", "1.6.5.0")]
    [ExportMetadata("Description", "文件下载")]
    [ExportMetadata("Author", "Kritsu")]
    [ExportMetadata("Guid", "87844cf0-a062-4f34-8c3b-d8e6bda28daa")]
    [Export(typeof(IPlugin))]
    class MainPlugin :IPlugin{
    }
}
