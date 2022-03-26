using System;
using System.ComponentModel.Composition;
using System.IO;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Listener {
    [ExportMetadata("Name", "CommandLog")]
    [ExportMetadata("IsGlobal", true)]
    [Export(typeof(ICommandListener))]
    internal class CommandLog : InjectService, ICommandListener {

        [StoreBinding(StoreKeys.APP_DIR)]
        private string APP_DIR { set; get; }

        private string FileName { set; get; }

        public override void OnImportsSatisfied() {
            base.OnImportsSatisfied();
            var time = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var dir = $"{this.APP_DIR}/logs";
            this.FileName = $"{dir}/COMMAND_{time}.log";
            if(!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            this.Log("Command Logger Startup...");
        }


        public void After(CommandEventArgs e) {

        }

        public void Before(CommandEventArgs e) {
            var name = this.Language[e.Name];
            this.Log($"Command [{name}] Did.");
        }

        private void Log(string message) {
            var time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var writer = File.AppendText(this.FileName);
            writer.WriteLine();
            writer.Write(time);
            writer.Write(" : ");
            writer.Write(message);
            writer.Close();
        }

    }
}
