using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;

namespace ExtractorSharp.Command.ImageCommand {
    public class CutImage : ICommand {
        public bool CanUndo => true;

        public bool Changed => true;

        public string Name => "CutImage";

        private Album Album;
        private int[] Indexes;
        private Controller Controller => Program.Controller;
        private Clipboarder Clipborder;
        private ClipMode Mode;

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indexes = args[1] as int[];
            Mode = (ClipMode)args[2];
            Clipborder = Controller.Clipboarder;
            Controller.Clipboarder = Clipboarder.CreateClipboarder(Album, Indexes, Mode);

            var arr = new string[Indexes.Length];
            var dir = $"{Program.Config["RootPath"]}/temp/clipbord_image";
            if (Directory.Exists(dir)) {
                Directory.Delete(dir,true);
            }
            Directory.CreateDirectory(dir);
            var builder = new LSBuilder();
            for (var i = 0; i < Indexes.Length; i++) {
                var entity = Album[Indexes[i]];
                var path = $"{dir}/{entity.Index}";
                builder.WriteObject(entity, path + ".json");
                arr[i] = path + ".png";
                entity.Picture.Save(arr[i]);
            }
            var collection = new StringCollection();
            collection.AddRange(arr);
            Clipboard.SetFileDropList(collection);
        }

        public void Redo() {
            Do(Album, Indexes);
        }

        public void Undo() {
            Controller.Clipboarder = Clipborder;
        }
        
    }
}
