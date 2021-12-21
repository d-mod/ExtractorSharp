using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View.Dialog {

    [Export(typeof(IView))]
    [ExportMetadata("Name", "merge")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MergeDialog : BaseDialog, IProgress<ProgressEventArgs>, IPartImportsSatisfiedNotification {


        private int SelectedIndex { set; get; } = -1;

        private Album Album;

        private const int IGNORE = 0;

        private const int LINEARDODGE = 1;

        private const int NONE = 2;

        private Regex GlowLayerRegex = new Regex("^(.*)f[\\d+]?.img$");

        public MergeDialog() {

        }

        /*
       private string Extensions {
           get {
               var ex = string.Empty;
               foreach (var support in Connector.FileSupports) {
                   ex = string.Concat(ex, $"*{support.Extension};");
               }
               return ex;
           }
       }*/


        public void OnImportsSatisfied() {
            InitializeComponent();
            sortButton.Click += Sort;
            list.MouseDown += ListMouseDown;
            list.DragDrop += ListDragDrop;
            list.DragOver += (o, e) => e.Effect = DragDropEffects.Move;
            palatteBox.SelectedIndexChanged += ChangePalatte;
            deleteItem.Click += Remove;
            clearItem.Click += ClearMerge;
            mergerButton.Click += MergeImg;
            addFileButton.Click += AddOutside;
            moveDownItem.Click += MoveDown;
            moveUpItem.Click += MoveUp;

            priviewPanel.Paint += Priview;
            frameBox.SelectedIndexChanged += (o, e) => priviewPanel.Invalidate();
            lastButton.Click += LastFrame;
            nextButton.Click += NextFrame;
            versionBox.Items.Add(Language["Default"]);
            for(var i = 1; i < Handler.Versions.Count; i++) {
                versionBox.Items.Add(Handler.Versions[i]);
            }
            versionBox.SelectedIndex = 0;
            versionBox.SelectedIndexChanged += SelectVersion;
            glowLayerBox.Items.AddRange(new[] { Language["Ignore"], Language["LinearDodge"], Language["None"] });
            glowLayerBox.SelectedIndex = Config["GlowLayerMode"].Integer;
            glowLayerBox.SelectedIndexChanged += GlowLayer;
            Store.Watch("/merge/queue", _ => Flush(null, null));

        }




        private void GlowLayer(object sender, EventArgs e) {

            //Merger.GlowLayerMode = glowLayerBox.SelectedIndex;
        }

        private void SelectVersion(object sender, EventArgs e) {
            var version = versionBox.SelectedIndex == 0 ? 0 : (int)versionBox.SelectedItem;
            Store.Set("/merge/version", version);
        }

        protected override void OnDragEnter(DragEventArgs e) {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        protected override void OnDragDrop(DragEventArgs e) {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var args = e.Data.GetData(DataFormats.FileDrop, false) as string[];
                Controller.Do("loadFile", args);
                Controller.Do("addMerge");
            }
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            Config["AutoTrim"] = new ConfigValue(autoTrimCheck.Checked);
            Config["AutoSort"] = new ConfigValue(autoSortCheck.Checked);
            Config["MergeCompletedHide"] = new ConfigValue(completedHideCheck.Checked);
            Config["GlowLayerMode"] = new ConfigValue(glowLayerBox.SelectedIndex);
            Config.Save();
        }

        private void ClearMerge(object sender, EventArgs e) {
            Controller.Do("clearMerge");
        }

        private void SelectFile(object sender, EventArgs e) {
            var album = list.SelectedItem as Album;
            if(album != null && list.SelectedIndex != SelectedIndex) {
                palatteBox.Items.Clear();
                for(var i = 0; i < album.Palettes.Count; i++) {
                    palatteBox.Items.Add($"{Language["Palette"]} - {i}");
                }
                palatteBox.SelectedIndex = album.PaletteIndex;
                SelectedIndex = list.SelectedIndex;
            }
        }

        private void ChangePalatte(object sender, EventArgs e) {
            var album = list.SelectedItem as Album;
            if(album != null) {
                var index = palatteBox.SelectedIndex;
                album.PaletteIndex = index;
                priviewPanel.Invalidate();
            }

        }


        private void NextFrame(object sender, EventArgs e) {
            var i = frameBox.SelectedIndex + 1;
            if(i < frameBox.Items.Count) {
                frameBox.SelectedIndex = i;
            }
        }

        private void LastFrame(object sender, EventArgs e) {
            var i = frameBox.SelectedIndex - 1;
            if(i > 0) {
                frameBox.SelectedIndex = i;
            }
        }

        private void Priview(object sender, PaintEventArgs e) {
            Store.Get("/merge/queue", out List<Album> queues);
            var array = queues.ToArray();
            Array.Reverse(array);
            var bmp = Avatars.Preview(array, frameBox.SelectedIndex);
            e.Graphics.DrawImage(bmp, new Point(20, 20));
        }




        /// <summary>
        ///     向上移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveUp(object sender, EventArgs e) {
            var i = list.SelectedIndex;
            if(i > 0) {
                Controller.Do("MoveMerge", new CommandContext {
                    { "Source", i },
                    { "Target", --i }
                });
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
            if(i < list.Items.Count - 1 && i > -1) {
                Controller.Do("MoveMerge", new CommandContext {
                    { "Source", i },
                    { "Target", ++i }
                });
                list.SelectedIndex = i;
            }
        }


        private void AddOutside(object sender, EventArgs e) {
            //TODO
            var dialog = new OpenFileDialog {
                Filter = $"{Language["ImageResources"]}(*.img)|*.img",
                Multiselect = true
            };
            if(dialog.ShowDialog() == DialogResult.OK) {
                Controller
                    .Do("loadFile", dialog.FileNames)
                    .Do("addMerge");
            }
        }

        private void Sort(object sender, EventArgs e) {
            Controller.Do("sortMerge");
        }

        public int GetFrameCount() {
            Store.Get("/merge/queue", out List<Album> queues);
            var count = 0;
            foreach(var al in queues) {
                if(al.List.Count > count) {
                    count = al.List.Count;
                }
            }
            return count;
        }


        private void Flush(object sender, MergeQueueEventArgs e) {
            if(autoSortCheck.Checked && (e.Mode == QueueChangeMode.Add || e.Mode == QueueChangeMode.Remove)) {
                Sort(sender, e);
                return;
            }
            Store.Get("/merge/queue", out List<Album> queues);
            list.Items.Clear();
            list.Items.AddRange(queues.ToArray());
            var count = GetFrameCount();
            var arr = new string[count];
            for(var i = 0; i < arr.Length; i++) {
                arr[i] = $"{Language["FrameCount"]} - {i}";
            }
            frameBox.Items.Clear();
            frameBox.Items.AddRange(arr);
            if(arr.Length > 0) {
                frameBox.SelectedIndex = 0;
            }
            priviewPanel.Invalidate();
        }


        public override object ShowView(params object[] args) {
            Album = args[0] as Album;
            Flush(this, new MergeQueueEventArgs { Mode = QueueChangeMode.Add });
            Store.Get("/data/files", out List<Album> files);

            targetBox.Items.Clear();
            targetBox.Items.AddRange(files.ToArray());
            targetBox.SelectedItem = Album;
            return ShowDialog();
        }


        private void ListMouseDown(object sender, MouseEventArgs e) {
            SelectFile(sender, e);
            if(list.Items.Count == 0 || e.Button != MouseButtons.Left || list.SelectedIndex < 0 ||
                e.Clicks == 2) {
                return;
            }
            DoDragDrop(list.SelectedItem, DragDropEffects.Move);
        }

        private void ListDragDrop(object sender, DragEventArgs e) {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                OnDragDrop(e);
                return;
            }
            var index = list.SelectedIndex;
            var target = list.IndexFromPoint(list.PointToClient(new Point(e.X, e.Y)));
            target = target < 0 ? list.Items.Count - 1 : target;
            Controller.Do("MoveMerge", new CommandContext {
                    { "Source", index },
                    { "Target", target }
                });
            if(target > -1) {
                list.SelectedIndex = target;
            }
        }


        public int GlowLayerHandle(List<Album> list, int version) {
            var glowLayerMode = glowLayerBox.SelectedIndex;
            if(glowLayerMode != NONE) {
                for(var i = 0; i < list.Count; i++) {
                    var f = list[i];
                    if(GlowLayerRegex.IsMatch(f.Name)) {
                        switch(glowLayerMode) {
                            case IGNORE:
                                list.RemoveAt(i);
                                i--;
                                break;
                            case LINEARDODGE:
                                list.RemoveAt(i);
                                var clone = f.Clone();
                                clone.List.ForEach(e => e.Image = e.Image.LinearDodge());
                                list.Insert(i, clone);
                                version = (int)ImgVersion.Ver2;
                                break;
                        }
                    }
                }
            }
            return version;
        }

        private void Remove(object sender, EventArgs e) {
            var album = list.SelectedItem as Album;
            if(album != null) {
                Controller.Do("removeMerge", new Album[] { album });
            }
        }

        private void MergeImg(object sender, EventArgs e) {
            if(list.Items.Count < 1) {
                //当拼合队列为空时
                Messager.Warning("EmptyMergeTips");
                return;
            }
            if(targetBox.SelectedItem == null) {
                //没有选择Img时
                var name = targetBox.Text;
                if(name == string.Empty) {
                    Messager.Warning("NotSelectImgTips");
                    return;
                }
                Store.Get("/data/files", out List<Album> files);
                var rs = files.Find(al => al.Name.Equals(name));
                if(rs != null) {
                    Album = rs;
                } else {
                    Album = new Album {
                        Path = targetBox.Text
                    };
                    Controller.Do("newFile", Album);
                }
            } else {
                Album = targetBox.SelectedItem as Album;
            }
            Controller.Do("runMerge", this);
        }

        public void Report(ProgressEventArgs e) {
            prograss.Value = e.Value;
            if(e.Value < 1) {
                mergerButton.Enabled = false;
                sortButton.Enabled = false;
                prograss.Show();
                prograss.Maximum = e.Maximum;
            } else if(e.Value == e.Maximum) {
                Controller.Do("ReplaceFile", new CommandContext {
                    { "Source",Album },
                    { "Target",e.Result as Album }
                });
                mergerButton.Enabled = true;
                sortButton.Enabled = true;
                prograss.Visible = false;
                if(autoTrimCheck.Checked) {
                    Controller.Do("trimImage", Album);
                }
                if(completedHideCheck.Checked) {
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}