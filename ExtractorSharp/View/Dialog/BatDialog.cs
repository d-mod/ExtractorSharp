using System.Collections.Generic;
using ExtractorSharp.UI;
using ExtractorSharp.Bat;
using System;
using System.Threading;
using ExtractorSharp.Command;
using ExtractorSharp.Data;
using ExtractorSharp.Core;

namespace ExtractorSharp.View {
    public partial class BatDialog : EaseDialog {
        List<Batch> batList = new List<Batch>();
        List<ImageEntity> List = new List<ImageEntity>();
        bool Running;
        Batch Bat;
        Controller Controller { get; }
        public BatDialog() {
            Controller = Program.Controller;
            batList.Add(new BatDye());
            batList.Add(new BatReplace());
            batList.Add(new LinearDodge());
            batList.Add(new Linghting());
            batList.Add(new BatCavas());
            batList.Add(new Split());
            InitializeComponent();
            comboBox1.Items.AddRange(batList.ToArray());
            comboBox1.SelectedIndex = 0;
            Bat = batList[0];
            button1.Click += Run;
        }

        private void Run(object sender,EventArgs e) {
            if (Running) {
                Running = false;
            } else {
                button1.Text = "中止";
                Running = true;
                List = new List<ImageEntity>();
                if (selectAlbumRadio.Checked) {
                    var array = Controller.CheckedAlbum;
                    foreach (var al in array)
                        List.AddRange(al.List);
                } else if (allAlbumRadio.Checked) {
                    var array = Controller.AllAlbum;
                    foreach (var al in array)
                        List.AddRange(al.List);
                } else if (selectImageRadio.Checked)
                    List.AddRange(Controller.CheckedImage);
                else if (allImageRadio.Checked)
                    List.AddRange(Controller.AllImage);
                Bat = comboBox1.SelectedItem as Batch;
                bar.Maximum = List.Count;
                bar.Value = 0;
                bar.Visible = true;
                if (List.Count > 0)
                    Bat.Run(Controller, List.ToArray(), ref Running, bar);
                bar.Visible = false;
                button1.Text = "执行";
                Running = false;
            }
        }



    }
}
