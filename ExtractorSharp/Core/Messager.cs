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
        private static Language Language => Language.Default;
        public static Messager Default { get; } = new Messager();
        private Timer timer = new Timer();

        private Messager() {
            InitializeComponent();
            timer.Tick += Exit;
            button.Click += Exit;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public static void ShowMessage(Msg_Type type, string msg) => Default?.Show(type, msg);

        public static void ShowError(string msg) => ShowMessage(Msg_Type.Error, Language[msg]);

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowWarnning(string msg) => ShowMessage(Msg_Type.Warning, Language[msg]);

        /// <summary>
        /// 操作成功时触发的消息
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowOperate(string msg) => ShowMessage(Msg_Type.Operate, Language[msg] + Language["Success"]);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="type">提示类型</param>
        /// <param name="msg">提示信息</param>
        /// <param name="code">提示码</param>
        public void Show(Msg_Type type,string msg) {
            switch (type) {
                case Msg_Type.Error:
                    BackColor = Color.Red;
                    break;
                case Msg_Type.Operate:
                    BackColor = Color.FromArgb(150,200,150);
                    break;
                case Msg_Type.Warning:
                    BackColor = Color.Yellow;
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
        public void Exit(object sender, EventArgs e) => Hide();
        
    }
}
