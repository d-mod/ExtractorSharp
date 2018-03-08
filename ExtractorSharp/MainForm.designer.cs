using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.View;
using ExtractorSharp.Component;
using ExtractorSharp.Draw;
using ExtractorSharp.Core;
using System.IO;
using ExtractorSharp.Data;

namespace ExtractorSharp {
    partial class MainForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            components = new Container();
            albumList = new ESListBox<Album>();
            albumListMenu = albumList.ContextMenuStrip;

            editImgItem = new ToolStripMenuItem();
            cutImgItem = new ToolStripMenuItem();
            copyImgItem = new ToolStripMenuItem();
            pasteImgItem = new ToolStripMenuItem();

            replaceItem = new ToolStripMenuItem();
            saveAsItem = new ToolStripMenuItem();
            newImgItem = new ToolStripMenuItem();
            hideImgItem = new ToolStripMenuItem();
            addMergeItem = new ToolStripMenuItem();
            addOutsideMergeItem = new ToolStripMenuItem();
            runMergeItem = new ToolStripMenuItem();
            renameItem = new ToolStripMenuItem();


            repairFileItem = new ToolStripMenuItem();
            splitFileItem = new ToolStripMenuItem();
            mixFileItem = new ToolStripMenuItem();

            imageList = new ESListBox<Sprite>();
            imageListMenu = imageList.ContextMenuStrip;

            editImageItem = new ToolStripMenuItem();
            cutImageItem = new ToolStripMenuItem();
            copyImageItem = new ToolStripMenuItem();
            pasteImageItem = new ToolStripMenuItem();

            addLayerItem = new ToolStripMenuItem();
            saveImageItem = new ToolStripMenuItem();
            changePositionItem = new ToolStripMenuItem();
            changeSizeItem = new ToolStripMenuItem();

            adjustPositionItem = new ToolStripMenuItem();
            replaceImageItem = new ToolStripMenuItem();
            hideCheckImageItem = new ToolStripMenuItem();
            linkImageItem = new ToolStripMenuItem();
            newImageItem = new ToolStripMenuItem();
            saveSingleImageItem = new ToolStripMenuItem();
            saveAllImageItem = new ToolStripMenuItem();
            openFileItem = new ToolStripMenuItem();
            saveFileItem = new ToolStripMenuItem();

            convertItem = new ToolStripMenuItem();

            mainMenu = new MenuStrip();
            fileMenu = new ToolStripMenuItem();
            addFileItem = new ToolStripMenuItem();
            openDirItem = new ToolStripMenuItem();
            saveAsFileItem = new ToolStripMenuItem();
            saveDirItem = new ToolStripMenuItem();
            
            canvasImageItem = new ToolStripMenuItem();
            uncanvasImageItem = new ToolStripMenuItem();
            lineDodgeItem = new ToolStripMenuItem();

            editMenu = new ToolStripMenuItem();
            undoItem = new ToolStripMenuItem();
            redoItem = new ToolStripMenuItem();
            classifyItem = new ToolStripMenuItem();

            viewMenu = new ToolStripMenuItem();

            ruleItem = new ToolStripMenuItem();
            displayRuleItem = new ToolStripMenuItem();
            displayRuleCrossHairItem = new ToolStripMenuItem();
            adjustRuleItem = new ToolStripMenuItem();
            lockRuleItem = new ToolStripMenuItem();
            previewItem = new ToolStripMenuItem();

            gridItem = new ToolStripMenuItem();
            
            aboutItem = new ToolStripMenuItem();
            sortItem = new ToolStripMenuItem();

            toolsMenu = new ToolStripMenuItem();

            modelMenu = new ToolStripMenuItem();

            Messager Messager = Messager.Default;
            saveGifItem = new ToolStripMenuItem();
            box = new PictureBox();
            mutipleLayerItem = new ToolStripMenuItem();
            linedodgeBox = new CheckBox();
            realPositionBox = new CheckBox();
            onionskinBox = new CheckBox();
            displayBox = new CheckBox();

            openButton = new ESButton();
            closeButton = new ESButton();
            historyButton = new ESButton();
            searchBox = new TextBox();
            pathBox = new TextBox();

            scaleLabel = new Label();
            scaleBox = new NumericUpDown();
            pixelateBox = new CheckBox();


            aboutMenu = new ToolStripMenuItem();
            debugItem = new ToolStripMenuItem();
            versionItem = new ToolStripMenuItem();
            pluginItem = new ToolStripMenuItem();
            propertyItem = new ToolStripMenuItem();

            trackBar = new TrackBar();

            layerList = new ESListBox<Layer>();
            layerMenu = layerList.ContextMenuStrip;
            renameLayerItem = new ToolStripMenuItem();
            changeLayerPositionItem = new ToolStripMenuItem();
            adjustEntityPositionItem = new ToolStripMenuItem();
            loadModelItem = new ToolStripMenuItem();
            saveAsLayerItem = new ToolStripMenuItem();
            replaceLayerItem = new ToolStripMenuItem();

            canvasMenu = new ContextMenuStrip();
            canvasCutItem = new ToolStripMenuItem();
            canvasCopyItem = new ToolStripMenuItem();
            canvasPasteItem = new ToolStripMenuItem();


            selectItem = new ToolStripMenuItem();
            selectAllHideItem = new ToolStripMenuItem();
            selectAllLinkItem = new ToolStripMenuItem();
            selectThisLinkItm = new ToolStripMenuItem();
            selectThisTargetItem = new ToolStripMenuItem();

            previewPanel = new Panel();
            colorPanel = new Panel();
            albumListMenu.SuspendLayout();
            imageListMenu.SuspendLayout();
            mainMenu.SuspendLayout();
            ((ISupportInitialize)(box)).BeginInit();
            SuspendLayout();
            // 
            // albumList
            // 
            albumList.HorizontalScrollbar = true;
            albumList.Location = new Point(20, 90);
            albumList.Name = "albumList";
            albumList.Size = new Size(200, 579);
            albumList.TabIndex = 3;
            albumList.CanClear = false;
            // 
            // albumListMenu
            // 
            albumListMenu.Items.Add(editImgItem);
            albumListMenu.Items.Add(newImgItem);
            albumListMenu.Items.AddSeparator();
            albumListMenu.Items.Add(replaceItem);
            albumListMenu.Items.Add(saveAsItem);
            albumListMenu.Items.AddSeparator();
            albumListMenu.Items.Add(repairFileItem);
            albumListMenu.Items.Add(splitFileItem);
            albumListMenu.Items.Add(mixFileItem);
            albumListMenu.Items.AddSeparator();
            albumListMenu.Items.Add(hideImgItem);
            albumListMenu.Items.Add(renameItem);
            albumListMenu.Items.Add(convertItem);
            albumListMenu.Items.AddSeparator();
            albumListMenu.Items.Add(addMergeItem);
            albumListMenu.Items.Add(addOutsideMergeItem);
            albumListMenu.Items.Add(runMergeItem);
            albumListMenu.Items.AddSeparator();
            albumListMenu.Size = new Size(221, 268);

            editImgItem.Text = Language["Edit"];

            editImgItem.DropDownItems.Add(cutImgItem);
            editImgItem.DropDownItems.Add(copyImgItem);
            editImgItem.DropDownItems.Add(pasteImgItem);

            replaceItem.Text = Language["ReplaceFile"];
            replaceItem.ShortcutKeys = Keys.Control | Keys.Q;

            cutImgItem.Text = Language["Cut"];
            cutImgItem.ShortcutKeys = Keys.Control | Keys.X;
            copyImgItem.Text = Language["Copy"];
            copyImgItem.ShortcutKeys = Keys.Control | Keys.C;
            pasteImgItem.Text = Language["Paste"];
            pasteImgItem.ShortcutKeys = Keys.Control | Keys.V;

            saveAsItem.Text = Language["SaveAs"];
            saveAsItem.ShortcutKeys = Keys.Control | Keys.E;
            newImgItem.Text = Language["NewFile"];
            newImgItem.ShortcutKeys = Keys.Control | Keys.N;
            hideImgItem.Text = Language["HideFile"];
            hideImgItem.ShortcutKeys = Keys.Control | Keys.H;
            convertItem.Text = Language["ConvertVersion"];
            addMergeItem.Text = Language["AddMerge"];
            addMergeItem.ShortcutKeys = Keys.Control | Keys.M;
            addOutsideMergeItem.Text = Language["AddOutsideMerge"];
            addOutsideMergeItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.M;
            runMergeItem.Text = Language["RunMerge"];
            renameItem.Text = Language["Rename"];
            renameItem.ShortcutKeys = Keys.Control | Keys.R;
            repairFileItem.Text = Language["RepairFile"];
            splitFileItem.Text = Language["SplitFile"];
            mixFileItem.Text = Language["MixFile"];
            // 
            // imageList
            // 
            imageList.HorizontalScrollbar = true;
            imageList.Location = new Point(1050, 90);
            imageList.Name = "imageList";
            imageList.Size = new Size(240, 280);
            imageList.TabIndex = 4;
            imageList.CanClear = false;
            // 
            // imageListMenu
            // 
            imageListMenu.Items.Add(selectItem);
            imageListMenu.Items.AddSeparator();
            imageListMenu.Items.Add(editImageItem);
            imageListMenu.Items.AddSeparator();
            imageListMenu.Items.Add(newImageItem);
            imageListMenu.Items.Add(replaceImageItem);
            imageListMenu.Items.AddSeparator();
            imageListMenu.Items.Add(changePositionItem);
            imageListMenu.Items.Add(changeSizeItem);
            imageListMenu.Items.AddSeparator();
            imageListMenu.Items.Add(saveImageItem);
            imageListMenu.Items.Add(saveSingleImageItem);
            imageListMenu.Items.Add(saveAllImageItem);
            imageListMenu.Items.Add(saveGifItem);
            imageListMenu.Items.AddSeparator();
            imageListMenu.Items.Add(hideCheckImageItem);
            imageListMenu.Items.Add(linkImageItem);
            imageListMenu.Items.Add(addLayerItem);
            imageListMenu.Name = "imageListMenu";
            imageListMenu.Size = new Size(161, 202);

            editImageItem.Text = Language["Edit"];

            editImageItem.DropDownItems.Add(cutImageItem);
            editImageItem.DropDownItems.Add(copyImageItem);
            editImageItem.DropDownItems.Add(pasteImageItem);
            editImageItem.DropDownItems.AddSeparator();
            editImageItem.DropDownItems.Add(canvasImageItem);
            editImageItem.DropDownItems.Add(uncanvasImageItem);
            editImageItem.DropDownItems.AddSeparator();
            editImageItem.DropDownItems.Add(lineDodgeItem);

            cutImageItem.Text = Language["Cut"];
            copyImageItem.Text = Language["Copy"];
            pasteImageItem.Text = Language["Paste"];


            saveImageItem.Text = Language["SaveImage"];
            saveSingleImageItem.Text = Language["SaveAs"];
            saveAllImageItem.Text = Language["SaveAllImage"];
            saveGifItem.Text = Language["SaveGif"];
            changePositionItem.Text = Language["ChangeImagePosition"];
            changeSizeItem.Text = Language["ChangeImageSize"];
            replaceImageItem.Text = Language["ReplaceImage"];
            hideCheckImageItem.Text = Language["HideImage"];
            linkImageItem.Text = Language["LinkImage"];
            newImageItem.Text = Language["NewImage"];
            addLayerItem.Text = Language["AddLayer"];


            selectItem.Text = Language["Select"];
            selectAllHideItem.Text = Language["SelectHide"];
            selectAllLinkItem.Text = Language["SelectLink"];
            selectThisLinkItm.Text = Language["SelectThisLink"];
            selectThisTargetItem.Text = Language["SelectThisTarget"];

            selectItem.DropDownItems.Add(selectAllHideItem);
            selectItem.DropDownItems.Add(selectAllLinkItem);
            selectItem.DropDownItems.Add(selectThisLinkItm);
            selectItem.DropDownItems.Add(selectThisTargetItem);
        
            canvasImageItem.Text = Language["CanvasImage"];
            uncanvasImageItem.Text = Language["UnCanvasImage"];
            lineDodgeItem.Text = Language["LineDodge"];

            // 
            // mainMenu
            // 
            mainMenu.BackColor = Config["MainColor"].Color;
            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(editMenu);
            mainMenu.Items.Add(viewMenu);
            mainMenu.Items.Add(toolsMenu);
            mainMenu.Items.Add(modelMenu);
            mainMenu.Items.Add(aboutMenu);

            modelMenu.Text = Language["Model"];

            aboutMenu.Text = Language["About"];
            aboutMenu.DropDownItems.Add(aboutItem);
            aboutMenu.DropDownItems.Add(versionItem);
            aboutMenu.DropDownItems.Add(debugItem);
            aboutMenu.DropDownItems.Add(propertyItem);
            aboutItem.Text = Language["About"];
            versionItem.Text = Language["Features"];
            debugItem.Text = Language["FeedBack"];
            pluginItem.Text = Language["Plugin"];
            propertyItem.Text = Language["Setting"];

            // 
            // fileMenu
            // 
            fileMenu.DropDownItems.Add(openFileItem);
            fileMenu.DropDownItems.Add(addFileItem);
            fileMenu.DropDownItems.AddSeparator();
            fileMenu.DropDownItems.Add(saveFileItem);
            fileMenu.DropDownItems.Add(saveAsFileItem);
            fileMenu.DropDownItems.AddSeparator();
            fileMenu.DropDownItems.Add(openDirItem);
            fileMenu.DropDownItems.Add(saveDirItem);
            fileMenu.Text = Language["File"];
            openFileItem.Text = Language["Open"];
            openFileItem.ShowShortcutKeys = false;
            openFileItem.ShortcutKeys = Keys.Control | Keys.O;
            saveFileItem.Text = Language["Save"];
            saveFileItem.ShowShortcutKeys = false;
            saveFileItem.ShortcutKeys = Keys.Control | Keys.S;

            addFileItem.Text = Language["Add"];
            addFileItem.ShowShortcutKeys = false;
            addFileItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;

            saveAsFileItem.Text = Language["SaveAs"];
            saveAsFileItem.ShowShortcutKeys = false;
            saveAsFileItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;

            openDirItem.Text = Language["OpenDir"];
            openDirItem.ToolTipText = "从文件夹打开多个img/npk文件";

            saveDirItem.Text = Language["SaveDir"];
            saveDirItem.ToolTipText = "将文件分割为多个img保存至文件夹";

            editMenu.Text = Language["Edit"];
            editMenu.DropDownItems.Add(undoItem);
            editMenu.DropDownItems.Add(redoItem);
            editMenu.DropDownItems.AddSeparator();
            editMenu.DropDownItems.Add(adjustPositionItem);
            editMenu.DropDownItems.AddSeparator();
            editMenu.DropDownItems.Add(classifyItem);
            editMenu.DropDownItems.Add(sortItem);
            undoItem.Text = Language["Undo"];
            undoItem.ShortcutKeys = Keys.Control | Keys.Z;
            redoItem.Text = Language["Redo"];
            redoItem.ShortcutKeys = Keys.Control | Keys.Y;
            classifyItem.Text = Language["Classify"];
            classifyItem.CheckOnClick = true;
            sortItem.Text = Language["Sort"];
            adjustPositionItem.Text = Language["AdjustPosition"];
            adjustPositionItem.ShortcutKeys = Keys.Control | Keys.B;


            viewMenu.Text = Language["View"];
            viewMenu.DropDownItems.Add(ruleItem);
            viewMenu.DropDownItems.Add(gridItem);
            viewMenu.DropDownItems.Add(previewItem);
            viewMenu.DropDownItems.Add(mutipleLayerItem);

            ruleItem.Text = Language["Ruler"];
            ruleItem.DropDownItems.Add(displayRuleItem);
            ruleItem.DropDownItems.Add(displayRuleCrossHairItem);
            ruleItem.DropDownItems.Add(lockRuleItem);
            ruleItem.DropDownItems.Add(adjustRuleItem);

            displayRuleItem.Text = Language["DisplayRuler"];
            displayRuleItem.Checked = Config["Ruler"].Boolean;
            displayRuleItem.ShortcutKeys = Keys.Control | Keys.N;
            displayRuleItem.ShowShortcutKeys = true;
            displayRuleItem.CheckOnClick = true;
            displayRuleCrossHairItem.Text = Language["DisplayRulerCrosshair"];
            displayRuleCrossHairItem.Checked = Config["RulerCrosshair"].Boolean;
            displayRuleCrossHairItem.ToolTipText = "显示准心可以让标尺移动";
            displayRuleCrossHairItem.CheckOnClick = true;
            displayRuleCrossHairItem.Checked = true;
            adjustRuleItem.Text = Language["ResetRuler"];
            adjustRuleItem.ToolTipText = "还原标尺位置";
            lockRuleItem.Text = Language["LockRuler"];
            lockRuleItem.ToolTipText = "固定标尺，禁止移动";
            lockRuleItem.CheckOnClick = true;
            lockRuleItem.Checked = Config["RulerLocked"].Boolean;
            previewItem.Text = Language["Preview"];
            previewItem.CheckOnClick = true;
            previewItem.Checked = Config["Preview"].Boolean;
            gridItem.Text = Language["Grid"];
            gridItem.CheckOnClick = true;
            gridItem.Checked = Config["Grid"].Boolean;



            toolsMenu.Text = Language["Tools"];


            mutipleLayerItem.Text = Language["MutipleLayer"];
            mutipleLayerItem.CheckOnClick = true;
            // 
            // Message
            // 
            Messager.Location = new Point(1100, 30);
            Messager.Name = "Message";
            Messager.Size = new Size(250, 50);
            Messager.TabIndex = 9;
            Messager.Text = Language["Tips"];
            Messager.Visible = false;

            openButton.Location = new Point(20, 63);
            openButton.Text = Language["Open"];
            openButton.Size = new Size(75, 25);
            openButton.UseVisualStyleBackColor = true;

            closeButton.Location = new Point(145, 63);
            closeButton.Text = Language["Close"];
            closeButton.Size = new Size(75, 25);
            closeButton.UseVisualStyleBackColor = true;

            pathBox.Location = new Point(20, 40);
            pathBox.Size = new Size(200, 20);
            // 
            // box
            // 
            box.Location = new Point(230, 90);
            box.Name = "box";
            box.BackColor = Color.Gray;
            if (Config["CanvasSize"].Size != Size.Empty) { 
                box.Size = Config["CanvasSize"].Size;
            }
            //
            //colorPanel
            //
            colorPanel.Location = new Point(800, 48);
            colorPanel.BackColor = Config["BrushColor"].Color;
            colorPanel.Size = new Size(25, 25);
            colorPanel.BorderStyle = BorderStyle.FixedSingle;
            ///
            ///
            ///
            scaleLabel.AutoSize = true;
            scaleLabel.Text = Language["CanvasScale"];
            scaleLabel.Location = new Point(230, 53);

            ///
            ///
            ///
            scaleBox.Size = new Size(100, 40);
            scaleBox.Location = new Point(300, 50);
            scaleBox.Minimum = 20;
            scaleBox.Maximum = 100000;
            scaleBox.Value = Config["CanvasScale"].Integer;


            //
            //
            //
            pixelateBox.Text = Language["Pixelate"];
            pixelateBox.Location = new Point(420, 50);
            pixelateBox.Checked = Config["Pixelate"].Boolean;

            // 
            // realPositionBox
            // 
            realPositionBox.Location = new Point(1050, 70);
            realPositionBox.Name = "realPositionBox";
            realPositionBox.AutoSize = true;
            realPositionBox.TabIndex = 11;
            realPositionBox.Text = Language["RealPosition"];
            realPositionBox.Checked = Config["RealPosition"].Boolean;
            //
            //
            //
            displayBox.Location = new Point(1170, 70);
            displayBox.AutoSize = true;
            displayBox.Text = Language["Animation"];
            displayBox.Checked = Config["Animation"].Boolean;
            //
            //
            //
            linedodgeBox.Location = new Point(1050, 50);
            linedodgeBox.AutoSize = true;
            linedodgeBox.Text = Language["LineDodge"];
            linedodgeBox.Checked = Config["LineDodge"].Boolean;
            ///
            //
            //
            onionskinBox.Location = new Point(1170, 50);
            onionskinBox.AutoSize = true;
            onionskinBox.Text = Language["OnionSkin"];
            onionskinBox.Checked = Config["OnionSkin"].Boolean;
            //
            //
            //
            historyButton.Location = new Point(945, 47);
            historyButton.AutoSize = true;
            historyButton.Size = new Size(85, 25);
            historyButton.Text = Language["Other"];
            historyButton.UseVisualStyleBackColor = true;
            
            // 
            // searchBox
            // 
            searchBox.Location = new Point(20, 670);
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(200, 20);
            searchBox.TabIndex = 6;
            // 
            // MainForm
            // 
            trackBar.Location = new Point(1050, 370);
            trackBar.Size = new Size(240, 40);
            trackBar.LargeChange = trackBar.SmallChange = 1;

            layerList.HorizontalScrollbar = true;
            layerList.Location = new Point(1050, 405);
            layerList.Name = "layerList";
            layerList.Size = new Size(240, 280);
            layerList.TabIndex = 4;

            layerMenu.Items.Add(renameLayerItem);
            layerMenu.Items.Add(replaceLayerItem);
            // layerMenu.Items.Add(changeLayerPositionItem);
            layerMenu.Items.Add(adjustEntityPositionItem);
            layerMenu.Items.Add(loadModelItem);
            layerMenu.Items.Add(saveAsLayerItem);
            renameLayerItem.Text = Language["Rename"];
            changeLayerPositionItem.Text = Language["ChangeLayerPosition"];
            adjustEntityPositionItem.Text = Language["AdjustPosition"];
            loadModelItem.Text = Language["LoadModel"];
            saveAsLayerItem.Text = Language["SaveAs"];
            replaceLayerItem.Text = Language["ReplaceImage"];

            colorDialog = new ColorDialog();

            canvasMenu.Items.Add(canvasCutItem);
            canvasMenu.Items.Add(canvasCopyItem);
            canvasMenu.Items.Add(canvasPasteItem);

            canvasCutItem.Text = Language["Cut"];
            canvasCutItem.ShortcutKeys = Keys.Control | Keys.X;
            canvasCopyItem.Text = Language["Copy"];
            canvasCopyItem.ShortcutKeys = Keys.Control | Keys.C;
            canvasPasteItem.Text = Language["Paste"];
            canvasPasteItem.ShortcutKeys = Keys.Control | Keys.V;


            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(imageList);
            Controls.Add(mainMenu);
            Controls.Add(searchBox);
            Controls.Add(albumList);
            Controls.Add(layerList);
            Controls.Add(trackBar);
            Controls.Add(Messager);
            Controls.Add(openButton);
            Controls.Add(closeButton);
            Controls.Add(pathBox);
            Controls.Add(realPositionBox);
            Controls.Add(displayBox);
            Controls.Add(linedodgeBox);
            Controls.Add(onionskinBox);
            Controls.Add(box);
            Controls.Add(historyButton);
            Controls.Add(colorPanel);
            Controls.Add(scaleLabel);
            Controls.Add(scaleBox);
            Controls.Add(pixelateBox);
            Controls.Add(previewPanel);
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = $"{ProductName} Ver { Program.Version} { Config["Title"]}";
            AllowDrop = true;
            ClientSize = Config["MainSize"].Size;
            BackColor = Config["MainColor"].Color;
            albumListMenu.ResumeLayout(false);
            imageListMenu.ResumeLayout(false);
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();

            previewPanel.Visible = Config["Preview"].Boolean;
            previewPanel.BorderStyle = BorderStyle.FixedSingle;
            previewPanel.Size = new Size(100, 100);
            previewPanel.BackgroundImageLayout = ImageLayout.Zoom;
            previewPanel.Location = new System.Drawing.Point(930, 90);
            ((ISupportInitialize)(box)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        
        private ESListBox<Album> albumList;
        private ESListBox<Sprite> imageList;
        private ESListBox<Layer> layerList;

        private MenuStrip mainMenu;
        private ToolStripMenuItem fileMenu; 
        private ToolStripMenuItem addFileItem;
        private ToolStripMenuItem saveFileItem;
        private ToolStripMenuItem saveAsFileItem;
        private ToolStripMenuItem saveDirItem;
        private ToolStripMenuItem openDirItem;
        private ToolStripMenuItem openFileItem;

        private ToolStripMenuItem editMenu;
        private ToolStripMenuItem undoItem;
        private ToolStripMenuItem redoItem;
        private ToolStripMenuItem classifyItem;     //分类
        private ToolStripMenuItem sortItem;         //

        private ToolStripMenuItem aboutMenu;        //关于
        private ToolStripMenuItem aboutItem;        //关于
        private ToolStripMenuItem debugItem;        //问题反馈
        private ToolStripMenuItem versionItem;      //版本特性
        private ToolStripMenuItem pluginItem;       //插件拓展
        private ToolStripMenuItem propertyItem;     //设置

        private ToolStripMenuItem viewMenu;         //视图
        private ToolStripMenuItem ruleItem;         //标尺
        private ToolStripMenuItem displayRuleItem;  //显示标尺
        private ToolStripMenuItem displayRuleCrossHairItem; //显示标尺准心
        private ToolStripMenuItem adjustRuleItem;   //校正标尺
        private ToolStripMenuItem lockRuleItem;      //锁定标尺
        private ToolStripMenuItem previewItem;      //贴图预览

        private ToolStripMenuItem gridItem;         //网格
        private ToolStripMenuItem mutipleLayerItem; //多图层

        private ToolStripMenuItem toolsMenu;        //工具

        private ToolStripMenuItem modelMenu;      //模型管理

        private ContextMenuStrip albumListMenu;
        private ToolStripMenuItem newImgItem;       //新建文件
        private ToolStripMenuItem replaceItem;      //替换

        private ToolStripMenuItem editImgItem;      
        private ToolStripMenuItem cutImgItem;       //剪切
        private ToolStripMenuItem copyImgItem;      //复制
        private ToolStripMenuItem pasteImgItem;     //粘贴

        private ToolStripMenuItem saveAsItem;       //另存为
        private ToolStripMenuItem renameItem;       //重命名
        private ToolStripMenuItem convertItem;      //转换版本
        private ToolStripMenuItem hideImgItem;      //隐藏文件内所有贴图
        private ToolStripMenuItem addMergeItem;    //加入拼合队列
        private ToolStripMenuItem addOutsideMergeItem;//加入外部文件到拼合队列
        private ToolStripMenuItem runMergeItem;    //执行拼合队列
        private ToolStripMenuItem repairFileItem;    //帧数补正
        private ToolStripMenuItem splitFileItem;     //拆分文件
        private ToolStripMenuItem mixFileItem;       //合并文件

        private ContextMenuStrip imageListMenu;

        private ToolStripMenuItem editImageItem;
        private ToolStripMenuItem cutImageItem;     //剪切
        private ToolStripMenuItem copyImageItem;    //复制
        private ToolStripMenuItem pasteImageItem;   //粘贴

        private ToolStripMenuItem saveImageItem;    //提取贴图到文件夹
        private ToolStripMenuItem saveSingleImageItem;   //提取贴图
        private ToolStripMenuItem saveAllImageItem; //提取所有贴图
        private ToolStripMenuItem saveGifItem;      //保存为gif
        private ToolStripMenuItem changePositionItem;//修改坐标
        private ToolStripMenuItem changeSizeItem;    //修改大小
        private ToolStripMenuItem replaceImageItem;  //替换贴图
        private ToolStripMenuItem hideCheckImageItem;//隐藏贴图
        private ToolStripMenuItem linkImageItem;     //修改为链接贴图
        private ToolStripMenuItem newImageItem; //新建贴图;


        private ToolStripMenuItem adjustPositionItem;//校正坐标
        private ToolStripMenuItem addLayerItem; //加入图层
        private ToolStripMenuItem canvasImageItem;//画布化贴图
        private ToolStripMenuItem uncanvasImageItem;//去画布化贴图
        private ToolStripMenuItem lineDodgeItem;

        private ColorDialog colorDialog;

        
        private CheckBox realPositionBox;        //真实坐标
        private CheckBox displayBox;            //动画播放
        private CheckBox linedodgeBox;          //线性减淡
        private CheckBox onionskinBox;             //模拟残影

        private Label scaleLabel;
        private NumericUpDown scaleBox;         //画布比例
        private CheckBox pixelateBox;              //像素化


        private Button openButton;              //打开文件
        private Button closeButton;             //关闭文件
        private PictureBox box;
        private TextBox searchBox;
        private TextBox pathBox;
        private Button historyButton;           //历史操作
        private TrackBar trackBar;

        private ContextMenuStrip layerMenu;
        private ToolStripMenuItem renameLayerItem;//重命名
        private ToolStripMenuItem changeLayerPositionItem;//修改图层坐标
        private ToolStripMenuItem replaceLayerItem;//替换贴图
        private ToolStripMenuItem adjustEntityPositionItem;//校正坐标
        private ToolStripMenuItem loadModelItem;    //载入模板
        private ToolStripMenuItem saveAsLayerItem;//另存为


        private ContextMenuStrip canvasMenu;
        private ToolStripMenuItem canvasCutItem;
        private ToolStripMenuItem canvasCopyItem;
        private ToolStripMenuItem canvasPasteItem;

        private ToolStripMenuItem selectItem;
        private ToolStripMenuItem selectAllLinkItem;
        private ToolStripMenuItem selectThisLinkItm;
        private ToolStripMenuItem selectThisTargetItem;
        private ToolStripMenuItem selectAllHideItem;

        private DropPanel dropPanel;
        private OggPlayer player;
        private Panel previewPanel;
        private Panel colorPanel;
    }
}

