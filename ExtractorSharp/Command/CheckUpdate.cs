using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Json;
using ExtractorSharp.Model;

namespace ExtractorSharp.Services.Commands {

    [Export(typeof(ICommand))]
    [ExportMetadata("Name", "CheckUpdate")]
    class CheckUpdate : InjectService, ICommand {


        public void Do(CommandContext context) {
            var Tips = context.Get<bool>();

            var builder = new LSBuilder();
            var url = $"{Config["ApiHost"]}{Config["UpdateUrl"]}";
            var obj = builder.Get(url);

            var info = obj?.GetValue(typeof(VersionInfo)) as VersionInfo;

            if(info != null && !info.Name.Equals(Application.ProductVersion)) {
                //若当前版本低于最新版本时，触发更新
                if(MessageBox.Show(Language["Tips"], Language["Tips","NeedUpdate"],
                        CommonMessageBoxButton.OKCancel) != CommonMessageBoxResult.OK) {
                    return; //提示更新
                }
                StartUpdate(); //启动更新
            } else if(Tips) {
                MessageBox.Show(Language["Tips"],Language["Tips","NeedNotUpdate"]); //提示不需要更新
            }
            Config.Save();
        }

        /// <summary>
        ///     启动更新
        /// </summary>
        /// <param name="updateUrl"></param>
        /// <param name="address"></param>
        /// <param name="productName"></param>
        private void StartUpdate() {
            var url = Config["UpdateExeUrl"].Value;

            var filename = $"{Config["RootPath"]}\\{url.GetSuffix()}";
            try {
                var client = new WebClient();
                client.DownloadFile(url, filename);
                client.Dispose();
                Process.Start(filename);
            } finally {
                Environment.Exit(-1);
            }
        }
    }
}
