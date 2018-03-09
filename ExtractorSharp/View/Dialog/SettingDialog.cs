using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.View.SettingPane;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog {
    public partial class SettingDialog : ESDialog {
        private Dictionary<TreeNode, AbstractSettingPane> Dictionary;
        private AbstractSettingPane SelectPane { set; get; }
        public SettingDialog(IConnector Data) : base(Data) {
            Dictionary = new Dictionary<TreeNode, AbstractSettingPane>();
            InitializeComponent();
            this.tree.AfterSelect += SelectNode;
            yesButton.Click += Yes;
            applyButton.Click += Apply;
            resetButton.Click += Reset;
            cancelButton.Click += Cancel;
            AddConfig(typeof(GerneralPane));
            AddConfig(typeof(SaveImagePane));
            AddConfig(typeof(GridPane));
            AddConfig(typeof(FlashPane));
            AddConfig(typeof(LanguagePane));
            AddConfig(typeof(PluginListPane));
            AddConfig(typeof(MarketplacePane));
        }


        protected override void OnEscape() {
            Cancel(null, null);
        }


        private void AddConfig(Type type) {
            if (!typeof(AbstractSettingPane).IsAssignableFrom(type)) {
                return;
            }
            var control = type.CreateInstance(Connector) as AbstractSettingPane;
            var parentArray = tree.Nodes.Find(control.Parent, false);
            TreeNode parentNode = null;
            if (parentArray.Length > 0) {
                parentNode = parentArray[0];
            } else {
                parentNode = tree.Nodes.Add(Language[control.Parent]);
                parentNode.Name = control.Parent;
            }
            TreeNode childrenNode = null;
            if (control.Name == null) {
                childrenNode = parentNode;
            } else {
                var name = Language[control.Name];
                var childrenArray = parentNode.Nodes.Find(name, false);
                if (childrenArray.Length > 0) {
                    childrenNode = childrenArray[0];
                } else {
                    childrenNode = parentNode.Nodes.Add(name);
                }
            }
            Dictionary[childrenNode] = control;
        }

        private void SelectNode(object sender,TreeViewEventArgs e) {
            panel.Controls.Clear();
            if (!Dictionary.ContainsKey(e.Node)) {
                return;
            }
            var control = Dictionary[e.Node];
            control.Size = panel.Size;
            panel.Controls.Add(control);
            SelectPane = control;
        }


        private void Initialize() {
            foreach (var control in Dictionary.Values) {
                control.Initialize();
            }
        }

        private void Save() {
            foreach (var control in Dictionary.Values) {
                control.Save();
            }
        }

        public override DialogResult Show(params object[] args) {
            Initialize();
            return ShowDialog();
        }

        private void Cancel(object sender, EventArgs e) {
            Config.Reload();
            DialogResult = DialogResult.Cancel;
        }

        private void Apply(object sender,EventArgs e) {
            Save();
            Config.Save();
        }

        private void Yes(object sender,EventArgs e) {
            Apply(sender, e);
            DialogResult = DialogResult.OK;
        }

        private void Reset(object sender, EventArgs e) {
            Config.Reset();
            Initialize();
        }
        
       
    }
}
