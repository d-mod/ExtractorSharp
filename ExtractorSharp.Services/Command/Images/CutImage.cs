using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("CutImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    public class CutImage : IRollback {

        [CommandParameter("File")]
        private Album file;


        [CommandParameter("Indices")]
        private int[] indices;


        [Import]
        private IConfig Config { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            var arr = new string[this.indices.Length];
            var dir = $"{this.Config["RootPath"]}/temp/clipbord_image";
            if(Directory.Exists(dir)) {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
            var builder = new LSBuilder();
            for(var i = 0; i < this.indices.Length; i++) {
                var entity = this.file[this.indices[i]];
                var path = $"{dir}/{entity.Index}";
                builder.WriteObject(entity, path + ".json");
                arr[i] = path + ".png";
                entity.Image.Save(arr[i]);
            }
            var collection = new StringCollection();
            collection.AddRange(arr);
            // Clipboard.SetFileDropList(collection);
        }

        public void Undo() {
        }
    }
}