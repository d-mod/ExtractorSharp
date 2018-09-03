using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.Install {
    public partial  class PagePanel : UserControl {

        public event EventHandler Nexting;

        public event EventHandler Canceling;

        protected Dictionary<string,ConfigValue> Config;

        protected bool ShowNext {
            set {
                nextButton.Visible = value;
            }
        }

        protected bool ShowCancel {
            set {
                cancelButton.Visible = value;
            }
        }

        

        public PagePanel(Dictionary<string,ConfigValue> Config) {
            this.Config = Config;
            InitializeComponent();
            nextButton.Click += (o, e) => Next();
            cancelButton.Click += (o, e) => Cancel();
        }

        public virtual void Init() {

        }

        public virtual void Cancel() {
            Canceling?.Invoke(this, null);
        }

        public virtual void Next() {
            Nexting?.Invoke(this,null);
        }

    }
}
