using System.Collections.Generic;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("PasteFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class PasteFile : InjectService, IRollback {


        private int _index;

        private int[] _indexes;

        public void Do(CommandContext context) {
            this._index = context.Get<int>();
            this.Redo();
        }

        public void Redo() {



            /*            _clipboarder = Clipboarder.Default;
                        var array = new Album[0];
                        if(_clipboarder != null) {
                            array = _clipboarder.Albums;
                            if(_clipboarder.Mode == ClipMode.Cut) {
                                Clipboarder.Clear();
                                Clipboard.Clear();
                                Store.Use<List<Album>>("/data/files", list => {
                                    list.RemoveAll(array.Contains);
                                    return list;
                                });
                            }
                            for(var i = 0; i < array.Length; i++) {
                                array[i] = array[i].Clone();
                            }
                        } else if(Clipboard.ContainsFileDropList()) {
                            var collection = Clipboard.GetFileDropList();
                            var fileArr = new string[collection.Count];
                            collection.CopyTo(fileArr, 0);
                            Controller.Do("loadFile", fileArr);
                            Store.Get(StoreKeys.LOAD_FILES, out List<Album> loads)
                                .Use<List<Album>>(StoreKeys.FILES, list => {
                                    list.InsertRange(_index, loads);
                                    return list;
                                });

                            array = loads.ToArray();

                            var builder = new LSBuilder();
                            foreach(var file in array) {
                                var name = file.Name.RemoveSuffix(".img");
                                name = name.RemoveSuffix(".ogg");
                                name += ".json";
                                if(File.Exists(name)) {
                                    var root = builder.Read(name)["path"];
                                    var path = root.Value?.ToString();
                                    if(path != null) {
                                        file.Path = path;
                                    }
                                }
                            }
                        }*/

            /*            _indexes = new int[array.Length];
                        if(array.Length > 0) {

                            Store.Use<List<Album>>("/data/files", list => {

                                if(list.Count > 0) {
                                    Store.Set("/filelist/selected-index", list.Count - 1);
                                }
                                _index = Math.Max(0, Math.Min(_index, list.Count));
                                for(var i = 0; i < array.Length; i++) {
                                    _indexes[i] = _index + i;
                                }
                                return list;

                            });

                            if(_clipboarder == null) {
                                _clipboarder = Clipboarder.CreateClipboarder(array, _indexes, ClipMode.Copy);
                            }
                        }
            */
            this.Messager.Success($"{this.Language[this.Name]}{this.Language["Success"]}");
        }

        public void Undo() {
/*            Clipboarder.Default = this._clipboarder;
            if(this._clipboarder == null) {
                return;
            }
            this.Store.Use<List<Album>>("/data/files", list => {

                var array = list.GetRange(this._index, this._indexes.Length);

                list.RemoveAll(array.Contains);
                if(this._clipboarder.Mode == ClipMode.Cut) {
                    for(var i = 0; i < array.Count; i++) {
                        list.Insert(this._indexes[i], array[i]);
                    }
                }
                return list;
            });*/
        }

        public string Name => "PasteFile";
    }
}