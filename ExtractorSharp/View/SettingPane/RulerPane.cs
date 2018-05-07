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
using ExtractorSharp.Core;

namespace ExtractorSharp.View.SettingPane {
    public partial class RulerPane : AbstractSettingPane {
        public RulerPane(IConnector Connector):base(Connector){
            InitializeComponent();
        }

        public override void Initialize() {

        }

        public override void Save() {

        }
    }
}
