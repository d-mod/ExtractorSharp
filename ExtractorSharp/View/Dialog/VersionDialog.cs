using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Json;
using ExtractorSharp.Model;
using ExtractorSharp.Properties;

namespace ExtractorSharp.View.Dialog {
    public partial class VersionDialog : ESDialog {
        public VersionDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            button.Click += ButtonClose;
            var builder = new LSBuilder();
            var root = builder.ReadJson(Resources.Version);
            var info = new VersionInfo();
            root.GetValue(ref info);
            box.Text = info.ToString();
        }

        private void ButtonClose(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }
    }
}