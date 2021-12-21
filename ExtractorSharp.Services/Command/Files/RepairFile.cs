using System;
using System.Collections.Generic;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     修复文件，将文件缺少的贴图恢复为原始文件
    /// </summary>
    /// 
    [ExportCommand("RepairFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class RepairFile : InjectService, IRollback, IMutipleMacro {


        private IEnumerable<Album> array;

        private int[] counts;

        [StoreBinding("/config/data/game-path")]
        private string GamePath;

        public void Do(CommandContext context) {
            context.Get(out this.array);
            if(this.array == null) {
                throw new ArgumentException();
            }
            this.Redo();
        }


        public void Redo() {

            this.counts = new int[this.array.Count()];
            var i = 0;
            NpkCoder.Compare(this.GamePath, (a1, a2) => {
                this.counts[i] = a1.List.Count - a2.List.Count;
                if(this.counts[i] > 0) {
                    var source = a1.List.GetRange(a2.List.Count, this.counts[i]); //获得源文件比当前文件多的贴图集合
                    foreach(var e in source) {
                        e.Load();
                        e.Parent = a2;
                    }

                    a2.List.AddRange(source); //加入到当前文件中,不修改原贴图。
                }

                i++;
            }, this.array);
            this.Messager.Success(this.Language["<RepairFile><Success>!"]);
        }


        public void Action(IEnumerable<Album> array) {
            NpkCoder.Compare(this.GamePath, (a1, a2) => {
                var count = a1.List.Count - a2.List.Count;
                if(count <= 0) {
                    return;
                }
                var source = a1.List.GetRange(a2.List.Count, count);
                source.ForEach(e => {
                    e.Load();
                    e.Parent = a2;
                });
                a2.List.AddRange(source);
            }, array);
        }


        public void Undo() {
            for(var i = 0; i < this.array.Count(); i++) {
                var item = this.array.ElementAt(i);
                if(this.counts[i] > 0 && this.counts[i] <= item.List.Count) {//从文件中移除修复的贴图
                    item.List.RemoveRange(item.List.Count - this.counts[i], this.counts[i]);
                }
            }
        }
    }
}