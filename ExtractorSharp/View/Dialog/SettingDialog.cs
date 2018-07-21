using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.View.SettingPane;
using ExtractorSharp.View.SettingPane.Theme;

namespace ExtractorSharp.View.Dialog {
    public partial class SettingDialog : ESDialog {
        private readonly Dictionary<TreeNode, AbstractSettingPane> Dictionary;

        public SettingDialog(IConnector Connector) : base(Connector) {
            Dictionary = new Dictionary<TreeNode, AbstractSettingPane>();
            InitializeComponent();
            tree.AfterSelect += SelectNode;
            yesButton.Click += Yes;
            applyButton.Click += Apply;
            resetButton.Click += Reset;
            cancelButton.Click += Cancel;
            try {
                AddConfig();
            } catch (Exception e) { }
        }

        private AbstractSettingPane SelectPane { set; get; }


        protected override void OnEscape() {
            Cancel(null, null);
        }

        private void AddConfig() {
            AddConfig(typeof(GerneralPane));
            //AddConfig(typeof(BackgroundPane));
            AddConfig(typeof(GifPane));
            AddConfig(typeof(SaveImagePane));
            AddConfig(typeof(RulerPane));
            AddConfig(typeof(GridPane));
            AddConfig(typeof(AnimationPane));
            AddConfig(typeof(LanguagePane));
            AddConfig(typeof(InstalledPluginPane));
            AddConfig(typeof(MarketplacePane));
            AddConfig(typeof(MovePane));
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

        private void SelectNode(object sender, TreeViewEventArgs e) {
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
            Config.Save();
            foreach (var control in Dictionary.Values) {
                control.Initialize();
            }
        }

        private void Save() {
            foreach (var control in Dictionary.Values) {
                control.Save();
            }
            Config.Save();
        }

        public override DialogResult Show(params object[] args) {
            Initialize();
            return ShowDialog();
        }

        private void Cancel(object sender, EventArgs e) {
            Config.Reload();
            DialogResult = DialogResult.Cancel;
        }

        private void Apply(object sender, EventArgs e) {
            Save();
            Connector.CanvasFlush();
        }

        private void Yes(object sender, EventArgs e) {
            Apply(sender, e);
            DialogResult = DialogResult.OK;
        }

        private void Reset(object sender, EventArgs e) {
            Config.Reset();
            Initialize();
        }
    }
}