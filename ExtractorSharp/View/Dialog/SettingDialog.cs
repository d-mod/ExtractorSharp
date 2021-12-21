using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core;
using ExtractorSharp.View.SettingPane;

namespace ExtractorSharp.View.Dialog {

    [ExportMetadata("Name", "setting")]
    [Export(typeof(IView))]
    public partial class SettingDialog : BaseDialog, IPartImportsSatisfiedNotification {

        private Dictionary<TreeNode, ISetting> Dictionary;

        [ImportMany(typeof(ISetting))]
        private List<Lazy<ISetting, ISettingMetadata>> lazies;

        [Import]
        private IConfig Config;

        public SettingDialog() {
        }

        private ISetting Current { set; get; }

        public void OnImportsSatisfied() {
            Dictionary = new Dictionary<TreeNode, ISetting>();
            InitializeComponent();
            tree.AfterSelect += SelectNode;
            yesButton.Click += Yes;
            applyButton.Click += Apply;
            resetButton.Click += Reset;
            cancelButton.Click += Cancel;
            AddConfig();
        }

        protected override void OnEscape() {
            Cancel(null, null);
        }

        private void AddConfig() {
            foreach(var lazy in lazies) {
                var setting = lazy.Value;
                var metadata = lazy.Metadata;
                AddConfig(setting, metadata);
            }
            tree.Sort();

            /*            AddConfig(typeof(GerneralPane));
                        //AddConfig(typeof(BackgroundPane));
                        AddConfig(typeof(GifPane));
                        AddConfig(typeof(SaveImagePane));
                        AddConfig(typeof(RulerPane));
                        AddConfig(typeof(GridPane));
                        AddConfig(typeof(AnimationPane));
                        AddConfig(typeof(LanguagePane));
                        AddConfig(typeof(InstalledPluginPane));
                        //AddConfig(typeof(MarketplacePane));
                        AddConfig(typeof(MovePane));*/
        }

        private void AddConfig(ISetting setting, ISettingMetadata metadata) {
            var control = setting.View as Control;
            var name = metadata.Name;
            var parent = metadata.Parent;
            var parentArray = tree.Nodes.Find(parent, false);
            TreeNode parentNode;
            if(parentArray.Length > 0) {
                parentNode = parentArray[0];
            } else {
                parentNode = tree.Nodes.Add(Language[parent]);
                parentNode.Name = parent;
            }

            TreeNode childrenNode;
            if(name == null) {
                childrenNode = parentNode;
            } else {
                var childrenArray = parentNode.Nodes.Find(name, false);
                if(childrenArray.Length > 0) {
                    childrenNode = childrenArray[0];
                } else {
                    childrenNode = parentNode.Nodes.Add(Language[name]);
                }
            }

            Dictionary[childrenNode] = setting;
        }

        private void SelectNode(object sender, TreeViewEventArgs e) {
            panel.Controls.Clear();
            if(!Dictionary.ContainsKey(e.Node)) {
                return;
            }
            var setting = Dictionary[e.Node];
            var control = setting.View as Control;
            control.Size = panel.Size;
            panel.Controls.Add(control);
            Current = setting;
        }


        private void Initialize() {
            Config.Save();
            foreach(var control in Dictionary.Values) {
                control.Initialize();
            }
        }

        private void Save() {
            foreach(var control in Dictionary.Values) {
                control.Save();
            }
            Config.Save();
        }

        public override object ShowView(params object[] args) {
            Initialize();
            return base.ShowView(args);
        }

        private void Cancel(object sender, EventArgs e) {
            Config.Reload();
            DialogResult = DialogResult.Cancel;
        }

        private void Apply(object sender, EventArgs e) {
            Save();
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