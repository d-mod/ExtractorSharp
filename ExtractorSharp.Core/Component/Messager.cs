using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Properties;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Core{
    /// <summary>
    /// 提示类型
    /// </summary>
    public enum Msg_Type {
        /// <summary>
        /// 错误！
        /// </summary>
        Error,
        /// <summary>
        /// 警告！
        /// </summary>
        Warning,
        /// <summary>
        /// 完成！
        /// </summary>
        Operate
    }
    /// <summary>
    /// 提示窗口
    /// </summary>
    public partial class Messager : Panel {
        public static Messager Default { get; } = new Messager();

        private Timer timer;

        public IConnector Connector { set; get; }
        public Language Language { set; get; } = Language.Default;


        private Messager() {
            InitializeComponent();
            timer = new Timer();
            timer.Tick += Exit;
            button.Click += Exit;        
        }

        protected override void OnVisibleChanged(EventArgs e) {

        }


        public static void ShowOperate(string msg) => Default?._ShowOperate(msg);

        public static void ShowWarnning(string msg) => Default?._ShowWarnning(msg);

        public static void ShowError(string msg) => Default?._ShowError(msg); 

        public static void ShowMessage(Msg_Type type, string msg) => Default?._ShowMessage(type, msg);


        private void _ShowError(string msg) => _ShowMessage(Msg_Type.Error, Language[msg]);

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="msg"></param>
        private void _ShowWarnning(string msg) => _ShowMessage(Msg_Type.Warning, Language[msg]);

        /// <summary>
        /// 操作成功时触发的消息
        /// </summary>
        /// <param name="msg"></param>
        private void _ShowOperate(string msg) {
            _ShowMessage(Msg_Type.Operate, $"{Language[msg]}  {Language["Success"]}");
            Bass.Play(Resources.end);
        }
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="type">提示类型</param>
        /// <param name="msg">提示信息</param>
        /// <param name="code">提示码</param>
        public void _ShowMessage(Msg_Type type,string msg) {
            switch (type) {
                case Msg_Type.Error:
                    iconLabel.Image = Resources.errorIcon;
                    break;
                case Msg_Type.Operate:
                    iconLabel.Image = Resources.successIcon;
                    break;
                case Msg_Type.Warning:
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
        /// 隐藏消息窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, EventArgs e) => Hide();
        
    }
}
