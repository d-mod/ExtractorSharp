using System.Collections.Generic;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("SaveFile")]
    public class SaveFile : InjectService, IMutipleMacro {

        private bool isSaveAll;

        [CommandParameter("Path", IsDefault = true)]
        private string _path;

        [CommandParameter("Files", IsRequired = false)]
        private IEnumerable<Album> _files;

        public void Action(IEnumerable<Album> array) {
            if(string.IsNullOrEmpty(this._path)) {
                return;
            }
            NpkCoder.Save(this._path, new List<Album>(array));
        }

        public void Do(CommandContext context) {
            context.Export(this);

            if(this._files == null) {
                this.Store.Get(StoreKeys.FILES, out this._files);
                this.isSaveAll = true;
            }

            this.Action(this._files);
            if(this.isSaveAll) {
                this.Store.Set(StoreKeys.IS_SAVED, true)
                    .Use<List<string>>(StoreKeys.RECENTS, list => {
                        list.Add(this._path);
                        return list.Distinct().ToList();
                    });
            }
            this.Messager.Success(this.Language["<SaveFile><Success>!"]);

        }
    }
}