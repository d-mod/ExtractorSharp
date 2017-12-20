using System;
using System.Collections.Generic;
using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.View;
using System.Reflection;
using ExtractorSharp.Core;

namespace ExtractorSharp {
    /// <summary>
    /// 窗口管理器
    /// </summary>
    public class Viewer : IDisposable {
        private Dictionary<string, EaseDialog> List = new Dictionary<string, EaseDialog>();
        private Dictionary<string, Type> Dic;

        internal delegate void DialogHandler(object sender, DialogEventArgs e);

        /// <summary>
        /// 窗口首次打开
        /// </summary>
        internal event DialogHandler DialogShown;
        /// <summary>
        /// 窗口打开
        /// </summary>
        internal event DialogHandler DialogShowed;
        /// <summary>
        /// 窗口关闭
        /// </summary>
        internal event DialogHandler DialogClosing;

        private void OnDialogShowed(DialogEventArgs e) => DialogShowed?.Invoke(this, e);

        private void OnDialogShown(DialogEventArgs e) => DialogShown?.Invoke(this, e);

        private void OnDialogClosing(DialogEventArgs e) => DialogClosing?.Invoke(this, e);

        public Viewer() {
            Dic = new Dictionary<string, Type>();
            List = new Dictionary<string, EaseDialog>();
        }


        /// <summary>
        /// 注册窗口命令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void Regisity(string name, Type type) {
            if (Dic.ContainsKey(name)) {
                Dic.Remove(name);
            }
            Dic.Add(name, type);
        }

        public void RegisityPlugin() {
            foreach (var lazy in Program.Hoster.DialogList) {
                var dialog = lazy;
                List.Add(dialog.Name, dialog);
            }
        }


        /// <summary>
        /// 打开一个窗口
        /// </summary>
        /// <param name="dialogName">窗口名</param>
        /// <param name="args">参数</param>
        public void Show(string dialogName, params object[] args) {
            if (List.ContainsKey(dialogName)) {
                List[dialogName].Show(args);
            } else if (Dic.ContainsKey(dialogName)) {
                var e = new DialogEventArgs();
                e.DialogType = Dic[dialogName];
                OnDialogShown(e);
                if (e.Dialog != null) {
                    e.Dialog.Show(args);
                    List.Add(dialogName, e.Dialog);
                }
            } else {
                Messager.ShowError("NotExistCommand");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            foreach (var name in List.Keys)
                List[name].Dispose();
            List.Clear();
        }
    }
}
