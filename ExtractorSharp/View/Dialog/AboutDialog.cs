using System;
using System.Diagnostics;
using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Properties;

namespace ExtractorSharp.View{
    public partial class AboutDialog : EaseDialog {
        public AboutDialog(IConnector Data) : base(Data) {
            InitializeComponent();
        }
        

  
    }
}
