using System;
using System.Collections.Generic;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <inheritdoc />
    /// <summary>
    ///     文件排序
    /// </summary>
    /// 
    [ExportCommand("SortFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class SortFile : InjectService, IRollback {

        private List<Album> _list;


        public void Do(CommandContext context) {
            this.Redo();
        }

        public void Redo() {
            this.Store.Use<List<Album>>(StoreKeys.FILES, list => {
                this._list = new List<Album>(list);
                list.Sort(this.Comparision);
                return list;
            });
            this.Messager.Success(this.Language["<SortFile><Success>!"]);
        }


        public void Undo() {
            this.Store.Set(StoreKeys.FILES, new List<Album>(this._list));
            //将原文件数组还原到文件列表里
        }

        public int Comparision(Album al1, Album al2) {
            var cs1 = al1.Path.ToCharArray();
            var cs2 = al2.Path.ToCharArray();
            var i = Math.Min(cs1.Length, cs2.Length) - 1;
            for(var j = 0; j < i; j++) {
                if(cs1[i] < cs2[i]) {
                    return -1;
                }
                if(cs1[i] > cs2[i]) {
                    return 1;
                }
            }
            return 0;
        }
    }
}