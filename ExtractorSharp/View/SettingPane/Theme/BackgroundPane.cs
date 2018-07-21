using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.View.SettingPane.Theme {
    public partial class BackgroundPane : AbstractSettingPane {
        public BackgroundPane(IConnector Connector):base(Connector){
            InitializeComponent();
        }

        public override void Initialize() {

        }

        public override void Save() {

        }
    }
}
