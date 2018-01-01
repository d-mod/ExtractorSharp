using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ExtractorSharp.View;
using ExtractorSharp.Component;
using ExtractorSharp.Handle;
using ExtractorSharp.Draw;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using System.Drawing.Drawing2D;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Draw.Paint;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Composition;
using ExtractorSharp.Lib;

namespace ExtractorSharp {
    public partial class MainForm : ESForm {


        private Image BackImage;
        private Color BackBoxColor = Color.DimGray;
        private Viewer Viewer { get; }
        private Drawer Drawer { get; }
        private Controller Controller { get; }
        private decimal ImageScale => scaleBox.Value / 100;
        public string Path {
            set => pathBox.Text = value;
            get => pathBox.Text;
        }

        private Point Hotpot { set; get; }

        /// <summary>
        /// 当前图层
        /// </summary>
        private IPaint CurrentLayer { set; get; } = new Canvas();
        /// <summary>
        /// 上一图层
        /// </summary>
        private IPaint LastLayer { set; get; }


        /// <summary>
        /// 绘图的临时图层
        /// </summary>
        private IPaint BufferLayer { set; get; }

        private IPaint Rule { set; get; }

        private IPaint Grid { set; get; }
        
        private int move_mode = -1;
        private int rule_radius = 25;


        public MainForm():base(new MainConnector()) {
            (this.Connector as MainConnector).MainForm = this;
            InitializeComponent();
            Controller = Program.Controller;
            Viewer = Program.Viewer;
            Drawer = Program.Drawer;
            dropPanel = new DropPanel();
            player = new OggPlayer(Controller);
            Controls.Add(dropPanel);
            Controls.Add(player);
            player.BringToFront();
            previewPanel.BringToFront();
            AddListenter();
            AddShow();
            AddBrush();
            AddPaint();
        }

        private void AddBrush() {
            foreach (var entry in Drawer.Brushes) {
                var item = new ToolStripMenuItem(Language[entry.Key]);
                item.CheckOnClick = true;
                if (Drawer.IsSelect(entry.Key)) {
                    item.Checked = true;
                }            
                item.CheckedChanged += (o, e) => {
                    Drawer.Select(entry.Key);
                    foreach (ToolStripMenuItem i in toolsMenu.DropDownItems) {
                        i.Checked = false;
                    }
                    item.Checked = true;
                };
                toolsMenu.DropDownItems.Add(item);
            }
        }

        private void AddPaint() {
            Rule = new Rule();
            Grid = new Grid();
            AddPaint(displayRuleItem, Rule);
            AddPaint(gridItem, Grid);
        }

        private void AddPaint(ToolStripMenuItem item ,IPaint paint) {
            item.CheckedChanged += (o, e) => paint.Visible = item.Checked;
        }
        

        /// <summary>
        /// 给不需要动态参数的窗口-菜单添加监听
        /// </summary>
        private void AddShow() {
            AddShow(clearItem, "clear");
            AddShow(aboutItem, "about");
            AddShow(debugItem, "debug", "feedback");
            AddShow(propertyItem, "setting");
            AddShow(versionItem, "version");
            AddShow(otherSeverItem, "download");
        }

        public void AddShow(ToolStripMenuItem item, string name, params object[] args) => item.Click += (o, e) => Viewer.Show(name, args);


        public void AddCommand(Control control,string name) {
            control.Click += (o, e) => Controller.Do(name, base.Connector);
        }

        public void AddCommand(ToolStripItem control,string name) {
            control.Click += (o, e) => Controller.Do(name, base.Connector);
        }


        public ToolStripMenuItem AddMenuItem(IMenuItem item) {
            var toolItem = new ToolStripMenuItem(Language[item.Name]);
            switch (item.Parent) {
                case MenuItemType.MAIN:
                    mainMenu.Items.Add(toolItem);
                    break;
                case MenuItemType.FILE:
                    fileMenu.DropDownItems.Add(toolItem);
                    break;
                case MenuItemType.EDIT:
                    editMenu.DropDownItems.Add(toolItem);
                    break;
                case MenuItemType.VIEW:
                    editMenu.DropDownItems.Add(toolItem);
                    break;
                case MenuItemType.MODEL:
                    modelMenu.DropDownItems.Add(toolItem);
                    break;
                case MenuItemType.TOOLS:
                    toolsMenu.DropDownItems.Add(toolItem);
                    break;
                case MenuItemType.ABOUT:
                    aboutMenu.DropDownItems.Add(toolItem);
                    break;
                case MenuItemType.FILELIST:
                    albumListMenu.Items.Add(toolItem);
                    break;
                case MenuItemType.IMAGELIST:
                    imageListMenu.Items.Add(toolItem);
                    break;
                default:
                    return null;
            }
            if (!string.IsNullOrEmpty(item.Command)) {
                var command = item.Command;
                switch (item.Click) {
                    case ClickType.Command:
                        AddCommand(toolItem, command);
                        break;
                    case ClickType.View:
                        AddShow(toolItem, command);
                        break;
                }
            }
            if (item.Childrens != null) {
                AddChildItem(item);
            }
            return toolItem;
        }

        public void AddChildItem(IMenuItem item) {
            foreach (var child in item.Childrens) {
                var childItem = new ToolStripMenuItem(Language[child.Name]);
                childItem.DropDownItems.Add(childItem);
                if (string.IsNullOrEmpty(item.Command)) {
                    var command = child.Command;
                    switch (item.Click) {
                        case ClickType.Command:
                            AddCommand(childItem, command);
                            break;
                        case ClickType.View:
                            AddShow(childItem, command);
                            break;
                    }
                }
                if (item.Childrens != null) {
                    AddChildItem(child);
                }
            }
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        private void AddListenter() {
            addFileItem.Click += AddFile;
            openDirItem.Click += InputDirectory;
            saveAsFileItem.Click += OutputFile;
            saveDirItem.Click += OutputDirectory;
            replaceItem.Click += ReplaceImg;
            saveAsItem.Click += SaveAsImg;
            renameItem.Click += RenameImg;
            addMergeItem.Click += AddMerge;
            addOutsideMergeItem.Click += AddOutMerge;
            runMergeItem.Click += DisplayMerge;
            albumList.SelectedIndexChanged += ImageChanged;
            albumList.Deleted = DeleteImg;
            albumList.ItemDraged += MoveFileIndex;
            albumList.DragDrop += DragDropInput;
            box.Paint += OnPainting;
            box.MouseClick += OnMouseClick;
            box.MouseDown += OnMouseDown;
            box.MouseUp += OnMouseUp;
            box.MouseMove += OnMouseMove;
            box.MouseWheel += OnMouseWheel;
            saveImageItem.Click += SaveImage;
            saveSingleImageItem.Click += SaveSingleImage;
            saveAllImageItem.Click += SaveAllImage;
            saveGifItem.Click += SaveGif;
            replaceImageItem.Click += ReplaceImage;
            hideCheckImageItem.Click += (o, e) => Controller.Do("hideImage", base.Connector.SelectedFile, base.Connector.CheckedImageIndices);
            linkImageItem.Click += LinkImage;
            imageList.Deleted = DeleteImage;
            imageList.ItemDraged += MoveImageIndex;
            imageList.SelectedIndexChanged += SelectImageChanged;
            imageList.ItemHoverChanged += PreviewHover;
            changePositionItem.Click += (o, e) => Viewer.Show("changePosition", base.Connector.CheckedImages);
            changeSizeItem.Click += (o, e) => Controller.Do("changeSize", base.Connector.SelectedFile, imageList.CheckedIndices, ImageScale);
            searchBox.TextChanged += (o, e) => ListFlush();

            newImageItem.Click += (o, e) => Viewer.Show("newImage", base.Connector.SelectedFile);
            realPostionBox.CheckedChanged += SelectImageChanged;
            newImgItem.Click += ShowNewImgDialog;
            hideImgItem.Click += HideImg;
            displayBox.Click += Display;
            convertItem.Click += ShowConvert;
            DragEnter += DragEnterInput;
            DragDrop += DragDropInput;
            undoItem.Click += (o, e) => Controller.Move(-1);
            redoItem.Click += (o, e) => Controller.Move(1);
            closeButton.Click += CloseFile;
            historyButton.Click += ShowHistory;
            scaleBox.ValueChanged += Flush;
            scaleBox.Increment = 30;
            sortItem.Click += Sort;
            classifyItem.CheckedChanged += Classify;
            displayRuleCrossHairItem.Click += Flush;
            displayRuleItem.Click += Flush;
            adjustRuleItem.Click += AjustRule;
            openButton.Click += AddFile;
            pathBox.TextChanged += ChangePath;
            pathBox.Click += SelectPath;
            openFileItem.Click += AddFile;
            saveFileItem.Click += (o, e) => base.Connector.Save();
            cavasImageItem.Click += CavasImage;
            uncavasImageItem.Click += UnCavasImage;
            lockRuleItem.Click += LockRule;
            gridItem.Click += Flush;
            linedodgeBox.CheckedChanged += Flush;
            mutipleLayerItem.CheckedChanged += Flush;
            replaceLayerItem.Click += ReplaceLayer;
            layerList.ItemCheck += HideLayer;
            layerList.Cleared += ClearLayer;
            layerList.Deleted += DeleteLayer;
            renameLayerItem.Click += RenameLayer;
            addLayerItem.Click += AddLayer;
            adjustEntityPositionItem.Click += AdjustPosition;
            adjustPositionItem.Click += AjsutPostion;
            repairFileItem.Click += (o, e) => Controller.Do("repairFile", base.Connector.CheckedFiles);
            Drawer.BrushChanged += (o, e) => box.Cursor = e.Brush.Cursor;
            onionskinBox.Click += Flush;
            previewItem.CheckedChanged += PreviewChanged;
            trackBar.ValueChanged += TabLayer;
            Drawer.ColorChanged += ColorChanged;
            colorPanel.MouseClick += ColorChanged;
            lineDodgeItem.Click += LineDodge;
            splitFileItem.Click += (o, e) => Controller.Do("splitFile", base.Connector.CheckedFiles);
            mixFileItem.Click += (o, e) => Controller.Do("mixFile", base.Connector.CheckedFiles);
            cutImageItem.Click += CutImage;
            copyImageItem.Click += CutImage;
            pasteImageItem.Click += PasteImage;
            cutImgItem.Click += CutImg;
            copyImgItem.Click += CutImg;
            pasteImgItem.Click += PasteImg;
        }
        
        /// <summary>
        /// 粘贴img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteImg(object sender, EventArgs e) {
            var index = base.Connector.SelectedFileIndex;
            index = index < 0 ? base.Connector.FileCount : index;
            Controller.Do("pasteImg", index);
        }

        /// <summary>
        /// 复制/剪切img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutImg(object sender, EventArgs e) {
            var mode = ClipMode.Copy;
            if (sender.Equals(cutImgItem)) {
                mode = ClipMode.Cut;
            }
            Controller.Do("cutImg", base.Connector.CheckedFiles, mode);
        }


        /// <summary>
        /// 复制/剪切图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutImage(object sender, EventArgs e) {
            var mode = ClipMode.Copy;
            if (sender.Equals(cutImageItem)) {
                mode = ClipMode.Cut;
            }
            var al = base.Connector.SelectedFile;
            if (al != null) {
                var indexes = base.Connector.CheckedImages;
                Controller.Do("cutImage", al, indexes, mode);
            }
        }

        /// <summary>
        /// 粘贴图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteImage(object sender, EventArgs e) {
            var al = base.Connector.SelectedFile;
            if (al != null) {
                var index = base.Connector.SelectedImageIndex;
                index = index < 0 ? base.Connector.ImageCount : index;
                Controller.Do("pasteImage", al, index);
            }
        }

        /// <summary>
        /// 线性减淡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineDodge(object sender, EventArgs e) {
            var arr = base.Connector.CheckedImages;
            if (arr.Length > 0) {
                Controller.Do("lineDodge", arr);
            }
        }

        /// <summary>
        /// 当绘制器的颜色发生改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorChanged(object sender, ColorEventArgs e) {
            colorPanel.BackColor = e.NewColor;
        }

        /// <summary>
        /// 点击颜色选择框切换颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorChanged(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                Drawer.Color = Color.Empty;
                return;
            }
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                Drawer.Color = colorDialog.Color;
            }
        }

        private void ImageChanged(object sender, EventArgs e) {
            Drawer.OnImageChanged(new ImageEntityEventArgs {
                Entity = base.Connector.SelectedImage,
                Album = base.Connector.SelectedFile
            });
            ImageFlush(true);
        }

        private void PreviewChanged(object sender, EventArgs e) {
            Config["Preview"] = new ConfigValue(previewItem.Checked);
            previewPanel.Visible = previewItem.Checked;
        }

        private void PreviewHover(object sender, ItemHoverEventArgs e) {
            var entity = e.Item as ImageEntity;
            if (previewItem.Checked && entity != null) {
                previewPanel.BackgroundImage = entity.Picture;
                previewPanel.Visible = true;
            }
        }


        private void DeleteLayer() {
            var array = layerList.CheckedItems;
            foreach (var item in array) {
                Drawer.LayerList.Remove(item);
                layerList.Items.Remove(item);
            }
        }

        private void ClearLayer() {
            base.Connector.List.Clear();
            layerList.Items.Clear();
        }

        private void AjsutPostion(object sender, EventArgs e) {
            var index = base.Connector.SelectedImageIndex;
            var item = base.Connector.SelectedFile;
            if (index > -1 && item != null && CurrentLayer.Location != item.List[index].Location) {
                Controller.Do("changePosition", item, new int[] { index }, new int[] { CurrentLayer.Location.X, CurrentLayer.Location.Y, 0, 0 }, new bool[] { true, true, false, false, false });
            }
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameLayer(object sender, EventArgs e) {
            if (mutipleLayerItem.Checked) {
                if (layerList.SelectedItem is Layer item) {
                    var dialog = new ESTextDialog();
                    dialog.InputText = item?.ToString();
                    dialog.Text = Language["Rename"];
                    if (dialog.Show() == DialogResult.OK) {
                        Controller.Do("renameLayer", item, dialog.InputText);
                    }
                }
            }
        }

        /// <summary>
        /// 加入图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLayer(object sender, EventArgs e) {
            var array = base.Connector.CheckedImages;
            if (array.Length > 0) {
                Drawer.AddLayer(array);
                layerList.Items.AddRange(Drawer.LayerList.ToArray());
                if (!mutipleLayerItem.Checked) {
                    mutipleLayerItem.Checked = true;
                } else {
                    CavasFlush();
                }
                Messager.ShowOperate("AddLayer");
            }
        }


        /// <summary>
        /// 校正坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdjustPosition(object sender, EventArgs e) {
            var array = layerList.CheckedItems;
            if (array.Length > 0) {
                foreach (var item in array) {
                    item.Adjust();
                }
                Messager.ShowOperate("AdjustPosition");
            }

        }

        /// <summary>
        /// 隐藏图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideLayer(object sender, EventArgs e) {
            if (mutipleLayerItem.Checked) {
                if (layerList.SelectedItem is Layer item) {
                    item.Visible = !item.Visible;
                }
                CavasFlush();
            }
        }

        /// <summary>
        /// 切换图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLayer(object sender, EventArgs e) {
            var value = trackBar.Value;
            if (value == trackBar.Maximum && value < Config["LayerMaximum"].Integer - 1) {
                trackBar.Maximum++;
            }
            Drawer.TabLayer(value);
            layerList.Clear();
            layerList.Items.AddRange(Drawer.LayerList.ToArray());
            CavasFlush();
        }

        /// <summary>
        /// 替换图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceLayer(object sender, EventArgs e) {
            var array = base.Connector.ImageArray;
            Drawer.ReplaceLayer(array);
            Messager.ShowOperate("ReplaceImage");
            CavasFlush();
        }



        private void LockRule(object sender, EventArgs e) {
            Rule.Locked = lockRuleItem.Checked;
        }


        /// <summary>
        /// 移动文件序列
        /// </summary>
        private void MoveFileIndex(object sender, ItemDragEventArgs<Album> e) {
            if (e.Index > -1 && base.Connector.FileCount > 0) {
                Controller.Do("moveFile", e.Index, e.Target);
                base.Connector.SelectedFileIndex = e.Target;
            }
        }


        /// <summary>
        /// 移动贴图序列
        /// </summary>
        private void MoveImageIndex(object sender, ItemDragEventArgs<ImageEntity> e) {
            var al = base.Connector.SelectedFile;
            if (al != null && e.Index > -1 && base.Connector.ImageCount> 0) {
                Controller.Do("moveImage", al, e.Index, e.Target);
                base.Connector.SelectedImageIndex = e.Target;
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (!base.Connector.IsSave) {
                var rs = MessageBox.Show(Language["SaveTips"], "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.Cancel) {
                    e.Cancel = true;
                    return;
                }
                if (rs == DialogResult.Yes) {
                    base.Connector.Save();
                    e.Cancel = !base.Connector.IsSave;
                }
                player.Close();
            }
            e.Cancel = false;
            base.OnFormClosing(e);
        }




        private void SelectPath(object sender, EventArgs e) {
            base.Connector.SelectPath();
        }

        private void ChangePath(object sender, EventArgs e) {
            pathBox.SelectionStart = pathBox.Text.Length;
        }


        private void AjustRule(object sender, EventArgs e) {
            Rule.Location = CurrentLayer.Location;
            Flush(sender, e);
        }

        private void SelectImageChanged(object sender, EventArgs e) {
            LastLayer = CurrentLayer;//图层更新
            CurrentLayer = new Canvas();
            if (realPostionBox.Checked && base.Connector.SelectedImage!= null) {
                var entity = base.Connector.SelectedImage;
                CurrentLayer.Location = entity.Location;
            }
            Flush(sender, e);
        }

        private void Sort(object sender, EventArgs e) {
            Controller.Do("sortImg");
        }

        private void Classify(object sender, EventArgs e) {
            ListFlush();
        }


        private void CavasImage(object sender, EventArgs e) {
            Viewer.Show("cavas", base.Connector.SelectedFile, base.Connector.CheckedImageIndices);
        }

        private void UnCavasImage(object sender, EventArgs e) {
            Controller.Do("uncavasImage", base.Connector.SelectedFile, base.Connector.CheckedImageIndices);
        }

        /// <summary>
        /// 列表刷新
        /// </summary>
        public void ListFlush() {
            var indices = albumList.CheckedIndices;
            var index = albumList.SelectedIndex;
            albumList.Items.Clear();
            var condition = searchBox.Text.Trim().Split(" ");
            var array = Tools.Find(base.Connector.List, condition);
            if (classifyItem.Checked) {
                var path = "";
                foreach (var al in array) {
                    var p = al.Path.Replace(al.Name, "");
                    if (p != path) {
                        path = p;
                        var sp = new Album();
                        sp.Path = "---------------分割线---------------";
                        albumList.Items.Add(sp);
                    }
                    albumList.Items.Add(al);
                }
            } else {
                albumList.Items.AddRange(array.ToArray());
            }
            foreach (var i in indices) {
                if (i.Between(0,albumList.Items.Count)) {
                    albumList.SetItemChecked(i, true);
                }
            }
            if (albumList.Items.Count > 0) {
                if (!index.Between(0, albumList.Items.Count)) {
                    index = Math.Min(index, albumList.Items.Count - 1);
                    index = Math.Max(index, 0);
                }
                albumList.SelectedIndex = index;
            }
        }



        private void AddOutMerge(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "img,npk文件|*.img;*.npk";
            if (dialog.ShowDialog() == DialogResult.OK) {
                var array = Tools.Load(dialog.FileNames).ToArray();
                Controller.Do("addMerge", array);
            }
        }


        private void ShowHistory(object sender, EventArgs e) {
            dropPanel.BringToFront();
            dropPanel.Visible = !dropPanel.Visible;
            dropPanel.Refresh();
        }



        private void CloseFile(object sender, EventArgs e) {
            albumList.Items.Clear();
            imageList.Items.Clear();
            Controller.Dispose();
            Viewer.Dispose();
            ImageFlush();
            pathBox.Text = string.Empty;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e) {
            if (ModifierKeys == Keys.Alt) {
                var i = scaleBox.Value + e.Delta / 2;
                i = i < scaleBox.Maximum ? i : scaleBox.Maximum;
                i = i > scaleBox.Minimum ? i : scaleBox.Minimum;
                scaleBox.Value = i;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData.HasFlag(Keys.Alt)) {
                box.Focus();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DragEnterInput(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void DragDropInput(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var args = e.Data.GetData(DataFormats.FileDrop, false) as string[];
                base.Connector.AddFile(false, args);
            } else if (e.Data.GetDataPresent(DataFormats.Serializable)) {
                (sender as Control)?.DoDragDrop(e.Data, e.Effect);
            }
        }


        private void ShowConvert(object sender, EventArgs e) {
            var array = base.Connector.CheckedFiles;
            if (array.Length > 0 && CheckOgg(array)) {
                Viewer.Show("convert", array);
            }
        }

        private bool CheckOgg(params Album[] args) {
            foreach (var al in args) {
                if (al.Version == Img_Version.OGG) {
                    Messager.ShowError("NotHandleFile");
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display(object sender, EventArgs e) {
            if (displayBox.Checked) {
                var thread = new Thread(Display);
                thread.IsBackground = true;
                thread.Name = "display";
                thread.Start();
            }
        }

        private void Display() {
            while (displayBox.Checked) {
                DisplayNext();
                Thread.Sleep(1000 / Config["FlashSpeed"].Integer);
            }
        }


        private delegate void InvokerCallBack();

        private void DisplayNext() {
            if (mutipleLayerItem.Checked) {
                if (trackBar.InvokeRequired) {
                    trackBar.Invoke(new InvokerCallBack(DisplayNext));
                    return;
                }
                var i = trackBar.Value + 1;
                trackBar.Value = i < Drawer.Count ? i : 0;
            } else {
                if (imageList.InvokeRequired) {
                    imageList.Invoke(new InvokerCallBack(DisplayNext));
                    return;
                }
                var i = base.Connector.SelectedImageIndex + 1;
                i = i < base.Connector.ImageCount ? i : 0;
                if (base.Connector.ImageCount > 0) {
                    base.Connector.SelectedImageIndex = i;
                }
            }
        }


        /// <summary>
        /// 隐藏勾选img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideImg(object sender, EventArgs e) {
            var list = base.Connector.CheckedFiles;
            if (list.Length > 0 && CheckOgg(list) && MessageBox.Show(Language["HideTips"], "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                Controller.Do("hideImg", list);
            }
        }

        /// <summary>
        /// 打开新建img窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>F
        private void ShowNewImgDialog(object sender, EventArgs e) {
            Viewer.Show("newImg", base.Connector.List.Count);
        }



        /// <summary>
        /// 替换img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceImg(object sender, EventArgs e) {
            var item = base.Connector.SelectedFile;
            if (item != null) {
                var dialog = new OpenFileDialog();
                dialog.Filter = "图片资源|*.img|音效资源|*.ogg;*.wav;*.mp3|全部文件|*.*";
                if (item.Version == Img_Version.OGG) {
                    dialog.FilterIndex = 2;
                } else if (item.Name.EndsWith(".img")) {
                    dialog.FilterIndex = 1;
                } else {
                    dialog.FilterIndex = 3;
                }
                if (dialog.ShowDialog() == DialogResult.OK) {
                    var list = new List<Album>(Tools.Load(dialog.FileName));
                    if (list.Count > 0) {
                        Controller.Do("replaceImg", item, list[0]);
                    }
                }
            }
        }

        /// <summary>
        /// img另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsImg(object sender, EventArgs e) {
            var array = base.Connector.CheckedFiles;
            if (array.Length == 1) {
                var dialog = new SaveFileDialog();
                dialog.FileName = array[0].Name;
                dialog.Filter = "img|*.img|ogg|*.ogg|mp3|*.mp3|wav|*.wav";
                dialog.FilterIndex = array[0].Version != Img_Version.OGG ? 1 : 2;
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Tools.SaveFile(dialog.FileName, array[0]);
                }
            } else if (array.Length > 1) {
                var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Tools.SaveDirectory(dialog.SelectedPath, array);
                }
            }
        }

        /// <summary>
        /// 删除img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteImg() {
            var indices = base.Connector.CheckedFileIndices;
            if (indices.Length > 0 && MessageBox.Show(Language["DeleteTips"], "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                Controller.Do("deleteImg", indices);
            }
        }

        /// <summary>
        /// 全部选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckAllImg(object sender, EventArgs e) {
            albumList.CheckAll();
        }

        /// <summary>
        /// 反向勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReverseCheckImg(object sender, EventArgs e) {
            albumList.ReverseCheck();
        }

        /// <summary>
        /// 全部选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckAllImage(object sender, EventArgs e) => imageList.CheckAll();

        /// <summary>
        /// 反向勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReverseCheckImage(object sender, EventArgs e) => imageList.ReverseCheck();


        /// <summary>
        /// img重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameImg(object sender, EventArgs e) {
            var album = base.Connector.SelectedFile;
            if (album != null) {
                var dialog = new ESTextDialog();
                dialog.InputText = album.Path;
                dialog.Text = Language["Rename"];
                if (dialog.Show() == DialogResult.OK) {
                    Controller.Do("renameImg", album, dialog.InputText);
                }
            }
        }



        /// <summary>
        /// 选择贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flush(object sender, EventArgs e) => CavasFlush();


        public void ImageFlush() => ImageFlush(false);

        /// <summary>
        /// 贴图列表刷新
        /// </summary>
        public void ImageFlush(bool clear) {
            var al = albumList.SelectedItem;            //记录当前所选img
            var index = imageList.SelectedIndex;        //记录当前选择贴图
            var indexes = imageList.CheckedIndices;
            if (al != null && al.Version == Img_Version.OGG) { //判断是否为ogg音频
                player.Play();
            } else {
                player.Visible = false;
                imageList.Items.Clear();
                if (al != null) {
                    imageList.Items.AddRange(al.List.ToArray());
                }
                //添加贴图
                index = (index > -1 && index < imageList.Items.Count) ? index : 0;
                if (!clear) {
                    foreach (var i in indexes) {
                        if (i > -1 && i < imageList.Items.Count) {
                            imageList.SetItemChecked(i, true);
                        }
                    }
                }
                if (imageList.Items.Count > 0) {
                    imageList.SelectedIndex = index;
                } else if (imageList.Items.Count == 0) {
                    CavasFlush();
                }
            }
        }




        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFile(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "图片资源|*.npk;*.img;|音效资源|*.mp3;*.wav;*.ogg";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK) {
                base.Connector.AddFile(!sender.Equals(addFileItem), dialog.FileNames);
            }
        }

        /// <summary>
        /// 读取文件夹(img)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputDirectory(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                base.Connector.AddFile(true, new string[] { dialog.SelectedPath });
            }
        }

        /// <summary>
        /// 存为npk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputFile(object sender, EventArgs e) {
            var dialog = new SaveFileDialog();
            dialog.Filter = "npk文件|*.npk";
            dialog.FileName = Path.GetName();
            if (dialog.ShowDialog() == DialogResult.OK) {
                base.Connector.Save(dialog.FileName);
            }
        }

        /// <summary>
        /// 存为文件夹(img)
        /// </summary>
        /// <param name="sender"></param>
        private void OutputDirectory(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                Tools.SaveDirectory(dialog.SelectedPath, base.Connector.List);
            }
        }

        private void AddMerge(object sender, EventArgs e) {
            var array = base.Connector.CheckedFiles;
            if (array.Length > 0 && CheckOgg(array)) {
                Controller.Do("addMerge", array);
            }
        }

        private void DisplayMerge(object sender, EventArgs e) {
            Viewer.Show("Merge", base.Connector.SelectedFile);
        }

        public void CavasFlush() => box.Invalidate();

        /// <summary>
        /// 画布刷新
        /// </summary>
        private void OnPainting(object sender, PaintEventArgs e) {
            var g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            var entity = Connector.SelectedImage;//获得当前选择的贴图
            var pos = CurrentLayer.Location;
            if (!mutipleLayerItem.Checked && entity?.Picture != null) {
                if (entity.Type == ColorBits.LINK && entity.Target != null) {
                    entity = entity.Target;
                }                                                   
                var pictrue = entity.Picture;
                var size = entity.Size.Star(ImageScale);
                if (linedodgeBox.Checked) {
                    pictrue = pictrue.LinearDodge();
                }
                if (onionskinBox.Checked) {
                    LastLayer?.Draw(g);
                }
                CurrentLayer.Tag = entity;
                CurrentLayer.Size = size;//校正当前图层的宽高
                CurrentLayer.Image = pictrue;//校正贴图
                CurrentLayer.Draw(g);//绘制贴图
            } else {//多图层模式
                Drawer.DrawLayer(g);
            }
            
            if (Rule.Visible) {//显示标尺        
                Rule.Tag = CurrentLayer.Location.Minus(Rule.Location);
                Rule.Size = box.Size;
                Rule.Draw(g);
            }

            if (Grid.Visible) {//显示网格
                Grid.Tag = Config["GridGap"].Integer;
                Grid.Size = box.Size;
                Grid.Draw(g);
            }
        }






        /// <summary>
        /// 是否选择到了图片上
        /// </summary>
        /// <returns></returns>
        private int IsSelctImage(Point p) {
            var entity = base.Connector.SelectedImage;
            if (mutipleLayerItem.Checked) {
                return Drawer.IndexOfLayer(p);
            }
            if (Rule.Visible && !Rule.Locked) {//是否在圆心上
                if (Rule.Contains(p)) {
                    return 1;
                }
            }
            if (entity != null && CurrentLayer.Contains(p)) {
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 鼠标左键单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                var point = e.Location;
                move_mode = IsSelctImage(point);
                Drawer.CusorLocation = point;
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e) {
            if (!Drawer.IsSelect("MoveTool")) {
                Drawer.Brush.Draw(CurrentLayer, e.Location, ImageScale);
            }
        }


        /// <summary>
        /// 鼠标左键单击释放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                move_mode = -1;
            }
        }

        /// <summary>
        /// 鼠标左键单击移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void OnMouseMove(object sender, MouseEventArgs e) {
            if (e.Button== MouseButtons.Left&&move_mode > -1) {
                var newPoint = e.Location;
                if (!Rule.Locked) {
                    Rule.Location = Rule.Location.Add(newPoint.Minus(Drawer.CusorLocation));
                }
                if (move_mode == 0) {
                    Drawer.Brush.Draw(CurrentLayer, newPoint, ImageScale);
                }else if(move_mode>1){
                    Drawer.Brush.Draw(Drawer.LayerList[move_mode - 2], newPoint, ImageScale);
                }
                Drawer.CusorLocation = e.Location;
                CavasFlush();
            }
        }



        /// <summary>
        /// 保存贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveImage(object sender, EventArgs e) {
            var indexes = base.Connector.CheckedImageIndices;
            var album = base.Connector.SelectedFile;
            if (album == null || indexes.Length < 1) {
                return;
            }
            Viewer.Show("saveImage", album, indexes);
        }

        private void SaveSingleImage(object sender, EventArgs e) {
            var album = base.Connector.SelectedFile;
            var index = base.Connector.SelectedImageIndex;
            if (album == null || index < 0) {
                return;
            }
            var dialog = new SaveFileDialog();
            dialog.FileName = album.Name.RemoveSuffix();
            dialog.Filter = "png|*.png|bmp|*.bmp|jpg|*.jpg";
            if (dialog.ShowDialog() == DialogResult.OK) {
                Controller.Do("saveImage", album, 0, new int[] { index }, dialog.FileName);
            }
        }

        private void SaveAllImage(object sender, EventArgs e) {
            var album = base.Connector.SelectedFile;
            if (album == null || album.List.Count < 1) {
                return;
            }
            var indexes = new int[album.List.Count];
            for (var i = 0; i < indexes.Length; i++) {
                indexes[i] = i;
            }
            Viewer.Show("saveImage", album, indexes);
        }


        /// <summary>
        /// 保存为gif
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveGif(object sender, EventArgs e) {
            var array = base.Connector.CheckedImages;
            if (array.Length < 1) {
                return;
            }
            var dialog = new SaveFileDialog();
            var name = base.Connector.SelectedFile.Name.RemoveSuffix(".");
            dialog.Filter = "gif动态图片|*.gif";
            dialog.FileName = name;
            if (dialog.ShowDialog() == DialogResult.OK) {
                FreeImage.WriteGif(dialog.FileName, array);
                Messager.ShowOperate("SaveGif");
            }
        }


        /// <summary>
        /// 替换贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceImage(object sender, EventArgs e) {
            var array = base.Connector.CheckedImages;
            if (array.Length > 0) {
                Viewer.Show("replace");
            }
            CavasFlush();
        }




        /// <summary>
        /// 将勾选贴图变成链接贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkImage(object sender, EventArgs e) {
            var indexes = base.Connector.CheckedImageIndices;
            if (indexes.Length < 1) {
                return;
            }
            var dialog = new ESTextDialog();
            dialog.CanEmpty = true;
            dialog.Text = Language["LinkImage"];
            if (dialog.Show() == DialogResult.OK) {
                var str = dialog.InputText;
                if (Regex.IsMatch(str, "^\\d")) {
                    Controller.Do("linkImage", base.Connector.SelectedFile, int.Parse(str), indexes);
                }
                CavasFlush();
            }
        }

        /// <summary>
        /// 删除贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteImage() {
            var indexes = base.Connector.CheckedImageIndices;
            var album = base.Connector.SelectedFile;
            if (album != null && indexes.Length > 0 && MessageBox.Show(Language["DeleteTips"], "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                Controller.Do("deleteImage", album, indexes);
            }
        }



        internal class MainConnector : IConnector {

            internal MainForm MainForm { get; set; }

            internal MainConnector() {
                SaveChanged += (o, e) => OnSaveChanged();
            }

            public Language Language => Language.Default;

            public IConfig Config => Program.Config;

            public List<Language> LanguageList => Language.List;

            public string SavePath {
                set => MainForm.pathBox.Text = value;
                get => MainForm.pathBox.Text;
            }

            public ImageEntity[] ImageArray => MainForm.imageList.AllItems;

            public ImageEntity SelectedImage => MainForm.imageList.SelectedItem;

            public ImageEntity[] CheckedImages => MainForm.imageList.CheckedItems;

            public int[] CheckedImageIndices => MainForm.imageList.CheckedIndices;

            public int ImageCount => MainForm.imageList.Items.Count;

            public int SelectedImageIndex {
                set {
                    MainForm.imageList.SelectedIndex = value;
                }
                get {
                    return MainForm.imageList.SelectedIndex;
                }
            }

            public Album[] FileArray => MainForm.albumList.AllItems;

            public Album[] CheckedFiles => MainForm.albumList.CheckedItems;

            public Album SelectedFile => MainForm.albumList.SelectedItem;

            public int[] CheckedFileIndices => MainForm.albumList.CheckedIndices;

            public int FileCount => MainForm.albumList.Items.Count;

            public int SelectedFileIndex {
                set {
                    MainForm.albumList.SelectedIndex = value;
                }
                get {
                    return MainForm.albumList.SelectedIndex;
                }
            }

            public List<Album> List => _list;

            public bool IsSave { set; get; } = true;

            private readonly List<Album> _list = new List<Album>();

            public event EventHandler SaveChanged;

            public void OnSaveChanged() {
                ImageListFlush();
                IsSave = false;
                if (Config["AutoSave"].Boolean) {
                    Save();
                }
            }

            public void CavasFlush() => MainForm.CavasFlush();

            public void ImageListFlush() => MainForm.ImageFlush();

            public void FileListFlush() => MainForm.ListFlush();

            public void AddFile(bool clear, params string[] args) {
                if (clear) {
                    SavePath = string.Empty;
                }
                if (SavePath.Length == 0) {
                    SavePath = args.Find(item => item.ToLower().EndsWith(".npk")) ?? string.Empty;
                }
                if (args.Length > 0) {
                    MainForm.Controller.Do("addImg", Tools.Load(args).ToArray(), clear);
                }
            }

            public void AddFile(bool clear, params Album[] array) {
                if (clear) {
                    List.Clear();
                }
                if (array.Length > 0) {
                    if (FileCount > 0) {
                        SelectedFileIndex = FileCount - 1;
                    }
                    List.AddRange(array);
                }
            }


            public void RemoveFile(params Album[] array) {
                foreach (var album in array) {
                    List.Remove(album);
                }
            }
            public void Save() {
                if (SavePath.Trim().Length == 0) {
                    SelectPath();
                }
                if (SavePath.Trim().Length == 0) {
                    return;
                }
                Save(SavePath);
            }

            public void Save(string file) {
                Tools.WriteNPK(file, List);
                IsSave = true;
            }



            public void SelectPath() {
                var dir = SavePath;
                var path = SavePath.GetName();
                if (path != string.Empty) {
                    dir = dir.Replace(path, "");
                }
                var dialog = new SaveFileDialog();
                dialog.InitialDirectory = dir;
                dialog.FileName = path;
                dialog.Filter = "npk文件|*.npk";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    SavePath = dialog.FileName;                 
                    OnSaveChanged();
                }

            }

            public void Do(string name,params object[] args) {
                MainForm.Controller.Do(name, args);
            }

            public void Draw(IPaint paint, Point location, decimal scale) {
                MainForm.Drawer.Brush.Draw(paint, location, scale);
            }
        }


    }
}
