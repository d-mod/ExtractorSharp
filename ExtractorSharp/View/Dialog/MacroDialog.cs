using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.View {
    public partial class MacroDialog : EaseDialog {
        private Controller Controller { get; }
        public MacroDialog(){
            Controller = Program.Controller;
            InitializeComponent();
            recordItem.Click += Start;
            stopItem.Click += Stop;
            CancelButton = cancelButton;
            Controller.CommandDid += DoCommand;
            runButton.Click += Run;
        }

        private void Start(object sender, EventArgs e) {
            Controller.Record();
            stopItem.Enabled = true;
            Print("开始录制");
        }

        private void Run(object sender,EventArgs e) {
            var array = new Album[0];
            if (selectImgRadio.Checked) 
                array = Controller.CheckedAlbum;
            else
                array = Controller.AllAlbum;
            Print("宏命令执行开始...");
            Print("处理数量:"+array.Length);
            Controller.Run(allImageRadio.Checked,array);
            Print("宏命令执行完成...");
        }

        private void Stop(object sender, EventArgs e) {
            Controller.Pause();
            if (Controller.IsRecord) {
                stopItem.Text = Language["Pause"];
                Print("录制继续");
            } else {
                stopItem.Text = Language["Continue"];
                Print("录制暂停");
            }
        }

        private void LoadScript(object sender,EventArgs e) {

        }

        private void SaveScript(object sender,EventArgs e) {

        }

        private void DoCommand(object sender,EventArgs e) {
            if (Controller.IsRecord&&Controller.Current is IAction action)
                Print("动作:" + action);
        }

        public void Print(string msg) {
            textBox.Text += msg + "\n";
        }
        
    }
}
