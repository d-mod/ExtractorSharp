using System;
using System.Collections.Generic;
using ExtractorSharp.Component;
using ExtractorSharp.Component.EventArguments;
using ExtractorSharp.Exceptions;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     窗口管理器
    /// </summary>
    public class Viewer : IDisposable {
        private readonly Dictionary<string, Type> Dic;
        private readonly Dictionary<string, ESDialog> List = new Dictionary<string, ESDialog>();

        public Viewer() {
            Dic = new Dictionary<string, Type>();
            List = new Dictionary<string, ESDialog>();
        }

        /// <summary>
        ///     释放资源
        /// </summary>
        public void Dispose() {
            foreach (var name in List.Keys) {
                List[name].Dispose();
            }
            List.Clear();
        }

        /// <summary>
        ///     窗口首次打开
        /// </summary>
        internal event DialogHandler DialogShown;

        /// <summary>
        ///     窗口打开
        /// </summary>
        internal event DialogHandler DialogShowed;

        /// <summary>
        ///     窗口关闭
        /// </summary>
        internal event DialogHandler DialogClosing;

        private void OnDialogShowed(DialogEventArgs e) {
            DialogShowed?.Invoke(this, e);
        }

        private void OnDialogShown(DialogEventArgs e) {
            DialogShown?.Invoke(this, e);
        }

        private void OnDialogClosing(DialogEventArgs e) {
            DialogClosing?.Invoke(this, e);
        }


        /// <summary>
        ///     注册窗口命令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void Registry(string name, Type type) {
            if (Dic.ContainsKey(name)) {
                Dic.Remove(name);
            }
            Dic.Add(name, type);
        }

        /// <summary>
        ///     直接将命令和窗口对象绑定
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dialog"></param>
        public void Registry(string name, ESDialog dialog) {
            List.Add(name, dialog);
            Dic.Add(name, dialog.GetType());
        }

        public void Remove(string name) {
            List.Remove(name);
            Dic.Remove(name);
        }


        /// <summary>
        ///     打开一个窗口
        /// </summary>
        /// <param name="dialogName">窗口名</param>
        /// <param name="args">参数</param>
        public void Show(string dialogName, params object[] args) {
            if (List.ContainsKey(dialogName)) {
                List[dialogName].Show(args);
            } else if (Dic.ContainsKey(dialogName)) {
                var e = new DialogEventArgs {
                    DialogType = Dic[dialogName]
                };
                OnDialogShown(e);
                if (e.Dialog != null) {
                    e.Dialog.Show(args);
                    List.Add(dialogName, e.Dialog);
                }
            } else {
                throw new CommandException("NotExistCommand");
            }
        }

        internal delegate void DialogHandler(object sender, DialogEventArgs e);
    }
}