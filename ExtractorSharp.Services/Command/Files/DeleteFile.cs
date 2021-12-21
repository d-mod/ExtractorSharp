using System.Collections.Generic;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("DeleteFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    public class DeleteFile : InjectService, IRollback, IMutipleMacro {

        private Dictionary<Album, int> _indices;

        private IEnumerable<Album> _array;

        /// <summary>
        ///     执行
        /// </summary>
        /// <param name="args"></param>
        public void Do(CommandContext context) {
            this._array = context.Get<IEnumerable<Album>>();
            this.Redo();
        }

        public void Undo() {
            if(this._indices.Count > 0) {
                this.Store.Use<List<Album>>("/data/files", list => {
                    foreach(var album in this._indices.Keys) {
                        var index1 = this._indices[album];
                        if(index1 < list.Count - 1 && index1 > -1) {
                            list.Insert(index1, album);
                        } else {
                            list.Add(album);
                        }

                    }
                    return list;
                });

            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     重做
        /// </summary>
        public void Redo() {
            if(this._array == null || this._array.Count() == 0) {
                this.Messager.Error("ArgumentsError");
                return;
            }

            this._indices = new Dictionary<Album, int>();

            this.Store.Use<List<Album>>(StoreKeys.FILES, list => {
                foreach(var file in this._array) {
                    var index = list.IndexOf(file);
                    if(index > 0) {
                        this._indices.Add(file, index);
                    }
                }
                list.RemoveAll(this._array.Contains);
                return list;
            });

            this.Messager.Success("<DeleteFile><Success>!");
        }

        public void Action(IEnumerable<Album> array) {
            this.Store.Use<List<Album>>("/data/files", list => {
                list.RemoveAll(array.Contains);
                return list;
            });
        }

    }
}