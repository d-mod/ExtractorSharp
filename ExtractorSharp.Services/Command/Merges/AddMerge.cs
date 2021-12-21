using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     加入拼合
    /// </
    [ExportCommand("AddMerge")]
    internal class AddMerge : InjectService, IRollback, IMutipleMacro {

        private IEnumerable<Album> files;

        public void Do(CommandContext context) {
            context.Get(out this.files);
            if(this.files == null) {
                this.Store.Get(StoreKeys.LOAD_FILES, out this.files, new Album[0]);
            }
            this.Action(this.files);
            this.Messager.Success(this.Language["<AddMerge><Success>"]);
        }

        public void Undo() {
            this.Store.Use(StoreKeys.MERGE_QUEUES, queues => {
                if(this.files != null) {
                    queues.RemoveAll(this.files.Contains);
                }
                return queues;
            }, new List<Album>());
        }


        public void Redo() {
            this.Action(this.files);
        }


        public void Action(IEnumerable<Album> array) {
            this.Store.Use("/merge/queue", queues => {
                if(array != null) {
                    queues.AddRange(array);
                }
                return queues;
            }, new List<Album>());
        }

    }
}