using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtractorSharp.Command.ImageCommand {
    public class PasteImage : ICommand {
        private Album Source,Target;
        private int[] Indexes;
        private int Index;
        private Controller Controller => Program.Controller;
        private Clipboarder Clipboarder;

        public string Name => "PasteImage";

        public bool CanUndo => true;

        public bool Changed => true;


        public void Do(params object[] args) {
            Target = args[0] as Album;
            Index = (int)args[1];
            Clipboarder = Controller.Clipboarder;
            var array = new ImageEntity[0];
            if (Clipboarder != null) {
                Indexes = Clipboarder.Indexes;
                Source = Clipboarder.Album;
                array = new ImageEntity[Indexes.Length];
                Source.Adjust();
                for (var i = 0; i < array.Length; i++) {
                    array[i] = Source[Indexes[i]].Clone(Target);
                }
                if (Clipboarder.Mode == ClipMode.Cut) {
                    //如果是剪切，清空剪切板
                    Controller.Clipboarder = null;
                    Clipboard.Clear();
                    for (var i = 0; i < array.Length; i++) {
                        Source.List.Remove(array[i]);
                    }
                }
                Source.Adjust();
            } else if (Clipboard.ContainsFileDropList()) {
                var collection = Clipboard.GetFileDropList();
                array = new ImageEntity[collection.Count];
                var builder = new LSBuilder();
                for (var i = 0; i < collection.Count; i++) {
                    if (!File.Exists(collection[i])) {
                        continue;
                    }
                    var image = Image.FromFile(collection[i]) as Bitmap;
                    var json = collection[i].Replace(".png", ".json");
                    if (File.Exists(json)) {
                        var obj = builder.Read(json);
                        array[i] = obj.GetValue(typeof(ImageEntity)) as ImageEntity;
                        array[i].Parent = Target;
                        array[i].Picture = image;
                    }
                }
            }
            Target.List.InsertRange(Index, array);
            Target.Adjust();
        }

        public void Redo() {
            Do(Source, Target, Index);
        }

        public void Undo() {
            var array = Target.List.GetRange(Index, Indexes.Length);
            Target.List.RemoveRange(Index, Indexes.Length);
            if (Clipboarder.Mode == ClipMode.Cut) {
                for (var i = 0; i < array.Count; i++) {
                    Source.List.Insert(Indexes[i], array[i]);
                }
            }
            Target.Adjust();
            Source.Adjust();
        }
        
    }
}
