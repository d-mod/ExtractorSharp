using System;
using System.Windows.Forms;
using ExtractorSharp.Component.Properties;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Component {
    /// <summary>
    ///     提示窗口
    /// </summary>
    public partial class ESMessager : Panel {
        private readonly Timer timer;


        public ESMessager(IConnector Connector) {
            this.Connector = Connector;
            Language = Connector.Language;
            InitializeComponent();
            timer = new Timer();
            timer.Tick += Exit;
            button.Click += Exit;
        }

        private IConnector Connector { get; }
        private Language Language { get; }

        public void ShowError(string msg) {
            ShowMessage(MessageType.Error, Language[msg]);
        }

        /// <summary>
        ///     警告消息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowWarnning(string msg) {
            ShowMessage(MessageType.Warning, Language[msg]);
        }

        /// <summary>
        ///     操作成功时触发的消息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowOperate(string msg) {
            ShowMessage(MessageType.Success, $"{Language[msg]}  {Language["Success"]}");
        }

        /// <summary>
        ///     提示
        /// </summary>
        /// <param name="type">提示类型</param>
        /// <param name="msg">提示信息</param>
        /// <param name="code">提示码</param>
        public void ShowMessage(MessageType type, string msg) {
            switch (type) {
                case MessageType.Error:
                    iconLabel.Image = Resources.errorIcon;
                    break;
                case MessageType.Success:
                    iconLabel.Image = Resources.successIcon;
                    break;
                case MessageType.Warning:
                    iconLabel.Image = Resources.warnningIcon;
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