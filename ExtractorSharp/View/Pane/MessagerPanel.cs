using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Properties;

namespace ExtractorSharp.Components {
    /// <summary>
    ///     提示窗口
    /// </summary>
    /// 
    [Export]
    public partial class MessagePanel : Panel, IPartImportsSatisfiedNotification {


        private Timer timer;

        [Import]
        public Language Language;

        [Import]
        private Messager Messager;

        public void OnImportsSatisfied() {
            InitializeComponent();
            timer = new Timer();
            timer.Tick += Exit;
            button.Click += Exit;
            Messager.Sent += OnMessageSent;
        }



        private void OnMessageSent(MessageEventArgs e) {
            var msg = e.Message;
            var type = e.Type;

            msg = Language[msg];
            switch(type) {
                case MessageType.Error:
                    iconLabel.Image = Resources.errorIcon;
                    break;
                case MessageType.Success:
                    iconLabel.Image = Resources.successIcon;
                    break;
                case MessageType.Warning:
                    iconLabel.Image = Resources.warnningIcon;
                    msg = string.Concat(msg, " ", Language["Success"]);
                    break;
                default:
                    return;
            }
            Visible = true;
            label.Text = msg;
            timer.Interval = 5000;
            timer.Start();
        }



        /// <summary>
        ///     隐藏消息窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, EventArgs e) {
            Hide();
        }
    }
}