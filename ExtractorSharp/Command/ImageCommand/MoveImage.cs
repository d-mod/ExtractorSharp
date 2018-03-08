using System;
using System.Collections.Generic;
using ExtractorSharp.Data;

namespace ExtractorSharp.Command.ImageCommand {
    class MoveImage : ISingleAction {

        public int[] Indices { get; set; }

        public string Name => "MoveImage";

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        private int Index;

        private int Target;

        private Album Album;

        private List<Sprite> List => Album.List;

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Index = (int)args[1];
            Target = (int)args[2];
            Move(Index, Target);
            Album.AdjustIndex();
        }

        private void Move(int index, int target) {
            var item = List[index];
            List.RemoveAt(index);
            List.Insert(target, item);
        }

        public void Redo() => Do(Album, Index, Target);

        public void Undo() {
            Move(Target, Index);
            Album.AdjustIndex();
        }

        public void Action(Album Album, int[] indexes) {
            var list = new List<Sprite>();
            for (var i = 0; i < indexes.Length; i++) {
                if (indexes[i] < Album.List.Count) {
                    list.Add(Album[indexes[i]]);
                    Album.List.RemoveAt(Indices[i]);
                }
            }
            var target = Math.Min(Target, Album.List.Count);
            Album.List.InsertRange(target, list);
        }

    }
}
