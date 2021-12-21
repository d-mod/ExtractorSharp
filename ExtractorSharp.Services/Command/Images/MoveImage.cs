using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    /// <summary>
    /// 移动贴图在列表中的位置
    /// </summary>

    [ExportCommand("MoveImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class MoveImage : IRollback, ISingleMacro {

        [CommandParameter]
        private Album Album;

        [CommandParameter]
        private int Index;

        [CommandParameter]
        private int Target;

        private List<Sprite> List => this.Album.List;

        [CommandParameter]
        public int[] Indices { get; set; }


        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this.Move(this.Index, this.Target);
            this.Album.AdjustIndex();
        }

        public void Undo() {
            this.Move(this.Target, this.Index);
            this.Album.AdjustIndex();
        }

        public void Action(Album album, int[] indexes) {
            var list = new List<Sprite>();
            for(var i = 0; i < indexes.Length; i++) {
                if(indexes[i] < album.List.Count) {
                    list.Add(album[indexes[i]]);
                    album.List.RemoveAt(this.Indices[i]);
                }
            }

            var target = Math.Min(this.Target, album.List.Count);
            album.List.InsertRange(target, list);
        }

        private void Move(int index, int target) {
            var item = this.List[index];
            this.List.RemoveAt(index);
            this.List.Insert(target, item);
        }
    }
}