using ExtractorSharp.Core;
using ExtractorSharp.Data;
using System.Collections.Generic;

namespace ExtractorSharp.Command.ImgCommand {
    class HideFile : IMutipleAciton {
        /// <summary>
        /// 原文件对象
        /// </summary>
        Album[] Array;
        /// <summary>
        /// 存储原文件对象和临时对象的集合
        /// </summary>
        Dictionary<Album, Album> Dic;
        public void Do(params object[] args) {
            Array = args as Album[];
            Dic = new Dictionary<Album, Album>();
            foreach (var album in Array) {
                var temp = new Album();
                temp.Replace(album);//将原文件数据保存到临时对象中
                Dic.Add(album, temp);
                album.Hide();//隐藏文件
            }
            Messager.ShowOperate("HideFile");
        }

        public void Undo() {
            foreach (var item in Array)
                item.Replace(Dic[item]);
        }

        /// <summary>
        /// 宏命令
        /// </summary>
        /// <param name="array"></param>
        public void Action(params Album[] array) {
            foreach (var album in array) {
                album.Hide();
            }
        }

        public void Redo() => Do(Array);

        public bool IsFlush => false;
        
        public bool IsChanged => true;

        public bool CanUndo => true;

        public string Name => "HideFile";
    }
}
