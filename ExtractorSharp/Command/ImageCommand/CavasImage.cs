using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;
using System;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 画布化
    /// 可撤销
    /// 可宏命令
    /// </summary>
    class CavasImage : ICommand,SingleAction {
        Album Album;
        Size Size;
        Bitmap[] Images;
        Point[] Locations;
        public int[] Indexes { set; get; }
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Size = (Size)args[1];
            Indexes = args[2] as int[];
            Images = new Bitmap[0];
            Locations = new Point[0];
            Images = new Bitmap[Indexes.Length];
            Locations = new Point[Indexes.Length];
            for (var i = 0; i < Indexes.Length; i++) {
                if (Indexes[i] > Album.List.Count - 1 || Indexes[i] < 0)
                    continue;
                var entity = Album.List[Indexes[i]];
                Images[i] = entity.Picture;
                Locations[i] = entity.Location;
                entity.CavasImage(Size);
            }
            Messager.ShowOperate("CavasImage");
        }

        public void Redo() => Do(Album, Size, Indexes);


        public void Undo() {
            for (var i = 0; i < Indexes.Length && i < Images.Length; i++) {
                var entity = Album.List[Indexes[i]];
                entity.ReplaceImage(entity.Type, false, Images[i]);
                entity.Location = Locations[i];
            }
        }

        public void Action(Album Album, int[] indexes) {
            foreach (var i in indexes)
                if (i < Album.List.Count && i > -1)
                    Album.List[i].CavasImage(Size);
        }

        public void RunScript(string arg) { }

        public bool CanUndo => true;

        public bool Changed => true;

        public override string ToString() => Language.Default["Cavas"];
    }
}
