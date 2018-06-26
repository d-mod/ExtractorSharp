using System.Collections.Generic;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.FileCommand {
    internal class HideFile : IMutipleAciton, ICommandMessage {
        /// <summary>
        ///     原文件对象
        /// </summary>
        private Album[] _array;

        /// <summary>
        ///     存储原文件对象和临时对象的集合
        /// </summary>
        private Dictionary<Album, Album> Dic;

        public void Do(params object[] args) {
            _array = args as Album[];
            if (_array == null) {
                return;
            }
            Dic = new Dictionary<Album, Album>();
            foreach (var album in _array) {
                var temp = new Album();
                temp.Replace(album); //将原文件数据保存到临时对象中
                Dic.Add(album, temp);
                album.Hide(); //隐藏文件
            }
        }

        public void Undo() {
            foreach (var item in _array) {
                item.Replace(Dic[item]);
            }
        }

        /// <summary>
        ///     宏命令
        /// </summary>
        /// <param name="array"></param>
        public void Action(params Album[] array) {
            foreach (var album in array) album.Hide();
        }

        public void Redo() {
            Do(_array);
        }

        public bool IsChanged => true;

        public bool CanUndo => true;

        public string Name => "HideFile";
    }
}