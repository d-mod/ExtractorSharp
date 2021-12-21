using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///    恢复文件，将文件恢复为原始文件
    /// </summary>
    /// 
    [ExportCommand("RecoverFile")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    internal class RecoverFile : InjectService, IRollback, IMutipleMacro {

        private IEnumerable<Album> currents;

        private Album[] olds;

        private string GamePath => this.Config["GamePath"].Value;

        public void Do(CommandContext context) {
            this.currents = context.Get<IEnumerable<Album>>();
            this.Redo();
        }


        public void Redo() {
            if(this.currents == null) {
                return;
            }
            this.olds = new Album[this.currents.Count()];

            var currentImages = new List<Album>();
            var currentSounds = new List<Album>();

            if(this.currents.Count() > 0) {
                var regex = new Regex("^sounds/.*\\.ogg$");

                foreach(var file in this.currents) {
                    if(regex.IsMatch(file.Path)) {
                        currentSounds.Add(file);
                    } else {
                        currentImages.Add(file);
                    }
                }

            }

            var i = 0;
            NpkCoder.Compare(this.GamePath, NpkCoder.IMAGE_DIR, (a1, a2) => {
                var old = new Album();
                old.Replace(a2);//保存旧文件
                a2.Replace(a1); //替换为源文件
                this.olds[i++] = old;
            }, currentImages);

            NpkCoder.Compare(this.GamePath, NpkCoder.SOUND_DIR, (a1, a2) => {
                var old = new Album();
                old.Replace(a2);
                a2.Replace(a1);
                this.olds[i++] = old;
            }, currentSounds);

            this.Messager.Success(this.Language["<RecoverFile><Success>!"]);
        }


        public void Action(IEnumerable<Album> array) {
            NpkCoder.Compare(this.GamePath, (a1, a2) => a1.Replace(a2), array);
        }


        public void Undo() {
            if(this.currents == null) {
                return;
            }
            for(var i = 0; i < this.currents.Count(); i++) {
                if(this.olds[i] == null) {
                    continue;
                }
                this.currents.ElementAt(i).Replace(this.olds[i]);
            }
        }


    }
}