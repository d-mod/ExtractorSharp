using System.Collections.Generic;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Command.Files {
    /// <summary>
    /// 保存至文件夹
    /// </summary>
    /// 
    [ExportCommand("SaveToDirectory")]
    internal class SaveToDirectory : InjectService, IMutipleMacro {

        [CommandParameter("Path", IsDefault = true)]
        private string _path;

        [CommandParameter("Files")]
        private IEnumerable<Album> _files;

        public void Action(IEnumerable<Album> array) {
            NpkCoder.SaveToDirectory(this._path, array);
        }

        public void Do(CommandContext context) {
            context.Export(this);
            if(this._files == null) {
                this.Store.Get(StoreKeys.FILES, out this._files);
            }
            this.Action(this._files);
        }
    }
}
