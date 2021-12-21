using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    /// <summary>
    /// 合并多个文件
    /// </summary>

    [ExportCommand("MixFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    public class MixFile : InjectService, IRollback {

        private Album _album;

        private IEnumerable<Album> _array;

        private Album[] _list;

        public void Do(CommandContext context) {
            this._array = context.Get<IEnumerable<Album>>();
            this.Redo();
        }

        public void Redo() {
            if(this._array == null || this._array.Count() < 1) {
                return;
            }
            var first = this._array.ElementAt(0);
            var regex = new Regex("\\d+");
            var match = regex.Match(first.Name);
            if(!match.Success) {
                return;
            }
            var code = int.Parse(match.Value);
            var codeStr = Avatars.CompleteCode(code / 100 * 100);
            this._album = first.Clone();
            this._album.ConvertTo(ImgVersion.Ver6);
            this._album.Adjust();
            this._album.Palettes.Clear();
            this._album.Path = first.Path.Replace(match.Value, codeStr);
            var max = 0;
            var table = new List<Color>();
            foreach(var al in this._array) {
                if(al.CurrentPalette.Count > max) {
                    max = al.CurrentPalette.Count;
                    table = al.CurrentPalette;
                }
                this._album.Palettes.Add(al.CurrentPalette);
            }
            foreach(var tl in this._album.Palettes) {
                if(tl.Count < max) {
                    tl.AddRange(table.GetRange(tl.Count, max - tl.Count));
                }
            }
            this.Store.Use<List<Album>>(StoreKeys.FILES, list => {
                this._list = list.ToArray();
                list.RemoveAll(this._array.Contains);
                list.Add(this._album);
                return list;
            });
            this.Messager.Success(this.Language["<MixFile><Success>!"]);
        }

        public void Undo() {
            this.Store.Set(StoreKeys.FILES, new List<Album>(this._list));
        }

    }
}