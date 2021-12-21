using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Draw.Paint;
using ExtractorSharp.EventArguments;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Exceptions;
using ExtractorSharp.Services.Constants;
using ExtractorSharp.View.Pane;

namespace ExtractorSharp {

    [Export]
    public partial class MainForm : BaseForm, IPartImportsSatisfiedNotification {

        private int move_mode = -1;

        private string Extensions { get; } = "*.img;*.npk";

        private List<Album> ReplaceStack = new List<Album>();

        private Album ExchangeSelectedFile { set; get; }

        [Import]
        private Language Language;

        [Import]
        private DropPanel dropPanel;

        public void OnImportsSatisfied() {
            InitializeComponent();
            player = new AudioPlayer();
            Controls.Add(dropPanel);
            Controls.Add(player);
            player.BringToFront();
            previewPanel.BringToFront();
            messagePanel.BringToFront();
            AddStore();
            AddListenter();
            AddShow();
            AddBrush();
            AddPaint();
            AddConfig();
            AddMenuItem();
            //LayerFlush();
        }

        [Import]
        private IConfig Config;

        [Import]
        private IClipboad Clipboad;

        [Import]
        private Drawer Drawer;

        [Import]
        private Viewer Viewer;

        [Import]
        private Messager Messager;

        [Import]
        private Controller Controller;

        [Import]
        private Store Store;

        [Import]
        private ICommonMessageBox CommonMessageBox;

        [ImportMany(typeof(IMenuItem))]
        private List<IMenuItem> MenuItems = new List<IMenuItem>();



        private Point Hotpot { set; get; }

        private IPaint Ruler { set; get; }

        private IPaint Grid { set; get; }

        private IPaint Border { set; get; }

        private KeysConverter KeysConverter = new KeysConverter();

        private void AddConfig() {
            linearDodgeBox.Checked = Config["LinearDodge"].Boolean;
            relativePositionCheckedBox.Checked = Config["RealPosition"].Boolean;
            scaleBox.Value = Config["CanvasScale"].Integer;
            dyeBox.Checked = Config["Dye"].Boolean;
            displayBox.Checked = Config["Animation"].Boolean;
            Ruler.Visible = Config["Ruler"].Boolean;
        }

        private void AddBrush() {
            Drawer.Select(Config["Brush"].Value);
            Drawer.Color = Config["BrushColor"].Color;

            foreach(var entry in Drawer.Brushes) {
                var item = new ToolStripMenuItem(Language[entry.Name]) {
                    CheckOnClick = true
                };
                if(Drawer.IsSelect(entry.Name)) {
                    item.Checked = true;
                }
                item.Click += (o, e) => {
                    Drawer.Select(entry.Name);
                    foreach(ToolStripMenuItem i in toolsMenu.DropDownItems) {
                        i.Checked = false;
                    }
                    item.Checked = true;
                };
                toolsMenu.DropDownItems.Add(item);
            }
        }

        private void AddPaint() {
            Ruler = new Ruler();
            Grid = new Grid();
            Border = new Border();
            AddPaint(displayRuleItem, Ruler);
            AddPaint(gridItem, Grid);
            AddPaint(borderItem, Border);
        }

        private void AddPaint(ToolStripMenuItem item, IPaint paint) {
            paint.Visible = item.Checked;
            item.CheckOnClick = true;
            item.Click += Flush;
            item.CheckedChanged += (o, e) => {
                paint.Visible = item.Checked;
                Store.Use<List<IPaint>>("/draw/layers", layers => {
                    if(item.Checked && !layers.Contains(paint)) {
                        layers.Add(paint);
                    } else if(!item.Checked && layers.Contains(paint)) {
                        layers.Remove(paint);
                    }
                    return layers;
                });
            };
        }


        /// <summary>
        ///     给不需要动态参数的窗口-菜单添加监听
        /// </summary>
        private void AddShow() {
            AddShow(aboutItem, "about");
            AddShow(feedbackItem, "debug", "feedback");
            AddShow(settingItem, "setting");
        }

        public void AddShow(ToolStripMenuItem item, string name, params object[] args) {
            item.Click += (o, e) => Viewer.Show(name, args);
        }


        public void AddCommand(Control control, string name) {
            control.Click += (o, e) => Controller.Do(name);
        }

        public void AddCommand(ToolStripItem control, string name) {
            control.Click += (o, e) => Controller.Do(name);
        }

        private void AddMenuItem() {

            var tree = MenuItemTools.CreateMenuTree(MenuItems);

            var mainItems = mainMenu.Items;
            foreach(var item in tree) {
                ToolStripMenuItem menuItem = null;

                AddChildren(mainMenu.Items, item);

            }
        }

        private void AddChildren(ToolStripItemCollection items, IMenuItem item) {
            var menuItem = FindMenuItem(items, item.Key);
            var child = item as IChildrenItem;

            if(menuItem == null) {
                menuItem = new ToolStripMenuItem {
                    Name = item.Key,
                    Text = Language[item.Key]
                };
                if(child == null || !child.IsTile) {
                    items.Add(menuItem);
                }
                if(child.IsTile) {
                    items.AddSeparator();
                }
            }
            if(item is IRoute route) {
                menuItem.Click += route.Execute;
                if(!string.IsNullOrEmpty(route.ShortCutKey)) {
                    menuItem.ShortcutKeys = (Keys)KeysConverter.ConvertFromString(route.ShortCutKey);
                }
            }
            if(child != null) {
                if(!child.IsTile) {
                    items = menuItem.DropDownItems;
                }
                child.Children.ForEach(c => AddChildren(items, c));
            }
        }

        private ToolStripMenuItem FindMenuItem(ToolStripItemCollection items, string name) {
            foreach(object item in items) {
                if(item is ToolStripMenuItem menuItem) {
                    if(menuItem.Name == name || menuItem.Text == Language[name]) {
                        return menuItem;
                    }
                }
            }
            return null;
        }



        private void AddStore() {


            Store
                .Compute<int>(StoreKeys.SELECTED_FILE_INDEX, () => fileList.SelectedIndex, value => fileList.SelectedIndex = value)
                .Compute<int>(StoreKeys.SELECTED_IMAGE_INDEX, () => imageList.SelectedIndex, value => imageList.SelectedIndex = value)
                .Compute<bool>("/checkbox/real-poisition", () => relativePositionCheckedBox.Checked, value => relativePositionCheckedBox.Checked = value)
                .Compute<Album>(StoreKeys.SELECTED_FILE, () => fileList.SelectedItem, value => fileList.SelectedItem = value)
                .Compute(StoreKeys.SELECTED_FILE_INDICES, () => fileList.SelectIndices)
                .Compute("/filelist/checked-items", () => fileList.SelectItems)
                .Compute("/filelist/items", () => fileList.AllItems)
                .Compute("/filelist/count", () => fileList.Items.Count)

                .Compute(StoreKeys.SELECTED_IMAGE, () => imageList.SelectedItem)
                .Compute(StoreKeys.SELECTED_IMAGE_RANGE, () => imageList.SelectItems)
                .Compute(StoreKeys.SELECTED_IMAGE_INDICES, () => imageList.SelectIndices)
                .Compute("/imagelist/items", () => imageList.AllItems)
                .Compute("/imagelist/count", () => imageList.Items.Count)
                .Bind("/draw/image-scale", scaleBox, "Value")
                .Bind(StoreKeys.SAVE_PATH, pathBox, "Text")
                .Watch(StoreKeys.FILES, _ => ListFlush())
                .Watch("/draw/layers", _ => LayerListFlush());

            ;


        }


        /// <summary>
        ///     添加监听
        /// </summary>
        private void AddListenter() {
            versionItem.Click += ShowFeature;
            helpItem.Click += ShowHelp;
            checkUpdateItem.Click += CheckUpdate;

            saveDirItem.Click += OutputDirectory;

            replaceFromFileItem.Click += ReplaceFile;
            addReplaceItem.Click += AddReplace;
            replaceToThisFileItem.Click += ReplaceFromList;

            pathBox.Click += OpenFile;

            saveAsItem.Click += SaveAsImg;
            renameItem.Click += RenameFile;
            addMergeItem.Click += AddMerge;
            addOutsideMergeItem.Click += AddOutMerge;
            runMergeItem.Click += DisplayMerge;
            fileList.SelectedIndexChanged += ImageChanged;
            fileList.ItemDeleted += DeleteImg;
            fileList.ItemDraged += MoveFileIndex;
            fileList.DragDrop += DragDropInput;
            box.Paint += OnPainting;
            box.MouseClick += OnMouseClick;
            box.MouseDown += OnMouseDown;
            box.MouseUp += OnMouseUp;
            box.MouseMove += OnMouseMove;
            box.MouseWheel += OnMouseWheel;
            saveImageItem.Click += SaveImage;
            saveSingleImageItem.Click += SaveSingleImage;
            saveGifItem.Click += SaveGif;
            replaceImageItem.Click += ReplaceImage;
            hideCheckImageItem.Click += HideImage;
            linkImageItem.Click += LinkImage;
            imageList.ItemDeleted += DeleteImage;
            imageList.ItemDraged += MoveImageIndex;
            imageList.SelectedIndexChanged += SelectImageChanged;
            imageList.ItemHoverChanged += PreviewHover;
            changePositionItem.Click += (o, e) => Viewer.Show("changePosition", imageList.SelectItems);
            changeSizeItem.Click += ChangeSize;
            searchBox.TextChanged += (o, e) => ListFlush();

            newImageItem.Click += (o, e) => Viewer.Show("newImage", fileList.SelectedItem);
            relativePositionCheckedBox.CheckedChanged += RealPosition;
            displayBox.CheckedChanged += Display;
            newFileItem.Click += NewFile;
            exchangeFileItem.Click += ExchangeFile;


            hideFileItem.Click += HideFile;
            convertItem.Click += ShowConvert;
            filePropertiesItem.Click += (o, e) => Viewer.Show("properties");

            DragEnter += DragEnterInput;
            DragDrop += DragDropInput;
            undoItem.Click += (o, e) => Controller.Move(-1);
            redoItem.Click += (o, e) => Controller.Move(1);
            closeButton.Click += CloseFile;
            historyButton.Click += ShowHistory;
            scaleBox.ValueChanged += ScaleChange;
            scaleBox.Increment = 30;
            pixelateItem.CheckedChanged += Flush;
            sortItem.Click += Sort;
            classifyItem.CheckedChanged += Classify;
            adjustRuleItem.Click += AjustRule;
            pathBox.TextChanged += (o, e) => pathBox.SelectionStart = pathBox.Text.Length; //光标移到最后，以便显示名称
            saveFileItem.Click += SaveFile;
            layerList.ItemCheck += HideLayer;
            adjustPositionItem.Click += AdjustPosition;


            repairFileItem.Click += RepairFile;
            recoverFileItem.Click += RecoverFile;

            compareFileItem.Click += AddCompareLayer;


            Drawer.BrushChanged += (o, e) => box.Cursor = new Cursor(e.Brush.Cursor);
            Drawer.LayerChanged += (o, e) => LayerListFlush();
            Drawer.LayerVisibleChanged += LayerVisibleChanged;
            Drawer.ColorChanged += ColorChanged;
            Drawer.LayerDrawing += BeforeDraw;

            linearDodgeBox.CheckedChanged += LinearDodge;
            dyeBox.CheckedChanged += Dye;


            previewItem.CheckedChanged += PreviewChanged;
            colorPanel.MouseClick += ColorChanged;
            linearDodgeItem.Click += (o, e) => Controller.Do("linearDodge", imageList.SelectItems);
            dyeItem.Click += DyeImage;

            splitFileItem.Click += (o, e) => Controller.Do("splitFile", fileList.SelectedItems);
            mixFileItem.Click += (o, e) => Controller.Do("mixFile", fileList.SelectedItems);
            cutImageItem.Click += CutImage;
            copyImageItem.Click += CopyImage;
            pasteImageItem.Click += PasteImage;
            cutFileItem.Click += CutFile;
            copyFileItem.Click += CopyFile;
            pasteFileItem.Click += PasteImg;

            canvasCutItem.Click += CutImage;
            canvasCopyItem.Click += CopyImage;
            canvasPasteItem.Click += CanvasPasteImage;
            canvasMoveUpItem.Click += CanvasMoveUp;
            canvasMoveDownItem.Click += CanvasMoveDown;
            canvasMoveLeftItem.Click += CanvasMoveLeft;
            canvasMoveRightItem.Click += CanvasMoveRight;
            canvasMoveHereItem.Click += CanvasMoveHere;


            moveUpItem.Click += CanvasMoveUp;
            moveDownItem.Click += CanvasMoveDown;
            moveLeftItem.Click += CanvasMoveLeft;
            moveRightItem.Click += CanvasMoveRight;

            selectAllHideItem.Click += (o, e) => imageList.CheckWith(sprite => sprite.IsHidden);
            selectAllLinkItem.Click += (o, e) => imageList.CheckWith(sprite => sprite.ColorFormat == ColorFormats.LINK);
            selectThisLinkItm.Click += SelectThisLink;
            selectThisTargetItem.Click += SelectThisTarget;



            fileListMenu.Opening += ShowFileListMenu;



            Controller.CommandChanged += CommandDid;

            layerList.ItemDraged += MoveLayer;
            layerList.ItemDeleted += DeleteLayer;
            addLayerItem.Click += AddLayer;
            upLayerItem.Click += UpLayer;
            downLayerItem.Click += DownLayer;
            renameLayerItem.Click += RenameLayer;

            Viewer.ViewCreated += OnViewCreated;

        }

        private void OnViewCreated(object sender, ViewEventArgs e) {
            if(e.View is Form window) {
                window.Owner = this;
                window.Text = Language[e.Name];
            }
        }

        private void ShowFileListMenu(object sender, EventArgs e) {
            exchangeFileItem.Text = ExchangeSelectedFile != null ? $"{Language["ExchangeFile"]} [{ExchangeSelectedFile.Name}]" : Language["ExchangeFile"];
        }

        private void ReplaceFromList(object sender, EventArgs e) {
            var item = fileList.SelectedItem;
            if(item == null || ReplaceStack.Count == 0) {
                return;
            }
            var f = ReplaceStack.Last();
            Controller.Do("ReplaceFileFromList", new CommandContext {
                { "Source" , item },
                { "Target" , f }
            });
            ReplaceStack.Remove(f);
            OnReplaceStackChanged();
        }

        private void AddReplace(object sender, EventArgs e) {
            ReplaceStack.AddRange(fileList.SelectItems);
            OnReplaceStackChanged();
        }

        private void OnReplaceStackChanged() {
            var stack = ReplaceStack.ToArray();
            Array.Reverse(stack);
            var arr0 = new ToolStripMenuItem[stack.Length];
            var arr1 = new ToolStripMenuItem[stack.Length];
            for(var i = 0; i < stack.Length; i++) {
                var f = stack[i];
                arr0[i] = _GetReplaceItem(f);
                arr1[i] = _GetReplaceItem(f);
                arr1[i].Click += (o, s) => {
                    var item = fileList.SelectedItem;
                    if(item == null) {
                        return;
                    }
                    Controller.Do("replaceFileFromList", new CommandContext {
                        { "Source" , item },
                        { "Target" , f }
                    });
                    ReplaceStack.Remove(f);
                    OnReplaceStackChanged();
                };
            }
            var clearItem0 = new ToolStripMenuItem(Language["ClearList"]);
            var clearItem1 = new ToolStripMenuItem(Language["ClearList"]);
            clearItem0.Click += ClearReplace;
            clearItem1.Click += ClearReplace;

            addReplaceItem.DropDownItems.Clear();
            addReplaceItem.DropDownItems.AddRange(arr0);

            replaceToThisFileItem.DropDownItems.Clear();
            replaceToThisFileItem.DropDownItems.AddRange(arr1);

            if(stack.Length > 0) {
                addReplaceItem.DropDownItems.AddSeparator();
                addReplaceItem.DropDownItems.Add(clearItem0);
                replaceToThisFileItem.DropDownItems.AddSeparator();
                replaceToThisFileItem.DropDownItems.Add(clearItem1);
            }
        }

        private void ClearReplace(object sender, EventArgs e) {
            ReplaceStack.Clear();
            OnReplaceStackChanged();
        }

        private ToolStripMenuItem _GetReplaceItem(Album f) {
            var item = new ToolStripMenuItem(f.Name);
            return item;
        }

        private void CanvasMoveLeft(object sender, EventArgs e) => Move(-Config["MoveStep"].Integer, 0);

        private void CanvasMoveRight(object sender, EventArgs e) => Move(Config["MoveStep"].Integer, 0);

        private void CanvasMoveUp(object sender, EventArgs e) => Move(0, -Config["MoveStep"].Integer);

        private void CanvasMoveDown(object sender, EventArgs e) => Move(0, Config["MoveStep"].Integer);

        private void CanvasMoveHere(object sender, EventArgs e) {
            Drawer.Brush.Location = Drawer.CurrentLayer.RealLocation;
            Drawer.Move(1, Hotpot);
            LayerListFlush();
        }

        private void Move(int x, int y) {
            Drawer.Brush.Location = Drawer.CurrentLayer.RealLocation;
            Drawer.Move(1, Drawer.CurrentLayer.RealLocation.Add(new Point(x, y)));
            LayerListFlush();
        }

        private void ShowHelp(object sender, EventArgs e) {
            Process.Start($"http://es.kritsu.net/guide/");
        }

        private void ShowFeature(object sender, EventArgs e) {
            Process.Start($"http://es.kritsu.net/feature/{Config["Version"]}.html");
        }

        private void CheckUpdate(object sender, EventArgs e) {
            Controller.Do("checkUpdate", true);
        }


        private void AddCompareLayer(object sender, EventArgs e) {
            Controller.Do("addCompareLayer", fileList.SelectItems);
        }


        /// <summary>
        /// 恢复文件 将文件完全替换为源文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecoverFile(object sender, EventArgs e) {
            if(!Directory.Exists(Config["ResourcePath"].Value)) {
                Messager.Error("SelectPathIsInvalid");
                return;
            }
            var files = fileList.SelectItems;
            if(files.Length == 0) {
                return;
            }
            Controller.Do("recoverFile", files);
        }

        /// <summary>
        /// 修复文件 将文件缺失部分补充为源文件的贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepairFile(object sender, EventArgs e) {
            if(!Directory.Exists(Config["ResourcePath"].Value)) {
                Messager.Error("SelectPathIsInvalid");
                return;
            }
            var files = fileList.SelectItems;
            if(files.Length == 0) {
                return;
            }
            Controller.Do("repairFile", files);
        }

        private void Dye(object sender, EventArgs e) {
            Config["Dye"] = new ConfigValue(dyeBox.Checked);
            CanvasFlush();
        }

        private void DeleteLayer(object sender, ItemEventArgs e) {
            var indices = e.Indices;
            if(indices.Length > 0 && MessageBox.Show(Language["DeleteLayerTips"], Language["Tips"],
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK) {
                Controller.Do("deleteLayer", indices);
            }
        }

        private void MoveLayer(object sender, ItemEventArgs e) {
            var index = e.Index;
            var target = e.Target;
            if(index > 1 && target > 1) {
                Controller.Do("moveLayer", new CommandContext {
                    { "Source" , index },
                    { "Target" , target }
                });
            }
        }

        private void UpLayer(object sender, EventArgs e) {
            var index = layerList.SelectedIndex;
            if(index > 2) {
                Controller.Do("moveLayer", new CommandContext {
                    { "Source" , index },
                    { "Target" , index-1 }
                });
            }
        }

        private void DownLayer(object sender, EventArgs e) {
            var index = layerList.SelectedIndex;
            if(index > 1 && index < layerList.Items.Count - 1) {
                Controller.Do("moveLayer", new CommandContext {
                    { "Source" , index },
                    { "Target" , index+1 }
                });
            }
        }

        private void RenameLayer(object sender, EventArgs e) {
            var item = layerList.SelectedItem;
            if(item is Layer layer) {
                var dialog = new ESTextDialog {
                    InputText = layer.Name,
                    Text = Language["Rename"]
                };
                if(dialog.ShowDialog() == DialogResult.OK) {
                    Controller.Do("RenameLayer", new CommandContext {
                        { "Tag" , layer },
                        { "Name" , dialog.InputText }
                    });
                    layerList.Refresh();
                }
            } else {
                Messager.Warning("RenameLayerTips");
            }
        }

        private void BeforeDraw(object sender, LayerEventArgs e) {
            Grid.Tag = Config["GridGap"].Integer;
            Grid.Size = box.Size;
            var entity = imageList.SelectedItem;
            entity = entity != null && entity.ColorFormat == ColorFormats.LINK ? entity.Target : entity;
            Drawer.CurrentLayer.Tag = entity;
            if(entity != null) {

                var pictrue = entity.Image;
                if(linearDodgeBox.Checked) {
                    pictrue = pictrue.LinearDodge();
                }
                if(dyeBox.Checked) {
                    pictrue = pictrue.Dye(Drawer.Color);
                }
                var size = entity.Size;
                size = size.Star(Drawer.ImageScale);
                Drawer.CurrentLayer.Size = size;
                Drawer.CurrentLayer.Image = pictrue; //校正贴图
            }

            Border.Tag = Drawer.CurrentLayer.Rectangle;
            var ruler = Ruler as Ruler;
            ruler.DrawSpan = Config["RulerSpan"].Boolean;
            ruler.DrawCrosshair = Config["RulerCrosshair"].Boolean;
            ruler.Tag = Drawer.CurrentLayer.Location.Minus(Ruler.Location);
            ruler.Size = box.Size;
        }


        private void AddLayer(object sender, EventArgs e) {
            Controller.Do("addLayer", imageList.SelectItems);
        }

        private void LayerVisibleChanged(object sender, LayerEventArgs e) {
            var index = e.ChangedIndex;
            Store.Get("/draw/layers", out List<IPaint> layers);
            var layer = layers[0];
            var pos = layerList.Items.IndexOf(layer);
            if(pos > -1) {
                layerList.SetItemChecked(pos, layer.Visible);
            }
        }

        private void LayerListFlush() {
            if(move_mode > -1) {
                return;
            }
            Store.Get("/draw/layers", out List<IPaint> layers);
            layerList.Clear();
            foreach(var t in layers) {
                layerList.Items.Add(t, t.Visible);
            }
            layerList.Invalidate();
        }

        private void CommandDid(object sender, CommandEventArgs e) {
            var refreshMode = e.Context.Get<RefreshMode>("__REFRESH_MODE");
            switch(refreshMode) {
                case RefreshMode.List:
                    ListFlush();
                    break;
                case RefreshMode.File:
                    ImageListFlush();
                    break;
                case RefreshMode.Image:
                    CanvasFlush();
                    break;
            }
        }

        private void ScaleChange(object sender, EventArgs e) {
            Config["CanvasScale"] = new ConfigValue(scaleBox.Value);
            Drawer.ImageScale = scaleBox.Value / 100;
            CanvasFlush();
        }

        private void ChangeSize(object sender, EventArgs e) {
            Viewer.Show("changeSize", fileList.SelectedItem, imageList.SelectIndices, Drawer.ImageScale);
        }

        private void RealPosition(object sender, EventArgs e) {
            Drawer.CurrentLayer.RealPosition = relativePositionCheckedBox.Checked;
            Drawer.LastLayer.RealPosition = relativePositionCheckedBox.Checked;
            Drawer.CompareLayers.ForEach(c => c.RealPosition = relativePositionCheckedBox.Checked);
            CanvasFlush();
        }


        private void SelectThisLink(object sender, EventArgs e) {
            var cur = imageList.SelectedItem;
            if(cur != null && cur.ColorFormat != ColorFormats.LINK) {
                imageList.CheckWith(sprite => sprite.ColorFormat == ColorFormats.LINK && cur.Equals(sprite.Target));
            }
        }

        private void SelectThisTarget(object sender, EventArgs e) {
            var cur = imageList.SelectedItem;
            if(cur != null && cur.ColorFormat == ColorFormats.LINK) {
                for(var i = 0; i < imageList.Items.Count; i++) {
                    if(imageList.Items[i].Equals(cur.Target)) {
                        imageList.SelectedIndex = i;
                    }
                }
            }
        }

        /// <summary>
        ///     粘贴img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteImg(object sender, EventArgs e) {
            var index = fileList.SelectedIndex;
            index = index < 0 ? fileList.Items.Count : index;
            Controller.Do("pasteFile", index);
        }

        /// <summary>
        ///     复制/剪切img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutFile(object sender, EventArgs e) {
            Controller.Do("CutFile", new CommandContext{
                { "Files",fileList.SelectItems},
                { "Mode", ClipMode.CUT }
            });
        }

        /// <summary>
        ///     复制/剪切img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyFile(object sender, EventArgs e) {
            Controller.Do("CutFile", new CommandContext{
                { "Files",fileList.SelectItems},
                { "Mode", ClipMode.COPY }
            });
        }

        private void CopyImage(object sender, EventArgs e) {
            CutImage(ClipMode.COPY);
        }


        /// <summary>
        ///     复制/剪切图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutImage(object sender, EventArgs e) {
            CutImage(ClipMode.CUT);
        }


        private void CutImage(ClipMode mode) {
            var al = fileList.SelectedItem;
            if(al != null) {
                var indices = imageList.SelectIndices;
                Controller.Do("CutImage", new CommandContext {
                    { "File",al },
                    { "Indices",indices },
                    { "Mode",mode }
                });
            }
        }

        private void CanvasPasteImage(object sender, EventArgs e) {
            var al = fileList.SelectedItem;
            if(al != null) {
                var image = imageList.SelectedItem;
                Controller.Do("pasteSingleImage", new CommandContext {
                    { "@" , image },
                    { "Location", Hotpot }
                });
                Drawer.CurrentLayer.RealLocation = Hotpot;
            }
        }


        /// <summary>
        ///     粘贴图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteImage(object sender, EventArgs e) {
            var al = fileList.SelectedItem;
            if(al != null) {
                var index = imageList.SelectedIndex;
                index = index < 0 ? fileList.Items.Count : index;
                Controller.Do("pasteImage", new CommandContext(al) {
                    { "Index",index }
                });
            }
        }

        /// <summary>
        ///     线性减淡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinearDodge(object sender, EventArgs e) {
            Config["LinearDodge"] = new ConfigValue(linearDodgeBox.Checked);
            CanvasFlush();
        }

        /// <summary>
        ///     当绘制器的颜色发生改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorChanged(object sender, ColorEventArgs e) {
            colorPanel.BackColor = e.NewColor;
            if(dyeBox.Checked) {
                CanvasFlush();
            }
        }

        /// <summary>
        ///     点击颜色选择框切换颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorChanged(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Right) {
                Drawer.Color = Color.Empty;
                return;
            }

            if(colorDialog.ShowDialog() == DialogResult.OK) {
                Drawer.Color = colorDialog.Color;
            }
        }

        private void DyeImage(object sender, EventArgs e) {
            Controller.Do("DyeImage", new CommandContext {
                { "File", fileList.SelectedItem },
                { "Indices",imageList.SelectIndices },
                { "Color", Drawer.Color }
            });
        }

        private void ImageChanged(object sender, EventArgs e) {
            Drawer.OnPalatteChanged(new FileEventArgs {
                Entity = imageList.SelectedItem,
                Album = fileList.SelectedItem
            });
            ImageListFlush();
        }

        private void PreviewChanged(object sender, EventArgs e) {
            Config["Preview"] = new ConfigValue(previewItem.Checked);
            previewPanel.Visible = previewItem.Checked;
        }

        private void PreviewHover(object sender, ItemHoverEventArgs e) {
            var entity = e.Item as Sprite;
            if(previewItem.Checked && entity != null) {
                previewPanel.BackgroundImage = entity.Image;
                previewPanel.Visible = true;
            }
        }


        private void AdjustPosition(object sender, EventArgs e) {
            var index = imageList.SelectedIndex;
            var item = fileList.SelectedItem;
            if(index > -1 && item != null) {
                var location = Drawer.CurrentLayer.Rectangle.Location;
                if(relativePositionCheckedBox.Checked) {
                    location = location.Minus(item[index].Location);
                }
                Drawer.CurrentLayer.Location = Point.Empty;
                Controller.Do("ChangePosition", new CommandContext {
                    { "File", item },
                    { "Indices",new []{index} },
                    { "X" , location.X },
                    { "Y" , location.Y },
                    { "FrameWidth",null },
                    { "FrameHeight",null },
                    { "Relative" , relativePositionCheckedBox.Checked }
                });

            }
        }


        /// <summary>
        ///     隐藏图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideLayer(object sender, EventArgs e) {
            var index = layerList.SelectedIndex;
            var item = layerList.SelectedItem;
            if(index < 0 || item == null) {
                return;
            }

            item.Visible = !layerList.GetItemChecked(index);
            CanvasFlush();
        }


        /// <summary>
        ///     移动文件序列
        /// </summary>
        private void MoveFileIndex(object sender, ItemEventArgs e) {
            if(e.Index > -1 && fileList.Items.Count > 0) {
                Controller.Do("MoveFile", new CommandContext {
                    {"Index", e.Index },
                    {"Target",e.Target }
                });
                fileList.SelectedIndex = e.Target;
            }
        }


        /// <summary>
        ///     移动贴图序列
        /// </summary>
        private void MoveImageIndex(object sender, ItemEventArgs e) {
            var al = fileList.SelectedItem;
            if(al != null && e.Index > -1 && imageList.Items.Count > 0) {

                Controller.Do("MoveImage", new CommandContext(al){
                    { "Source", e.Index },
                    { "Target", e.Target }
                });
                imageList.SelectedIndex = e.Target;
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e) {
            var context = new CommandContext();
            Controller.Do("ClosingConfirm", context);
            e.Cancel = context.Get<bool>("CANCEL");
            if(!e.Cancel) {
                SaveConfig();
                player.Close();
            }
            base.OnFormClosing(e);
        }

        private void SaveConfig() {
            Config["CanvasScale"] = new ConfigValue(scaleBox.Value);
            Config["Pixelate"] = new ConfigValue(pixelateItem.Checked);

            Config["LinearDodge"] = new ConfigValue(linearDodgeBox.Checked);
            Config["Dye"] = new ConfigValue(dyeBox.Checked);
            Config["RealPosition"] = new ConfigValue(relativePositionCheckedBox.Checked);
            Config["Animation"] = new ConfigValue(displayBox.Checked);

            Config["Ruler"] = new ConfigValue(Ruler.Visible);
            Config["RulerLocked"] = new ConfigValue(Ruler.Locked);

            Config["Grid"] = new ConfigValue(Grid.Visible);
            Config.Save();
        }


        private void AjustRule(object sender, EventArgs e) {
            Ruler.Location = Drawer.CurrentLayer.Location;
            CanvasFlush();
        }

        private void SelectImageChanged(object sender, EventArgs e) {
            var lastPosition = Drawer.CurrentLayer.Location;
            var lastLayerVisible = Drawer.LastLayer.Visible;
            Drawer.CurrentLayer = new Canvas();
            var image = imageList.SelectedItem;
            if(image != null) {
                Drawer.CurrentLayer.Size = image.Size;
            }
            Drawer.CurrentLayer.Location = lastPosition;
            Drawer.CompareLayers.ForEach(c => c.Index = imageList.SelectedIndex);
            CanvasFlush();
            Drawer.OnLayerChanged(new LayerEventArgs());
        }

        private void Sort(object sender, EventArgs e) {
            Controller.Do("sortFile");
        }

        private void Classify(object sender, EventArgs e) {
            ListFlush();
        }


        /// <summary>
        ///     列表刷新
        /// </summary>
        public void ListFlush() {
            if(fileList.InvokeRequired) {
                fileList.Invoke(new MethodInvoker(ListFlush));
                return;
            }

            Store.Get("/data/files", out List<Album> list);
            var items = fileList.CheckedItems;
            var itemArray = new Album[items.Count];
            items.CopyTo(itemArray, 0);
            fileList.Clear();
            var condition = searchBox.Text.Trim().Split(" ");
            var array = NpkCoder.Find(list, condition);

            if(classifyItem.Checked) {
                var path = "";
                foreach(var al in array) {
                    var p = al.Path.Replace(al.Name, "");
                    if(p != path) {
                        path = p;
                        var sp = new Album {
                            Path = $"---------------{Config["ClassifySeparator"]}---------------"
                        };
                        fileList.Items.Add(sp);
                    }
                    fileList.Items.Add(al);
                }
            } else {
                fileList.Items.AddRange(array.ToArray());
            }

            for(var i = 0; i < array.Count; i++) {
                if(itemArray.Contains(array[i])) {
                    fileList.SetItemChecked(i, true);
                }
            }

            // 如果文件列表没有选中任何文件 且文件列表至少有一个文件 则默认选择第一个文件
            if(fileList.SelectedIndex == -1 && fileList.Items.Count > 0) {
                fileList.SelectedIndex = 0;
            }
        }


        private void AddOutMerge(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Multiselect = true,
                Filter = $"{Language["ImageResources"]}| {Extensions}"
            };
            if(dialog.ShowDialog() == DialogResult.OK) {
                Controller.Do("loadFile", dialog.FileNames)
                    .Do("addMerge");
            }
        }


        private void ShowHistory(object sender, EventArgs e) {
            dropPanel.BringToFront();
            dropPanel.Visible = !dropPanel.Visible;
            dropPanel.Refresh();
        }


        private void CloseFile(object sender, EventArgs e) {
            player.Pause(null, null);
            Clipboad.Clear();
            Store.Set("/data/files", new List<Album>())
                .Set("/data/save-path", string.Empty)
                .Set("/data/is-save", true);
            ImageChanged(this, e);
            LayerListFlush();
        }

        /// <summary>
        /// 按住alt，滚动鼠标滑轮可放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseWheel(object sender, MouseEventArgs e) {
            if(ModifierKeys == Keys.Alt) {
                var i = scaleBox.Value + e.Delta / 12;
                i = i < scaleBox.Maximum ? i : scaleBox.Maximum;
                i = i > scaleBox.Minimum ? i : scaleBox.Minimum;
                scaleBox.Value = i;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if(keyData.HasFlag(Keys.Alt)) {
                box.Focus();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DragEnterInput(object sender, DragEventArgs e) {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void DragDropInput(object sender, DragEventArgs e) {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var args = e.Data.GetData(DataFormats.FileDrop, false) as string[];
                Controller.Do("OpenFile", new CommandContext(args){
                    {"IsClear" , false},
                });
            } else if(e.Data.GetDataPresent(DataFormats.Serializable)) {
                (sender as Control)?.DoDragDrop(e.Data, e.Effect);
            }
        }


        private void ShowConvert(object sender, EventArgs e) {
            var array = fileList.SelectItems;
            if(array.Length > 0 && CheckOgg(array)) {
                Viewer.Show("convert", array);
            }
        }

        private bool CheckOgg(params Album[] args) {
            foreach(var al in args) {
                if(al.Version == ImgVersion.Other) {
                    Messager.Warning("NotHandleFile");
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        ///     播放动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display(object sender, EventArgs e) {
            if(displayBox.Checked) {
                var thread = new Thread(Display) {
                    IsBackground = true,
                    Name = "display"
                };
                thread.Start();
            }
        }

        private void Display() {
            while(displayBox.Checked) {
                DisplayNext();
                Thread.Sleep(1000 / Config["FlashSpeed"].Integer);
            }
        }


        private void DisplayNext() {
            if(imageList.InvokeRequired) {
                imageList.Invoke(new MethodInvoker(DisplayNext));
                return;
            }

            var i = imageList.SelectedIndex + 1;
            var count = imageList.Items.Count;
            i = i < count ? i : 0;
            if(count > 0) {
                imageList.SelectedIndex = i;
            }
        }


        /// <summary>
        ///     隐藏勾选img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideFile(object sender, EventArgs e) {
            var list = fileList.SelectItems;
            if(list.Length > 0 && CheckOgg(list) &&
                MessageBox.Show(Language["HideTips"], "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK) {
                Controller.Do("hideFile", list);
            }
        }

        /// <summary>
        ///     打开新建img窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// F
        private void NewFile(object sender, EventArgs e) {
            var list = Store.Get<List<Album>>("/data/files");
            Viewer.Show("newFile", fileList.SelectedIndex);
        }

        private void OpenFile(object sender, EventArgs e) {
            Controller.Do("OpenFile");
        }

        private void ExchangeFile(object sender, EventArgs e) {
            if(ExchangeSelectedFile == null) {
                ExchangeSelectedFile = fileList.SelectedItem;
            } else {
                var target = fileList.SelectedItem;
                if(target == null || ExchangeSelectedFile.Equals(target)) {
                    return;
                }
                Controller.Do("ExchangeFile", new CommandContext {
                    { "Source",ExchangeSelectedFile },
                    {"Target",target }
                });
                ExchangeSelectedFile = null;
            }
        }


        /// <summary>
        ///     替换img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceFile(object sender, EventArgs e) {
            var item = fileList.SelectedItem;
            if(item != null) {
                var dialog = new OpenFileDialog {
                    Filter =
                    $"{Language["ImageResources"]}|*.img;*.gif|{Language["SoundResources"]}|*.ogg;|{Language["AllFormat"]}|*.*"
                };
                if(item.Version == ImgVersion.Other || item.Name.EndsWith(".ogg")) {
                    dialog.FilterIndex = 2;
                } else if(item.Name.EndsWith(".img")) {
                    dialog.FilterIndex = 1;
                } else {
                    dialog.FilterIndex = 3;
                }
                if(dialog.ShowDialog() == DialogResult.OK) {
                    Controller
                        .Do("LoadFile", dialog.FileName)
                        .Do("ReplaceFile", new CommandContext {
                            {"Target", item }
                        });
                }
            }
        }

        /// <summary>
        ///     img另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsImg(object sender, EventArgs e) {
            var array = fileList.SelectItems;
            if(array.Length == 1) {
                var dialog = new SaveFileDialog {
                    DefaultExt = $".{array[0].Name.LastSubstring('.')}",
                    FileName = array[0].Name,
                    Filter = "IMG|*.img|GIF|*.gif|OGG|*.ogg"
                };
                if(dialog.ShowDialog() == DialogResult.OK) {
                    Controller.Do("SaveAsFile", new CommandContext(array[0]) {
                        {"Path", dialog.FileName }
                    });
                }
            } else if(array.Length > 1) {
                var dialog = new FolderBrowserDialog();
                if(dialog.ShowDialog() == DialogResult.OK) {
                    Controller.Do("SaveToDirectory", new CommandContext(array) {
                        { "Path", dialog.SelectedPath }
                    });
                }
            }
        }

        /// <summary>
        ///     删除img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteImg(object sender, EventArgs e) {
            var array = fileList.SelectItems;
            if(array.Length > 0 && MessageBox.Show(Language["DeleteTips"], Language["Tips"],
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK) {
                Controller.Do("deleteFile", array);
            }
        }

        /// <summary>
        ///     全部选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckAllImg(object sender, EventArgs e) {
            fileList.CheckAll();
        }

        /// <summary>
        ///     反向勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReverseCheckImg(object sender, EventArgs e) {
            fileList.ReverseCheck();
        }

        /// <summary>
        ///     全部选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckAllImage(object sender, EventArgs e) {
            imageList.CheckAll();
        }

        /// <summary>
        ///     反向勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReverseCheckImage(object sender, EventArgs e) {
            imageList.ReverseCheck();
        }


        /// <summary>
        ///     img重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameFile(object sender, EventArgs e) {
            var album = fileList.SelectedItem;
            if(album != null) {
                var dialog = new ESTextDialog() {
                    InputText = album.Path,
                    Text = Language["Rename"]
                };
                if(dialog.ShowDialog() == DialogResult.OK) {
                    Controller.Do("RenameFile", new CommandContext {
                        { "File", album },
                        { "Path", dialog.InputText }
                    });
                }
            }
        }


        /// <summary>
        ///     画布刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flush(object sender, EventArgs e) {
            CanvasFlush();
        }


        /// <summary>
        ///     贴图列表刷新
        /// </summary>
        public void ImageListFlush() {
            if(move_mode > -1) {
                return;
            }
            var al = fileList.SelectedItem; //记录当前所选img
            var index = imageList.SelectedIndex; //记录当前选择贴图
            var items = imageList.SelectItems;
            if(al != null && al.Version == ImgVersion.Other) {
                //判断是否为ogg音频
                player.Play(al);
            } else {
                player.Visible = false;
                imageList.Clear();
                var array = al?.List.ToArray();
                if(array != null) {
                    imageList.Items.AddRange(array);
                    for(var i = 0; i < array.Length; i++) {
                        if(items.Contains(array[i])) {
                            imageList.SetItemChecked(i, true);
                        }
                    }
                }

                //添加贴图
                index = index > -1 && index < imageList.Items.Count ? index : 0;
                if(imageList.Items.Count > 0) {
                    imageList.SelectedIndex = index;
                } else {
                    CanvasFlush();
                }
            }

        }


        private void SaveFile(object sender, EventArgs e) {
            var path = Store.Get<string>("/data/save-path");
            if(path.Trim().Length == 0) {
                Controller.Do("SetSavePath");
                SaveFile(sender, e);
            } else {
                Controller.Do("SaveFile", path);
            }
        }


        /// <summary>
        ///     存为文件夹(img)
        /// </summary>
        /// <param name="sender"></param>
        private void OutputDirectory(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK) {
                Controller.Do("SaveToDirectory", dialog.SelectedPath);
            }
        }

        private void Exit(object sender, EventArgs e) {
            this.Close();

        }

        private void AddMerge(object sender, EventArgs e) {
            var array = fileList.SelectItems;
            if(array.Length > 0 && CheckOgg(array)) {
                Controller.Do("addMerge", array);
            }
        }

        private void DisplayMerge(object sender, EventArgs e) {
            Viewer.Show("merge", fileList.SelectedItem);
        }

        public void CanvasFlush() {
            box.Invalidate();
        }

        /// <summary>
        ///     画布刷新
        /// </summary>
        private void OnPainting(object sender, PaintEventArgs e) {
            var g = e.Graphics;
            g.InterpolationMode = pixelateItem.Checked ? InterpolationMode.NearestNeighbor : InterpolationMode.High;
            Drawer.DrawLayer(g);
        }


        /// <summary>
        ///     鼠标左键单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left) {
                var point = e.Location;
                move_mode = Drawer.IndexOfLayer(point);
                Drawer.CusorLocation = point;
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e) {
            Hotpot = e.Location;
            if(e.Button == MouseButtons.Left) {
                if(!Drawer.IsSelect("MoveTool")) {
                    Drawer.Brush.Draw(Drawer.CurrentLayer, Hotpot, Drawer.ImageScale);
                }
            } else if(e.Button == MouseButtons.Right) {

                var canMove = Drawer.IsSelect("MoveTool");
                canvasMoveUpItem.Visible = canMove;
                canvasMoveDownItem.Visible = canMove;
                canvasMoveLeftItem.Visible = canMove;
                canvasMoveRightItem.Visible = canMove;
                canvasMoveHereItem.Visible = canMove;
                if(canMove) {
                    canvasMoveHereItem.Text = $"{Language["MoveHere"]}{Hotpot.GetString()}";
                }


                canvasMenu.Show(box, Hotpot);
            }
        }


        /// <summary>
        ///     鼠标左键单击释放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left) {
                move_mode = -1;
                //ImageFlush();
                //LayerListFlush();
            }
        }

        /// <summary>
        ///     鼠标左键单击移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left && move_mode != -1) {
                Drawer.Move(move_mode, e.Location);
                CanvasFlush();
            }
        }


        /// <summary>
        ///     保存贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveImage(object sender, EventArgs e) {
            var album = fileList.SelectedItem;
            if(album == null || album.List.Count < 1) {
                return;
            }
            Viewer.Show("saveImage", new { allImage = false });
        }

        private void SaveSingleImage(object sender, EventArgs e) {
            var file = fileList.SelectedItem;
            var index = imageList.SelectedIndex;
            if(file == null || index < 0) {
                return;
            }
            var dialog = new SaveFileDialog {
                FileName = file.Name.RemoveSuffix(),
                Filter = "png|*.png|bmp|*.bmp|jpg|*.jpg"
            };
            if(dialog.ShowDialog() == DialogResult.OK) {
                Controller.Do("SaveImage", new CommandContext(file){
                    { "Indices",new []{index} },
                    { "Path" , dialog.FileName}
                });
            }
        }


        /// <summary>
        ///     保存为gif
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveGif(object sender, EventArgs e) {
            var indices = imageList.SelectIndices;
            if(indices.Length < 1) {
                return;
            }
            var dialog = new SaveFileDialog();
            var file = fileList.SelectedItem;
            var name = file.Name.RemoveSuffix(".");
            dialog.Filter = "GIF|*.gif";
            dialog.FileName = name;
            if(dialog.ShowDialog() == DialogResult.OK) {

                Controller.Do("SaveGif", new CommandContext(file){
                    {"Indices",indices },
                    {"Path",dialog.FileName },
                    {"Transparent", Config["GifTransparent"].Color},
                    {"Delay",Config["GifDelay"].Integer}
                });
            }
        }


        /// <summary>
        ///     替换贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceImage(object sender, EventArgs e) {
            var array = imageList.SelectItems;
            if(array.Length > 0) {
                Viewer.Show("replaceImage");
            }
            CanvasFlush();
        }

        private void HideImage(object sender, EventArgs e) {
            Controller.Do("HideImage", new CommandContext {
                { "File",fileList.SelectedItem },
                { "Indices",imageList.SelectIndices }
            });
        }

        /// <summary>
        ///     将勾选贴图变成链接贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkImage(object sender, EventArgs e) {
            var indices = imageList.SelectIndices;
            if(indices.Length < 1) {
                return;
            }
            var dialog = new ESTextDialog {
                CanEmpty = true,
                Text = Language["LinkImage"]
            };
            if(dialog.ShowDialog() == DialogResult.OK) {
                var str = dialog.InputText;
                if(Regex.IsMatch(str, "^\\d")) {
                    Controller.Do("LinkImage", new CommandContext(fileList.SelectedItem){
                        { "TargetIndex",int.Parse(str) },
                        { "Indices",indices }
                    });
                }
                CanvasFlush();
            }
        }

        /// <summary>
        ///     删除贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteImage(object sender, EventArgs e) {
            var indices = imageList.SelectIndices;
            var album = fileList.SelectedItem;
            if(album != null && indices.Length > 0 && MessageBox.Show(Language["DeleteTips"], Language["Tips"],
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK) {
                Controller.Do("deleteImage", new CommandContext {
                    {"File", album },
                    {"Indices", indices }
                });
            }
        }


    }
}