using System.Drawing;
using System.IO;
using System.Windows;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;

namespace ExtractorSharp.Command.ImageCommand {
    public class PasteImage : ICommand {
        private Clipboarder Clipboarder;

        private int Index;

        private int[] Indexes;

        private Album Source, Target;

        public string Name => "PasteImage";

        public bool CanUndo => true;

        public bool IsChanged => true;
        
        public void Do(params object[] args) {
            Target = args[0] as Album;
            Index = (int) args[1];
            Clipboarder = Clipboarder.Default;
            Redo();
        }

        public void Redo() {
            var array = new Sprite[0];
            if (Clipboarder != null) {
                Indexes = Clipboarder.Indexes;
                Source = Clipboarder.Album;
                array = new Sprite[Indexes.Length];
                Source.Adjust();
                for (var i = 0; i < array.Length; i++) array[i] = Source[Indexes[i]].Clone(Target);
                if (Clipboarder.Mode == ClipMode.Cut) {
                    //如果是剪切，清空剪切板
                    Clipboarder.Default = null;
                    Clipboard.Clear();
                    for (var i = 0; i < array.Length; i++) {
                        Source.List.Remove(array[i]);
                    }
                }
                Source.Adjust();
            } else if (Clipboard.ContainsFileDropList()) {
                var collection = Clipboard.GetFileDropList();
                array = new Sprite[collection.Count];
                var builder = new LSBuilder();
                for (var i = 0; i < collection.Count; i++) {
                    if (!File.Exists(collection[i])) {
                        continue;
                    }
                    var image = Image.FromFile(collection[i]) as Bitmap;
                    var json = collection[i].Replace(".png", ".json");
                    if (File.Exists(json)) {
                        var obj = builder.Read(json);
                        array[i] = obj.GetValue(typeof(Sprite)) as Sprite;
                        array[i].Parent = Target;
                        array[i].Picture = image;
                    }
                }
            }
            Target.List.InsertRange(Index, array);
            Target.Adjust();
        }

        public void Undo() {
            var array = Target.List.GetRange(Index, Indexes.Length);
            Target.List.RemoveRange(Index, Indexes.Length);
            if (Clipboarder != null && Clipboarder.Mode == ClipMode.Cut) {
                for (var i = 0; i < array.Count; i++) {
                    Source.List.Insert(Indexes[i], array[i]);
                }
            }
            Target.Adjust();
            Source.Adjust();
        }
    }
}