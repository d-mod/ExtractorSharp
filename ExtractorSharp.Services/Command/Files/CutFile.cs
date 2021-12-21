using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;

namespace ExtractorSharp.Services.Commands {


    [ExportCommand("CutFile")]
    public class CutFile : InjectService, IRollback {


        [Import]
        private IClipboad Clipboad;

        [CommandParameter("Files")]
        private Album[] _array;

        [CommandParameter("Mode")]
        private ClipMode _mode;

        [StoreBinding("/config/data/app-dir")]
        public string AppDir;

        [StoreBinding("/app/session-id")]
        public string SessionId;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {

            Store.Get("/data/clipboader", out object clipboader);


            var builder = new LSBuilder();
            var dir = $"{this.AppDir}/temp/clipbord_img/{SessionId}";
            if(Directory.Exists(dir)) {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
            if(this._array != null) {
                var pathArr = new string[this._array.Length];
                for(var i = 0; i < this._array.Length; i++) {
                    pathArr[i] = $"{dir}/{this._array[i].Name}";
                    this._array[i].Save(pathArr[i]);
                    var jsonPath = pathArr[i].RemoveSuffix(".ogg");
                    jsonPath = jsonPath.RemoveSuffix(".img");
                    jsonPath = $"{jsonPath}.json";
                    var root = new LSObject {
                        {"path", this._array[i].Path}
                    };
                    builder.Write(root, jsonPath);
                }

                var collection = new StringCollection();
                collection.AddRange(pathArr);
                Clipboad.SetFileDropList(collection);
                
                /*
                Clipboard.SetFileDropList(collection);*/
            }
        }

        public void Undo() {
        }
    }
}