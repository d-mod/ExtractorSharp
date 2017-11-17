using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExtractorSharp.Loose.Test {
    public partial class MainForm : Form {
        LSObject Root;
        public MainForm() {
            InitializeComponent();
            loadItem.Click += Browse;
            saveAsItem.Click += SaveAs;
            loadPropItem.Click += LoadProp;
        }

        private void LoadProp(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var name = dialog.FileName;
                using (var reader = new StreamReader(dialog.FileName)) {
                    var prop = reader.ReadToEnd();
                    var builder = new LSBuilder();
                    Root = builder.ReadProperties(prop);
                }
                ReFlush();
            }
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var name = dialog.FileName;
                var reader = new LSBuilder();
                if (name.EndsWith(".xml"))
                    Root = reader.ReadXml(name);
                else
                    Root = reader.Read(name);
                ReFlush();
            }
        }


        private void SaveAs(object sender,EventArgs e) {
            var dialog = new SaveFileDialog();
            dialog.Filter = "json文件|*.json";
            if (dialog.ShowDialog() == DialogResult.OK) {
                var builder = new LSBuilder();
                builder.Write(Root, dialog.FileName);
            }
        }

        private void RightClick(object sender,EventArgs e) {

        }

        private void ReFlush() {
            rootNode.Nodes.Clear();
            textbox.Text = Root?.ToString();
            ReFlush(rootNode.Nodes, Root);
            tree.ExpandAll();
            textbox.Font = new System.Drawing.Font("consolas", 10);
        }

        private void ReFlush(TreeNodeCollection node, LSObject obj) {
            if (obj != null) {
                foreach (var child in obj) {
                    if (child.ValueType == LSType.Object) {
                        var n = node.Add(child.Name + "");
                        ReFlush(n.Nodes, child);
                    } else {
                        var text = child.Name == null ? "" : $"{child.Name} :";
                        text += $"[{child.ValueType}]";
                        text += child.Value == null ? "null" : child.Value.ToString();
                        node.Add(text);
                    }
                }
            }
        }
    }
}
