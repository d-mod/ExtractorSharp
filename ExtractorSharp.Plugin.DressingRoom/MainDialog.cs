using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core;
using ExtractorSharp.Service;
using System.Diagnostics;
using ExtractorSharp.Plugin.DressingRoom.Properties;
using System.ComponentModel.Composition;

namespace ExtractorSharp.View {
    [ExportMetadata("Guid", "57951442-F28C-4D84-8677-0DE4105BFBD1")]
    [Export(typeof(ESDialog))]
    public partial class MainDialog : ESDialog {
        private string Path => Config["ResourcePath"].Value;
        private DressingService Service { get; }
        private int PartsCount => Service.Parts.Length;

        [ImportingConstructor]
        public MainDialog(IConnector Data) : base(Data) {
            Service = new DressingService(Config);
            InitializeComponent();
            Init();
            loadButton.Click += LoadModel;
            addListButton.Click += AddList;
            saveButton.Click += Save;
            addMergeButton.Click += AddMerge;
            maskCheck.CheckedChanged += (o, e) => Service.Mask = maskCheck.Checked;
            banCheck.CheckedChanged += (o, e) => Service.Ban = banCheck.Checked;
            professionBox.SelectedIndexChanged += (o, e) => PartUpdate();
            imageBox.Paint += OnPainting;
            otherButton.Click += OpenAvatar;
        }


        private void OpenAvatar(object sender,EventArgs e) {
            Process.Start(Config["AvatarUrl"].Value);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init() {
            partBoxes = new NumericUpDown[PartsCount];
            partCheckes = new CheckBox[PartsCount];
            for (var i = 0; i < PartsCount; i++) {
                var box = new NumericUpDown();
                box.Location = new Point(85 + 180 * (i % 2),  (i+2) / 2 * 45);
                box.Size = new Size(82, 21);
                box.TabIndex = i;
                box.Minimum = -1;
                box.Maximum = 99999;
                Controls.Add(box);
                partBoxes[i] = box;

                var check = new CheckBox();
                check.Location = new Point(15 + 185 * (i % 2), (i+2)  / 2 * 45);
                check.Text = Language["Alias",Service.Parts[i]];
                check.UseVisualStyleBackColor = true;
                Controls.Add(check);
                partCheckes[i] = check;
            }
            var array = Service.ProfessionList.ToArray();
            for(var i = 0; i < array.Length; i++) {
                array[i] = Language["Alias", array[i]];
            }
            professionBox.Items.AddRange(array);
            if (professionBox.Items.Count > 0) {
                professionBox.SelectedIndex = 0;
            }
            pathBox.Text = Config["GamePath"].Value;
            PartUpdate();
        }

        public override DialogResult Show(params object[] args) {
            if (args.Length > 0) {
                Import(args[0] as string);
            }
            return ShowDialog();
        }

        /// <summary>
        /// 时装刷新
        /// </summary>
        public void PartUpdate() {
            for(var i = 0; i < PartsCount; i++) {
                partBoxes[i].Value = -1;
                partCheckes[i].Checked = false;
            }
            weaponCombo.Items.Clear();
            if (professionBox.SelectedIndex > -1) {
                var arr = Service.GetWeapon(professionBox.SelectedIndex);
                for(var i = 0; i < arr.Length; i++) {
                    arr[i] = Language["Alias", arr[i]];
                }
                weaponCombo.Items.AddRange(arr);
                if (arr.Length > 0) {                 //当武器种类大于0时，默认选择第一个
                    weaponCombo.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 载入模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadModel(object sender, EventArgs e) {
            var dialog = new ESTextDialog();
            dialog.Text = Language["LoadModel"];
            if (dialog.ShowDialog() == DialogResult.OK) {
                Import(dialog.InputText);
            }
        }

        private void Import(string text) {
            var arr = text.Split("%");
            professionBox.SelectedIndex = Service.SelectProfessionByName(arr[0]);
            for (var i = 0; i < PartsCount; i++) {
                var code = int.Parse(arr[i + 1]);
                partBoxes[i].Value = code;
                if (code != -1) {
                    partCheckes[i].Checked = true;
                }
            }
            RefreshImage();
        }

        private void RefreshImage() => imageBox.Invalidate();

        private void OnPainting(object sender, PaintEventArgs e) {
            var g = e.Graphics;
            g.DrawImage(Resources.Back, 0, 0);
            var values = GetValues(true);
            var list = Service.LoadImage(values);
            var x = 800;
            var y = 600;
            var width = 0;
            var height = 0;
            foreach (var image in list) {
                if (image.Image.Width + image.X > width) {
                    width = image.Image.Width + image.X;
                }
                if (image.Image.Height + image.Y > height) {
                    height = image.Image.Height + image.Y;
                }
            }
            width /= 3;
            height /= 2;
            foreach (var image in list) {
                if (image.Image != null) {
                    g.DrawImage(image.Image, image.X - width, image.Y - height);
                }
            }
        }
        
        private void ExctractImg() => ExctractImg(true, true);

        /// <summary>
        /// 提取img
        /// </summary>
        private void ExctractImg(bool hasSkin, bool hasWeapon) {
            if (Path == null && Directory.Exists(Path)) {
                Messager.ShowWarnning("SelectPathTips");
                return;
            }
            var values = GetValues(hasSkin);
            hasWeapon &= weaponCheck.Checked;
            var value = (int)(hasWeapon ? weaponBox.Value : -1);
            var index = weaponCombo.SelectedIndex;
            Service.ExtractImg(values, value, index);
        }


        private int[] GetValues(bool hasSkin) {
            var values = new int[PartsCount];
            for (var i = 0; i < PartsCount; i++) {
                var check = partCheckes[i].Checked;
                if (i == 5) {
                    check &= hasSkin;
                }
                values[i] = (int)(check ? partBoxes[i].Value : -1);
            }
            return values;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddList(object sender, EventArgs e) {
            ExctractImg(addSkinCheck.Checked, true);
            var temp = Service.Array;
            if (hideCheck.Checked) {
                foreach (var album in temp) {
                    album.Hide();
                }
            }
            Connector.Do("addImg", temp, false);
        }


        public void Save(object sender, EventArgs e) {
            ExctractImg();
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                Tools.SaveDirectory(dialog.SelectedPath, Service.Array);
            }
        }

        /// <summary>
        /// 将试衣间的img加入拼合队列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddMerge(object sender, EventArgs e) {
            ExctractImg(true,addWeaponCheck.Checked);
            if (clearCheck.Checked) {
                Connector.Do("clearMerge");
            }
            Connector.Do("addMerge", Service.Array);
        }




 
     

    }
}
