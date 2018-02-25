using System;
using ExtractorSharp.Component;
using ExtractorSharp.Properties;
using ExtractorSharp.Data;
using ExtractorSharp.Core;
using ExtractorSharp.Json;

namespace ExtractorSharp.View.Dialog {
    public partial class VersionDialog : ESDialog {
        public VersionDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            button.Click += ButtonClose;
            var builder = new LSBuilder();
            var root = builder.ReadJson(Resources.Version);
            var info=new VersionInfo();
            root[root.Count - 1].GetValue(ref info);
            box.Text = info.ToString();
        }

        private void ButtonClose(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }
}