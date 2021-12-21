using System;
using System.Collections.Generic;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     替换IMG文件
    /// </summary>
    [ExportCommand("ReplaceFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    internal class ReplaceFile : InjectService, IRollback, IMutipleMacro {

        private Album _oldTarget = null;

        [CommandParameter("Source", IsRequired = false)]
        private Album _source = null;

        [CommandParameter("Target", IsDefault = true)]
        private Album _target = null;


        public void Do(CommandContext context) {
            context.Export(this);
            if(this._source == null) {
                this.Store.Get<List<Album>>(StoreKeys.LOAD_FILES, files => {
                    if(files != null && files.Count > 0) {
                        this._source = files[0];
                    }
                });
            }
            if(this._source == null) {
                throw new ArgumentException();
            }
            this._oldTarget = new Album();
            this._oldTarget.Replace(this._target);
            this.Redo();
            this.Messager.Success(this.Language["<ReplaceFile><Success>!"]);
        }

        public void Undo() {
            if(this._target == null || this._source == null) {
                return;
            }
            this._target.Replace(this._oldTarget);
        }


        public void Redo() {
            this._target.Replace(this._source);
        }


        public void Action(IEnumerable<Album> array) {
            foreach(var al in array) {
                al.Replace(this._source);
            }
        }

    }
}