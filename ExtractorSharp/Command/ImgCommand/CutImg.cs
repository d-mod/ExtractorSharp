using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Command.ImgCommand {
    class CutImg : ICommand {
        public bool CanUndo => true;

        public bool Changed => false;

        private Controller Controller => Program.Controller;

        private Clipboarder Clipboarder;

        private Album[] Array;

        private int[] Indexes;

        private ClipMode Mode;



        public void Do(params object[] args) {
            Array = args[0] as Album[];
            Indexes = args[1] as int[];
            Mode = (ClipMode)args[2];
            Clipboarder = Controller.Clipboarder;
            Controller.Clipboarder = Clipboarder.CreateClipboarder(Array, Indexes, Mode);
            var builder = new LSBuilder();
            var dir = $"{Application.StartupPath}/temp/clipbord_img";
            if (Directory.Exists(dir)) {
                //删除文件夹内容
                Directory.Delete(dir,true);
            }
            Directory.CreateDirectory(dir);
            var path_arr = new string[Array.Length];
            for (var i = 0; i < Array.Length; i++) {
                path_arr[i] = $"{dir}/{Array[i].Name}";
                Tools.SaveFile(path_arr[i], Array[i]);
                var json_path = path_arr[i].RemoveSuffix(".ogg");
                json_path = json_path.RemoveSuffix(".img");
                json_path = $"{json_path}.json";
                var root = new LSObject();
                root.Add("path", Array[i].Path);
                builder.Write(root, json_path);
            }
            var collection = new StringCollection();
            collection.AddRange(path_arr);
            Clipboard.SetFileDropList(collection);
        }

        public void Redo() {
           Do(Array, Mode);
        }

        public void Undo() {
            Controller.Clipboarder = Clipboarder;
        }

        public override string ToString() => Language.Default["Cut"];
    }
}
