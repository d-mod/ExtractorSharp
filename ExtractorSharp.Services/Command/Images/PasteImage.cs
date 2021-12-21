using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {


    /// <summary>
    ///  粘贴贴图
    /// </summary>
    [ExportCommand("PastImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class PasteImage : InjectService, IRollback {

  

        [CommandParameter]
        private int Index;

        private int[] Indexes;

        private Album Source;

        [CommandParameter]
        private Album Target;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
            this.Messager.Success(this.Language["<PasteImage><Success>"]);
        }

        public void Redo() {
            /*            Clipboarder = Clipboarder.Default;
                        var array = new Sprite[0];
                        if (Clipboarder != null) {
                            Indexes = Clipboarder.Indexes;
                            Source = Clipboarder.Album;
                            array = new Sprite[Indexes.Length];
                            Source.Adjust();
                            for (var i = 0; i < array.Length; i++) {
                                array[i] = Source[Indexes[i]].Clone(Target);
                            }

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
                                    array[i].Image = image;
                                }
                            }
                        }*//*
                        Target.List.InsertRange(Index, array);*/
            this.Target.Adjust();
        }

        public void Undo() {
/*            var array = this.Target.List.GetRange(this.Index, this.Indexes.Length);
            this.Target.List.RemoveRange(this.Index, this.Indexes.Length);
            if(this.Clipboarder != null && this.Clipboarder.Mode == ClipMode.Cut) {
                for(var i = 0; i < array.Count; i++) {
                    this.Source.List.Insert(this.Indexes[i], array[i]);
                }
            }
            this.Target.Adjust();
            this.Source.Adjust();*/
        }
    }
}