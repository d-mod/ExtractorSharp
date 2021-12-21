using System.Collections.Generic;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("HideFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    public class HideFile : InjectService, IRollback, IMutipleMacro {
        /// <summary>
        ///     原文件对象
        /// </summary>
        private IEnumerable<Album> _array = new List<Album>();

        /// <summary> 
        ///     存储原文件对象和临时对象的集合
        /// </summary>
        private Dictionary<Album, Album> dictionary;

        public void Do(CommandContext context) {
            this._array = context.Get<IEnumerable<Album>>();
            if(this._array?.Count() > 0) {
                this.Redo();
            }
        }

        public void Undo() {
            foreach(var item in this._array) {
                item.Replace(this.dictionary[item]);
            }
        }

        /// <summary>
        ///     宏命令
        /// </summary>
        /// <param name="array"></param>
        public void Action(IEnumerable<Album> array) {
            foreach(var album in array) {
                album.Hide();
            }
        }

        public void Redo() {
            this.dictionary = new Dictionary<Album, Album>();
            foreach(var album in this._array) {
                var temp = new Album();
                temp.Replace(album); //将原文件数据保存到临时对象中
                this.dictionary.Add(album, temp);
                album.Hide(); //隐藏文件
            }
            this.Messager.Success(this.Language["<HideFile><Success>!"]);
        }

    }
}