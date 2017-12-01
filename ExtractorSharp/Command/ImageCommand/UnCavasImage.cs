using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;
using System;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 去画布化
    /// </summary>
    class UnCavasImage : ICommand, SingleAction {
        public int[] Indexes { set; get; }
        public string Name => "UnCavas";
        private Album Album;
        private Bitmap[] Images;
        private Point[] Locations;
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indexes = args[1] as int[];
            Images = new Bitmap[Indexes.Length];
            Locations = new Point[Indexes.Length];
            for (var i = 0; i < Indexes.Length; i++) {
                if (Indexes[i] > Album.List.Count - 1 || Indexes[i] < 0) {
                    continue;
                }
                var entity = Album.List[Indexes[i]];
                Images[i] = entity.Picture;
                Locations[i] = entity.Location;
                entity.UnCavasImage();
            }
            Messager.ShowOperate("UnCavasImage");
        }

        public void Redo() => Do(Album, Indexes);


        public void Undo() {
            for (var i = 0; i < Indexes.Length && i < Images.Length; i++) {
                var entity = Album.List[Indexes[i]];
                entity.ReplaceImage(entity.Type, false, Images[i]);
                entity.Location = Locations[i];
            }
        }

        public void Action(Album Album, int[] indexes) {
            foreach (var i in indexes) {
                if (i < Album.List.Count && i > -1) {
                    Album.List[i].UnCavasImage();
                }
            }
        }

        public void RunScript(string arg) { }

        public bool CanUndo => true;

        public bool Changed => true;

        public override string ToString() => Language.Default["UnCavasImage"];
    }
}
