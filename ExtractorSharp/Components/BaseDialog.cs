using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Composition;

namespace ExtractorSharp.Components {

    public partial class BaseDialog : BaseForm, IView {

        public BaseDialog() {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if(msg.Msg == 256 || msg.Msg == 260) {
                if(keyData == Keys.Escape) {
                    OnEscape();
                    return true;
                }
            }
            return false;
        }

        protected virtual void OnEscape() {
            DialogResult = DialogResult.Cancel;
        }


        public virtual object ShowView(params object[] args) {
            return ShowDialog();
        }
    }
}
