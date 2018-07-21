using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View.Dialog {
    internal partial class MergeDialog : ESDialog {
        private readonly Merger Merger;
        private int SelectedIndex { set; get; } = -1;
        private Album Album;

        public MergeDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            Merger = Program.Merger;
            sortButton.Click += (o, e) => Merger.Sort();
            list.MouseDown += ListMouseDown;
            list.DragDrop += ListDragDrop;
            list.DragOver += (o, e) => e.Effect = DragDropEffects.Move;
            palatteBox.SelectedIndexChanged += ChangePalatte;
            deleteItem.Click += Remove;
            clearItem.Click += (o, e) => Connector.Do("clearMerge");
            mergerButton.Click += MergeImg;
            addFileButton.Click += AddOutside;
            moveDownItem.Click += MoveDown;
            moveUpItem.Click += MoveUp;
            Merger.MergeQueueChanged += Flush;
            Merger.MergeStarted += MergeStart;
            Merger.MergeProcessing += MergeProcessing;
            Merger.MergeCompleted += MergeCompleted;
            priviewPanel.Paint += Priview;
            frameBox.SelectedIndexChanged += (o, e) => priviewPanel.Invalidate();
            lastButton.Click += LastFrame;
            nextButton.Click += NextFrame;
            versionBox.Items.Add(Language["Default"]);
            for (var i = 1; i < Handler.Versions.Count; i++) {
                versionBox.Items.Add(Handler.Versions[i]);
            }
            versionBox.SelectedIndex = 0;
            versionBox.SelectedIndexChanged += SelectVersion;
            glowLayerBox.Items.AddRange(new[] { Language["Ignore"], Language["LinearDodge"], Language["None"] });
            glowLayerBox.SelectedIndex = Config["GlowLayerMode"].Integer;
            glowLayerBox.SelectedIndexChanged += GlowLayer;
        }


        private void GlowLayer(object sender, EventArgs e) {
            Merger.GlowLayerMode = glowLayerBox.SelectedIndex;
        }

        private void SelectVersion(object sender, EventArgs e) {
            if (versionBox.SelectedIndex == 0) {
                Merger.Version = 0;
            } else {
                Merger.Version = (int)versionBox.SelectedItem;
            }
        }

        protected override void OnDragEnter(DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        protected override void OnDragDrop(DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var args = e.Data.GetData(DataFormats.FileDrop, false) as string[];
                var array = NpkCoder.Load(args).ToArray();
                Connector.Do("addMerge", array);
            }
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            Config["AutoSort"] = new ConfigValue(autoSortCheck.Checked);
            Config["MergeCompletedHide"] = new ConfigValue(completedHideCheck.Checked);
            Config["GlowLayerMode"] = new ConfigValue(glowLayerBox.SelectedIndex);
            Config.Save();
        }

        private void SelectFile(object sender, EventArgs e) {
            var album = list.SelectedItem as Album;
            if (album != null && list.SelectedIndex != SelectedIndex) {
                palatteBox.Items.Clear();
                for (var i = 0; i < album.Tables.Count; i++) {
                    palatteBox.Items.Add($"{Language["Palette"]} - {i}");
                }
                palatteBox.SelectedIndex = album.TableIndex;
                SelectedIndex = list.SelectedIndex;
            }
        }

        private void ChangePalatte(object sender, EventArgs e) {
            var album = list.SelectedItem as Album;
            if (album != null) {
                var index = palatteBox.SelectedIndex;
                album.TableIndex = index;
                priviewPanel.Invalidate();
            }

        }


        private void NextFrame(object sender, EventArgs e) {
            var i = frameBox.SelectedIndex + 1;
            if (i < frameBox.Items.Count) {
                frameBox.SelectedIndex = i;
            }
        }

        private void LastFrame(object sender, EventArgs e) {
            var i = frameBox.SelectedIndex - 1;
            if (i > 0) {
                frameBox.SelectedIndex = i;
            }
        }

        private void Priview(object sender, PaintEventArgs e) {
            Merger.Priview(frameBox.SelectedIndex, e.Graphics);
        }

        private void MergeCompleted(object sender, MergeEventArgs e) {
            Connector.Do("replaceImg", Album, e.Album);
            mergerButton.Enabled = true;
            sortButton.Enabled = true;
            prograss.Visible = false;
            if (completedHideCheck.Checked) {
                DialogResult = DialogResult.OK;
            }
        }

        private void MergeProcessing(object sender, MergeEventArgs e) {
            prograss.Value++;
        }

        private void MergeStart(object sender, MergeEventArgs e) {
            mergerButton.Enabled = false;
            sortButton.Enabled = false;
            prograss.Show();
            prograss.Value = 0;
            prograss.Maximum = e.Count;
        }


        /// <summary>
        ///     向上移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveUp(object sender, EventArgs e) {
            var i = list.SelectedIndex;
            if (i > 0) {
                Connector.Do("moveMerge", i, --i);
                list.SelectedIndex = i;
            }
        }

        /// <summary>
        ///     向下移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveDown(object sender, EventArgs e) {
            var i = list.SelectedIndex;
            if (i < list.Items.Count - 1 && i > -1) {
                Connector.Do("moveMerge", i, ++i);
                list.SelectedIndex = i;
            }
        }


        private void AddOutside(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = $"{Language["ImageResources"]}| *.NPK; *.img";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK) {
                var array = NpkCoder.Load(dialog.FileNames).ToArray();
                Connector.Do("addMerge", array);
            }
        }


        private void Flush(object sender, MergeQueueEventArgs e) {
            if (autoSortCheck.Checked && (e.Mode == QueueChangeMode.Add || e.Mode == QueueChangeMode.Remove)) {
                Merger.Sort();
                return;
            }
            list.Items.Clear();
            list.Items.AddRange(Merger.Queues.ToArray());
            var count = Merger.GetFrameCount();
            var arr = new string[count];
            for (var i = 0; i < arr.Length; i++) {
                arr[i] = $"{Language["FrameCount"]} - {i}";
            }
            frameBox.Items.Clear();
            frameBox.Items.AddRange(arr);
            if (arr.Length > 0) {
                frameBox.SelectedIndex = 0;
            }
            priviewPanel.Invalidate();
        }


        public override DialogResult Show(params object[] args) {
            Album = args[0] as Album;
            Flush(this, new MergeQueueEventArgs { Mode = QueueChangeMode.Add});
            targetBox.Items.Clear();
            targetBox.Items.AddRange(Connector.FileArray);
            targetBox.SelectedItem = Album;
            return ShowDialog();
        }


        private void ListMouseDown(object sender, MouseEventArgs e) {
            SelectFile(sender, e);
            if (list.Items.Count == 0 || e.Button != MouseButtons.Left || list.SelectedIndex < 0 ||
                e.Clicks == 2) {
                return;
            }
            DoDragDrop(list.SelectedItem, DragDropEffects.Move);
        }

        private void ListDragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                OnDragDrop(e);
                return;
            }
            var index = list.SelectedIndex;
            var target = list.IndexFromPoint(list.PointToClient(new Point(e.X, e.Y)));
            target = target < 0 ? list.Items.Count - 1 : target;
            Connector.Do("moveMerge", index, target);
            if (target > -1) {
                list.SelectedIndex = target;
            }
        }

        private void Remove(object sender, EventArgs e) {
            var album = list.SelectedItem as Album;
            if (album != null) {
                Connector.Do("removeMerge", new Album[] { album});
            }
        }

        private void MergeImg(object sender, EventArgs e) {
            if (list.Items.Count < 1) {
                //当拼合队列为空时
                Connector.SendWarning("EmptyMergeTips");
                return;
            }
            if (targetBox.SelectedItem == null) {
                //没有选择Img时
                var name = targetBox.Text;
                if (name == string.Empty) {
                    Connector.SendWarning("NotSelectImgTips");
                    return;
                }
                var rs = Connector.FileArray.Find(al => al.Name.Equals(name));
                if (rs != null) {
                    Album = rs;
                } else {
                    Album = new Album();
                    Connector.Do("newImg", Album, targetBox.Text);
                }
            } else {
                Album = targetBox.SelectedItem as Album;
            }
            Connector.Do("runMerge");
        }
    }
}