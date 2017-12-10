using System;
using ExtractorSharp.UI;
using ExtractorSharp.Core;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View {
    public partial class MacroDialog : EaseDialog {
        private Controller Controller { get; }
        public MacroDialog(){
            Controller = Program.Controller;
            InitializeComponent();
            recordItem.Click += Start;
            stopItem.Click += Stop;
            CancelButton = cancelButton;
            Controller.ActionChanged += DoCommand;
            runButton.Click += Run;
        }

        private void Start(object sender, EventArgs e) {
            Controller.Record();
            stopItem.Enabled = true;
            Print("开始录制");
        }

        private void Run(object sender, EventArgs e) {
            var array = selectImgRadio.Checked ? Controller.CheckedAlbum : Controller.AllAlbum;
            Print("宏命令执行开始...");
            Print("处理数量:" + array.Length);
            Controller.Run(allImageRadio.Checked, array);
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


        private void DoCommand(object sender, ActionEventArgs e) {
            if (e.Mode == QueueChangeMode.Add) {
                Print(Language["Action"] + ":" + Language[e.Action.Name]);
            }
        }

        public void Print(string msg) {
            textBox.Text += msg + "\n";
        }
        
    }
}
