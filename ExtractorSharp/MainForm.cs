using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ExtractorSharp.View;
using ExtractorSharp.UI;
using ExtractorSharp.Handle;
using ExtractorSharp.Properties;
using ExtractorSharp.Draw;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using System.Drawing.Drawing2D;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Draw.Paint;
using System.ComponentModel;
using ExtractorSharp.Data;

namespace ExtractorSharp {
    public partial class MainForm : EaseForm {


        private Image BackImage;
        private Color BackBoxColor = Color.DimGray;
        private Viewer Viewer { get; }
        private Drawer Drawer { get; }
        private Controller Controller { get; }
        private decimal ImageScale => scaleBox.Value / 100;
        private ImageEntity preview;
        private int Preview_Width { set; get; } = 200;
        private int Preview_Height { set; get; } = 200;
        public string Path {
            set => pathBox.Text = value;
            get => pathBox.Text;
        }

        /// <summary>
        /// 当前图层
        /// </summary>
        private IPaint CurrentLayer { set; get; } = new Cavas();
        /// <summary>
        /// 上一图层
        /// </summary>
        private IPaint LastLayer { set; get; }

        /// <summary>
        /// 标尺位置
        /// </summary>
        Point rule_Point = Point.Empty;
        /// <summary>
        /// 标尺真实位置
        /// </summary>
        Point rule_Real_Point = Point.Empty;
        int move_mode = -1;
        int rule_radius = 25;


        public MainForm() {
            InitializeComponent();
            Controller = Program.Controller;
            Viewer = Program.Viewer;
            Drawer = Program.Drawer;
            decryptPanel = new DecryptPanel();
            dropPanel = new DropPanel();
            player = new OggPlayer(Controller);
            Controls.Add(decryptPanel);
            Controls.Add(dropPanel);
            Controls.Add(player);
            player.BringToFront();
            decryptPanel.BringToFront();
            previewPanel.BringToFront();
            AddListenter();
            AddShow();
            AddBrush();
        }

        private void AddBrush() {
            foreach (var entry in Drawer.Brushes) {
                var item = new ToolStripMenuItem();
                item.Text = Language[entry.Key];
                item.Click += (o, e) => Drawer.Select(entry.Key);
                toolsMenu.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// 给不需要动态参数的窗口-菜单添加监听
        /// </summary>
        private void AddShow() {
            AddShow(aboutItem, "about");
            AddShow(debugItem, "debug", 1);
            AddShow(fitItem, "fit");
            AddShow(propertyItem, "property");
            AddShow(versionItem, "version");
            AddShow(otherSeverItem, "download");
        }

        public void AddShow(ToolStripMenuItem item, string name, params object[] args) => item.Click += (o, e) => Viewer.Show(name, args);

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
            addSpliceItem.Click += AddSplice;
            addOutsideSpliceItem.Click += AddOutSplice;
            runSpliceItem.Click += DisplaySplice;
            albumList.SelectedIndexChanged += ImageChanged;
            albumList.Deleted = DeleteImg;
            albumList.Draged = DragList;
            albumList.DragDrop += DragDropInput;
            box.Paint += Painting;
            box.MouseClick += (o, e) => Drawer.Brush.Draw(CurrentLayer, e.Location, ImageScale);
            box.MouseDown += OnMouseDown;
            box.MouseUp += OnMouseUp;
            box.MouseMove += OnMouseMove;
            box.MouseWheel += OnMouseWheel;
            saveImageItem.Click += SaveImage;
            saveSingleImageItem.Click += SaveSingleImage;
            saveAllImageItem.Click += SaveAllImage;
            saveGifItem.Click += SaveGif;
            replaceImageItem.Click += ReplaceImage;
            hideCheckImageItem.Click += (o, e) => Controller.Do("hideImage", Controller.SelectAlbum, Controller.CheckedIndex);
            linkImageItem.Click += LinkImage;
            imageList.Deleted = DeleteImage;
            imageList.Draged = DragImageList;
            imageList.SelectedIndexChanged += SelectImageChanged;
            imageList.ItemHoverChanged += PreviewHover;
            changeBackButton.Click += ReplaceBack;
            displayBackBox.SelectedIndexChanged += Flush;
            changePositionItem.Click += (o, e) => Viewer.Show("changePosition", Controller.CheckedImage);
            changeSizeItem.Click += (o, e) => Controller.Do("changeSize", Controller.SelectAlbum, Controller.CheckedIndex, ImageScale);
            searchBox.TextChanged += (o, e) => ListFlush();

            newImageItem.Click += (o, e) => Viewer.Show("newImage", Controller.SelectAlbum);
            realPostionBox.CheckedChanged += SelectImageChanged;
            newImgItem.Click += ShowNewImgDialog;
            hideImgItem.Click += HideImg;
            searchItem.Click += ShowSearch;
            displayBox.Click += Display;
            encryptItem.Click += ShowEncrypt;
            decryptPanel.readbutton.Click += (o, e) => ImageFlush();
            deleteEncryptItem.Click += DeleteEncrypt;
            convertItem.Click += ShowConvert;
            batItem.Click += ShowBatch;
            DragEnter += DragEnterInput;
            DragDrop += DragDropInput;
            undoItem.Click += (o, e) => Controller.Move(-1);
            redoItem.Click += (o, e) => Controller.Move(1);
            closeButton.Click += CloseFile;
            historyButton.Click += ShowHistory;
            clearItem.Click += ClearModel;
            scaleBox.ValueChanged += Flush;
            scaleBox.Increment = 30;
            sortItem.Click += Sort;
            classifyItem.CheckedChanged += Classify;
            displayRuleCrossHairItem.Click += Flush;
            displayRuleItem.Click += Flush;
            adjustRuleItem.Click += AjustRule;
            macroItem.Click += ShowMacro;
            openButton.Click += AddFile;
            pathBox.TextChanged += ChangePath;
            pathBox.Click += SelectPath;
            openFileItem.Click += AddFile;
            saveFileItem.Click += SaveFile;
            cavasImageItem.Click += CavasImage;
            uncavasImageItem.Click += UnCavasImage;
            lockRuleItem.Click += LockRule;
            gridItem.Click += Flush;
            linedodgeBox.CheckedChanged += Flush;
            loadModelItem.Click += LoadModel;
            mutipleLayerItem.CheckedChanged += Flush;
            replaceLayerItem.Click += ReplaceLayer;
            layerList.ItemCheck += HideLayer;
            layerList.Cleared += ClearLayer;
            layerList.Deleted += DeleteLayer;
            renameLayerItem.Click += RenameLayer;
            addLayerItem.Click += AddLayer;
            adjustEntityPositionItem.Click += AdjustPosition;
            saveAsLayerItem.Click += SaveAsModel;
            adjustPositionItem.Click += AjsutPostion;
            repairImgItem.Click += (o, e) => Controller.Do("repairImg", Controller.CheckedAlbum);
            Drawer.BrushChanged += (o, e) => box.Cursor = e.Brush.Cursor;
            onionskinBox.Click += Flush;
            previewItem.CheckedChanged += PreviewChanged;
            trackBar.ValueChanged += CheckLayerList;
            Drawer.ColorChanged += ColorChanged;
            colorPanel.Click += ColorChanged;
            lineDodgeItem.Click += LineDodge;
            splitImgItem.Click += (o, e) => Controller.Do("splitImg", Controller.CheckedAlbum);
            mixImgItem.Click += (o, e) => Controller.Do("mixImg", Controller.CheckedAlbum);
            cutImageItem.Click += CutImage;
            copyImageItem.Click += CutImage;
            pasteImageItem.Click += PasteImage;
            cutImgItem.Click += CutImg;
            copyImgItem.Click += CutImg;
            pasteImgItem.Click += PasteImg;
        }

        private void PasteImg(object sender, EventArgs e) {
            var index = albumList.SelectedIndex;
            index = index < 0 ? albumList.Items.Count : index;
            Controller.Do("pasteImg", index);
        }

        private void CutImg(object sender, EventArgs e) {
            var mode = ClipMode.Copy;
            if (sender.Equals(cutImgItem)) {
                mode = ClipMode.Cut;
            }
            var indexes = new int[albumList.CheckedIndices.Count];
            albumList.CheckedIndices.CopyTo(indexes, 0);
            var array = Controller.CheckedAlbum;
            Controller.Do("cutImg", array, indexes, mode);
        }



        private void CutImage(object sender, EventArgs e) {
            var mode = ClipMode.Copy;
            if (sender.Equals(cutImageItem)) {
                mode = ClipMode.Cut;
            }
            var al = Controller.SelectAlbum;
            var indexes = Controller.CheckedIndex;
            Controller.Do("cutImage", al, indexes, mode);
        }

        private void PasteImage(object sender, EventArgs e) {
            var al = Controller.SelectAlbum;
            var index = imageList.SelectedIndex;
            index = index < 0 ? imageList.Items.Count : index;
            Controller.Do("pasteImage", al, index);
        }

        private void LineDodge(object sender, EventArgs e) {
            var arr = Controller.CheckedImage;
            if (arr.Length > 0) {
                Controller.Do("lineDodge", arr);
            }
        }


        private void ColorChanged(object sender, ColorEventArgs e) {
            colorPanel.BackColor = e.NewColor;
        }

        private void ColorChanged(object sender, EventArgs e) {
            var dialog = new ColorDialog();
            dialog.Color = Drawer.Color;
            if (dialog.ShowDialog() == DialogResult.OK) {
                Drawer.Color = dialog.Color;
            }
        }

        private void ImageChanged(object sender, EventArgs e) {
            var a = new ImageEntityEventArgs();
            a.Entity = Controller.SelectImage;
            a.Album = Controller.SelectAlbum;
            Drawer.OnImageChanged(a);
            ImageFlush(true);
        }

        private void PreviewChanged(object sender, EventArgs e) {
            ViewConfig["Preview"] = new ConfigValue(previewItem.Checked);
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
            var array = layerList.GetCheckItems();
            foreach (var item in array) {
                Drawer.LayerList.Remove(item);
                layerList.Items.Remove(item);
            }
        }

        private void ClearLayer() {
            Controller.List.Clear();
            layerList.Items.Clear();
        }

        private void AjsutPostion(object sender, EventArgs e) {
            var index = imageList.SelectedIndex;
            var Album = Controller.SelectAlbum;
            if (index > -1 && Album != null && CurrentLayer.Location != Album.List[index].Location) {
                Controller.Do("changePosition", Album, new int[] { index }, new int[] { CurrentLayer.Location.X, CurrentLayer.Location.Y, 0, 0 }, new bool[] { true, true, false, false, false });
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
                    var dialog = new EaseTextDialog();
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
            var array = Controller.CheckedImage;
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
            var array = layerList.GetCheckItems();
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

        private void CheckLayerList(object sender, EventArgs e) {
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
            var array = Controller.AllImage;
            Drawer.ReplaceLayer(array);
            Messager.ShowOperate("ReplaceImage");
            CavasFlush();
        }

        /// <summary>
        /// 载入模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadModel(object sender, EventArgs e) {
            var dialog = new OpenFileDialog() {
                Filter = "图层模板|*.md",
                InitialDirectory = Application.StartupPath + "/model/",
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                Drawer.LayerList.Clear();
                layerList.Items.Clear();
                var array = Tools.ReadModel(dialog.FileName).ToArray();
                Drawer.LayerList.AddRange(array);
                foreach (var item in array)
                    layerList.Items.Add(item, item.Visible);
                if (!mutipleLayerItem.Checked) {
                    mutipleLayerItem.Checked = true;
                } else {
                    CavasFlush();
                }
            }
        }

        private void SaveAsModel(object sender, EventArgs e) {
            var dialog = new SaveFileDialog() {
                Filter = "md模板|*.md",
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                Tools.SaveModel(dialog.FileName, Drawer.LayerList);
            }
        }


        private void LockRule(object sender, EventArgs e) {
            if (!lockRuleItem.Checked) {
                rule_Point = rule_Real_Point.Minus(CurrentLayer.Location);
            }
        } 



        private void DragList() {
            var item = albumList.DragItem as Album;
            var target = albumList.DragTarget;
            if (item != null && albumList.Items.Count > 0) {
                Controller.List.Remove(item);
                Controller.List.Insert(target, item);
            }
        }

        private void DragImageList() {
            var al = Controller.SelectAlbum;
            var item = imageList.DragItem as ImageEntity;
            var target = imageList.DragTarget;
            if (al != null && item != null && albumList.Items.Count > 0) {
                al.List.Remove(item);
                al.List.Insert(target, item);
                al.AdjustIndex();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (!Controller.isSave) {
                var rs = MessageBox.Show(Language["SaveTips"], "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes) {
                    SaveFile(this, e);
                    e.Cancel = !Controller.isSave;
                } else if (rs != DialogResult.No)
                    e.Cancel = true;
                player.Close();
            }
        }

   


        private void SelectPath(object sender, EventArgs e) {
            Controller.SelectPath();
        }

        private void ChangePath(object sender, EventArgs e) {
            pathBox.SelectionStart = pathBox.Text.Length;
        }

        private void ShowMacro(object sender, EventArgs e) {
            Viewer.Show("macro");
        }

        private void AjustRule(object sender, EventArgs e) {
            rule_Point = Point.Empty;
            Flush(sender, e);
        }

        private void SelectImageChanged(object sender, EventArgs e) {
            LastLayer = CurrentLayer;//图层更新
            CurrentLayer = new Cavas();
            if (realPostionBox.Checked && Controller.SelectImage != null) {
                var entity = Controller.SelectImage;
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
            Viewer.Show("cavas", Controller.SelectAlbum, Controller.CheckedIndex);
        }

        private void UnCavasImage(object sender, EventArgs e) {
            Controller.Do("uncavasImage", Controller.SelectAlbum, Controller.CheckedIndex);
        }

        /// <summary>
        /// 列表刷新
        /// </summary>
        public void ListFlush() {
            var cs = Controller.CheckedAlbum;
            var select = Controller.SelectAlbum;
            albumList.Items.Clear();
            var condition = searchBox.Text.Trim().Split(" ");
            var array = Tools.Find(Controller.List ,condition);
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
            } else
                albumList.Items.AddRange(new List<Album>(array).ToArray());
            foreach (var entity in cs) {
                var i = albumList.Items.IndexOf(entity);
                if (i > -1)
                    albumList.SetItemChecked(i, true);
            }
            if (select != null && albumList.Items.Contains(select))
                albumList.SelectedItem = select;
        }



        private void AddOutSplice(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "img,NPK文件|*.img;*.NPK";
            if (dialog.ShowDialog() == DialogResult.OK) {
                var array = new List<Album>(Tools.Load(dialog.FileNames)).ToArray();
                Controller.Do("addSplice", array);
            }
        }

        private void ClearModel(object sender, EventArgs e) {
            Viewer.Show("clear");
        }

        private void ShowHistory(object sender, EventArgs e) {
            dropPanel.BringToFront();
            dropPanel.Visible = !dropPanel.Visible;
            dropPanel.Refresh();
        }

        

        private void CloseFile(object sender, EventArgs e) {
            albumList.Items.Clear();
            imageList.Items.Clear();
            Controller.Close();
            Viewer.Dispose();
            ImageFlush();
            pathBox.Text = string.Empty;
            GC.Collect();
        }

        private void OnMouseWheel(object sender,MouseEventArgs e) {
            if (ModifierKeys == Keys.Alt) {
                var i = scaleBox.Value + e.Delta/2;
                i = i < scaleBox.Maximum ? i : scaleBox.Maximum;
                i = i > scaleBox.Minimum ? i : scaleBox.Minimum;
                scaleBox.Value = i;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData.HasFlag(Keys.Alt)) {
                box.Focus();
            }
            return base.ProcessCmdKey(ref msg,keyData);
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
                Controller.AddAlbum(false, args);
            } else if (e.Data.GetDataPresent(DataFormats.Serializable))
                (sender as System.Windows.Forms.Control)?.DoDragDrop(e.Data, e.Effect);
        }



        private void ShowBatch(object sender, EventArgs e) {
            Viewer.Show("batch");
        }


        private void ShowConvert(object sender, EventArgs e) {
            var array = Controller.CheckedAlbum;
            if (array.Length > 0 && CheckOgg(array))
                Viewer.Show("convert", array);
        }

        private void ShowEncrypt(object sender, EventArgs e) {
            var array = Controller.CheckedAlbum;
            if (array.Length > 0 && CheckOgg(array))
                Viewer.Show("encrypt", array[0].Work);
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


        private void DeleteEncrypt(object sender, EventArgs e) {
            var album = Controller.SelectAlbum;
            if (album != null) {
                if (MessageBox.Show(Language["DeletePasswordTips"], "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                    album.DeleteKeyword();
                    Messager.ShowOperate("DeletePassword");
                }
            }
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
                var i = imageList.SelectedIndex + 1;
                i = i < imageList.Items.Count ? i : 0;
                if (imageList.Items.Count > 0)
                    imageList.SelectedIndex = i;
            }
        }



        private void ShowSearch(object sender, EventArgs e) {
            Viewer.Show("search");
        }

        /// <summary>
        /// 隐藏勾选img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideImg(object sender, EventArgs e) {
            var list = Controller.CheckedAlbum;
            if (list.Length > 0 && CheckOgg(list) && MessageBox.Show(Language["HideTips"], "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                Controller.Do("hideImg", list);
        }

        /// <summary>
        /// 打开新建img窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>F
        private void ShowNewImgDialog(object sender, EventArgs e) {
            Viewer.Show("newImg", Controller.List.Count);
        }



        /// <summary>
        /// 替换img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceImg(object sender, EventArgs e) {
            var item = Controller.SelectAlbum;
            if (item != null) {
                var dialog = new OpenFileDialog();
                dialog.Filter = "图片资源|*.img|音效资源|*.ogg;*.wav;*.mp3|全部文件|*.*";
                if (item.Version == Img_Version.OGG)
                    dialog.FilterIndex = 2;
                else if (item.Name.EndsWith(".img"))
                    dialog.FilterIndex = 1;
                else
                    dialog.FilterIndex = 3;
                if (dialog.ShowDialog() == DialogResult.OK) {
                    var list = new List<Album>(Tools.Load(dialog.FileName));
                    if (list.Count > 0)
                        Controller.Do("replaceImg", item, list[0]);
                }
            }
        }

        /// <summary>
        /// img另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsImg(object sender, EventArgs e) {
            var array = Controller.CheckedAlbum;
            if (array.Length == 1) {
                var dialog = new SaveFileDialog();
                dialog.FileName = array[0].Name;
                dialog.Filter = array[0].Version == Img_Version.OGG ? "音效文件|*.ogg;*.mp3;*.wav" : "img文件|*.img";
                if (dialog.ShowDialog() == DialogResult.OK)
                    Tools.SaveFile(dialog.FileName, array[0]);
            } else if (array.Length > 1) {
                var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    Tools.SaveDirectory(dialog.SelectedPath, array);
            }
        }

        /// <summary>
        /// 删除img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteImg() {
            var list = Controller.CheckedAlbum;
            if (list.Length > 0 && MessageBox.Show(Language["DeleteTips"],"", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
                Controller.Do("deleteImg", list);          
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
            var album = Controller.SelectAlbum;      
            if (album != null) {
                var dialog = new EaseTextDialog();
                dialog.InputText = album.Path;
                dialog.Text = Language["Rename"];
                if (dialog.Show() == DialogResult.OK) 
                    Controller.Do("renameImg", album, dialog.InputText);                
            }
        }



        /// <summary>
        /// 选择贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flush(object sender, EventArgs e) => CavasFlush();

        /// <summary>
        /// 显示解密
        /// </summary>
        /// <param name="album"></param>
        public void ShowDecrypt(Album album) {
            var Work = album.Work;
            if (Work != null && !Work.IsDecrypt) {
                decryptPanel.Show(album.Work);
                return;
            }
        }

        public void ImageFlush() => ImageFlush(false);

        /// <summary>
        /// 贴图列表刷新
        /// </summary>
        public void ImageFlush(bool clear) {
            var al = albumList.SelectedItem as Album;            //记录当前所选img
            var index = imageList.SelectedIndex;        //记录当前选择贴图
            var indexes = new int[imageList.CheckedIndices.Count]; //记录勾选项
            imageList.CheckedIndices.CopyTo(indexes, 0);
            if (al != null && al.Version == Img_Version.OGG) { //判断是否为ogg音频
                player.Play();
            } else {
                player.Visible = false;
                imageList.Items.Clear();
                decryptPanel.Visible = false;
                if (al != null && Controller.CheckEncrypt(al)) {
                    imageList.Items.AddRange(al.List.ToArray());
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
                    }

                    if (imageList.Items.Count == 0) {
                        CavasFlush();
                    }
                }
            }
        }
        

        private void SaveFile(object sender, EventArgs e) {
            Controller.SaveFile();
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFile(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "图片资源|*.NPK;*.img;|音效资源|*.mp3;*.wav;*.ogg";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
                Controller.AddAlbum(!sender.Equals(addFileItem), dialog.FileNames);
        }

        /// <summary>
        /// 读取文件夹(img)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputDirectory(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                Controller.AddAlbum(true, dialog.SelectedPath);
        }

        /// <summary>
        /// 存为npk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputFile(object sender, EventArgs e) { 
            var dialog = new SaveFileDialog();
            dialog.Filter = "NPK文件|*.NPK";
            dialog.FileName = Path.GetName();
            if (dialog.ShowDialog() == DialogResult.OK) 
                Controller.WriteNPK(dialog.FileName);         
        }

        /// <summary>
        /// 存为文件夹(img)
        /// </summary>
        /// <param name="sender"></param>
        private void OutputDirectory(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                Controller.WriteToDirectory(dialog.SelectedPath);
        }

        private void AddSplice(object sender, EventArgs e) {
            var array = Controller.CheckedAlbum;
            if (array.Length > 0&&CheckOgg(array))
                Controller.Do("addSplice", array);
        }

        private void DisplaySplice(object sender, EventArgs e) {
            Viewer.Show("splice", Controller.SelectAlbum);
        }

        public void CavasFlush() => box.Invalidate();

        /// <summary>
        /// 画布刷新
        /// </summary>
        private void Painting(object sender, PaintEventArgs e) {
            var g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            if (displayBackBox.SelectedIndex == 0)
                box.BackColor = BackBoxColor;
            else if (displayBackBox.SelectedIndex == 1 && BackImage != null)
                box.BackgroundImage = BackImage;
            var entity = Controller.SelectImage;//获得当前选择的贴图
            var pos = CurrentLayer.Location;
            if (!mutipleLayerItem.Checked && entity?.Picture != null) {
                if (entity.Type == ColorBits.LINK && entity.Target != null)
                    entity = entity.Target;
                var pictrue = entity.Picture;
                if (pictrue == null)
                    return;
                var size = entity.Size.Star(ImageScale);
                if (linedodgeBox.Checked)
                    pictrue = pictrue.LinearDodge();
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
            if (displayRuleItem.Checked) {//显示标尺
                if (!lockRuleItem.Checked)
                    rule_Real_Point = rule_Point.Add(pos);
            //    rule_Point = rule_Real_Point.Minus(pos);
                var rp = rule_Real_Point;
                g.DrawString(Language["AbsolutePosition"] + ":" + rp.GetString(), DefaultFont, Brushes.White, new Point(rp.X + rule_radius, rp.Y - rule_radius - DefaultFont.Height));
                g.DrawString(Language["RealativePosition"] + ":" + rule_Point.Reverse().GetString(), DefaultFont, Brushes.White, new Point(rp.X + rule_radius, rp.Y - rule_radius - DefaultFont.Height * 2));
                g.DrawLine(Pens.White, new Point(rp.X, 0), new Point(rp.X, box.Height));
                g.DrawLine(Pens.White, new Point(0, rp.Y), new Point(box.Width, rp.Y));
                if (displayRuleCrossHairItem.Checked) {
                    var x = rp.X - rule_radius;
                    var y = rp.Y - rule_radius;
                    g.DrawEllipse(Pens.WhiteSmoke, x, y, rule_radius * 2, rule_radius * 2);
                }
            }
            if (gridItem.Checked) {//显示网格
                var grap = Config["GridGap"].Integer;
                for (var i = 0; i < box.Width || i < box.Height; i += grap) {
                    if (i < box.Width)
                        g.DrawLine(Pens.White, new Point(i, 0), new Point(i, box.Height));
                    if (i < box.Height)
                        g.DrawLine(Pens.White, new Point(0, i), new Point(box.Width, i));
                }
            }
        }


        /// <summary>
        /// 替换背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceBack(object sender, EventArgs e) {
            if (displayBackBox.SelectedIndex == 0) {
                var dialog = new ColorDialog();
                if (dialog.ShowDialog() == DialogResult.OK) {
                    BackBoxColor = dialog.Color;
                    CavasFlush();
                }
            } else if (displayBackBox.SelectedIndex == 1) {
                var dialog = new OpenFileDialog();
                dialog.Filter = "png文件,jpg文件|*.png;*.jpg;*.jpeg";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    BackImage = new Bitmap(dialog.FileName);
                    CavasFlush();
                }
            }
        }


     


        /// <summary>
        /// 是否选择到了图片上
        /// </summary>
        /// <returns></returns>
        private int IsSelctImage() {
            var entity = Controller.SelectImage;
            var p = box.PointToClient(Cursor.Position);
            var rect = Rectangle.Empty;
            if (mutipleLayerItem.Checked)
                return Drawer.IndexOfLayer(p);
            if (displayRuleItem.Checked && displayRuleCrossHairItem.Checked && !lockRuleItem.Checked) {//是否在圆心上
                var rp = CurrentLayer.Location.Add(rule_Point).Minus(p);
                if ((rp.X * rp.X + rp.Y * rp.Y) < rule_radius * rule_radius) {
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
                move_mode = IsSelctImage();
                Drawer.CusorLocation = e.Location;
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
            if (move_mode > -1) {
                 var newPoint = e.Location;
                if (move_mode == 0) {
                    Drawer.Brush.Draw(CurrentLayer, newPoint,ImageScale);
                } else if (move_mode == 1) {
                    rule_Point = rule_Point.Add(newPoint.Minus(Drawer.CusorLocation));
                } else {
                    Drawer.Brush.Draw(Drawer.LayerList[move_mode - 2], newPoint,ImageScale);
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
            var indexes = Controller.CheckedIndex;
            var album = Controller.SelectAlbum;
            if (album == null || indexes.Length < 1)
                return;
            Viewer.Show("saveImage", album, indexes);
        }

        private void SaveSingleImage(object sender, EventArgs e) {
            var dialog = new SaveFileDialog();
            dialog.Filter = "*.png|*.png;*.bmp;*.jpg";
            var album = Controller.SelectAlbum;
            var index = imageList.SelectedIndex;
            if (album == null || index < 0) {
                return;
            }
            if (dialog.ShowDialog() == DialogResult.OK) {
                Controller.Do("saveImage", album, 0, new int[] { index }, dialog.FileName);
            }
        }

        private void SaveAllImage(object sender, EventArgs e) {
            var album = Controller.SelectAlbum;
            if (album == null && album.List.Count < 1)
                return;
            var indexes = new int[album.List.Count];
            for (var i = 0; i < indexes.Length; i++)
                indexes[i] = i;
            Viewer.Show("saveImage", album, indexes);
        }


        /// <summary>
        /// 保存为gif
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveGif(object sender, EventArgs e) {
            var array = Controller.CheckedImage;
            if (array.Length < 1) {
                return;
            }
            var dialog = new SaveFileDialog();
            var name = Controller.SelectAlbum.Name.RemoveSuffix(".");
            dialog.Filter = "gif动态图片|*.gif";
            dialog.FileName = name;
            if (dialog.ShowDialog() == DialogResult.OK) {
                Tools.SaveGif(dialog.FileName,array);
                Messager.ShowOperate("SaveGif");
            }
        }

       
        /// <summary>
        /// 替换贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceImage(object sender, EventArgs e) {
            var array = Controller.CheckedImage;
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
            var indexes = Controller.CheckedIndex;
            if (indexes.Length < 1)
                return;
            var dialog = new EaseTextDialog();
            dialog.CanEmpty = true;
            dialog.Text = Language["LinkImage"];
            if (dialog.Show()==DialogResult.OK) {
                var str = dialog.InputText;
                if (Regex.IsMatch(str, "^\\d")) {
                    Controller.Do("linkImage", Controller.SelectAlbum, int.Parse(str), indexes);
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
            var indexes = Controller.CheckedIndex;
            if (indexes.Length > 0 && MessageBox.Show(Language["DeleteTips"], "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                Controller.Do("deleteImage", Controller.SelectAlbum, indexes);
            }
        }




    }
}
