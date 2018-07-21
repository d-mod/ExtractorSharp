using System;
using System.Collections.Generic;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    internal class MoveImage : ISingleAction {
        private Album Album;

        private int Index;

        private int Target;

        private List<Sprite> List => Album.List;

        public int[] Indices { get; set; }

        public string Name => "MoveImage";

        public bool CanUndo => true;

        public bool IsChanged => true;

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Index = (int) args[1];
            Target = (int) args[2];
            Move(Index, Target);
            Album.AdjustIndex();
        }

        public void Redo() {
            Do(Album, Index, Target);
        }

        public void Undo() {
            Move(Target, Index);
            Album.AdjustIndex();
        }

        public void Action(Album album, int[] indexes) {
            var list = new List<Sprite>();
            for (var i = 0; i < indexes.Length; i++) {
                if (indexes[i] < album.List.Count) {
                    list.Add(album[indexes[i]]);
                    album.List.RemoveAt(Indices[i]);
                }
            }

            var target = Math.Min(Target, album.List.Count);
            album.List.InsertRange(target, list);
        }

        private void Move(int index, int target) {
            var item = List[index];
            List.RemoveAt(index);
            List.Insert(target, item);
        }
    }
}