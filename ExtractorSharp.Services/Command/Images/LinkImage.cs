using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     修改为索引贴图
    ///     可撤销
    ///     可宏命令
    /// </summary>
    [ExportCommand("LinkImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class LinkImage : InjectService, IRollback, ISingleMacro {

        [CommandParameter("File")]
        private Album Album;

        [CommandParameter]
        private int TargetIndex;

        private ColorFormats[] Types;

        private int[] TargetIndices;

        [CommandParameter]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                var sprite = this.Album.List[this.Indices[i]];
                sprite.ColorFormat = this.Types[i];
                sprite.TargetIndex = this.TargetIndices[i];
            }
        }

        public void Redo() {
            this.Types = new ColorFormats[this.Indices.Length];
            this.TargetIndices = new int[this.Indices.Length];
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this.TargetIndex > this.Album.List.Count - 1 || this.TargetIndex < 0 || this.TargetIndex == this.Indices[i]) {
                    continue;
                }
                var entity = this.Album.List[this.Indices[i]];
                this.Types[i] = entity.ColorFormat;
                this.TargetIndices[i] = entity.TargetIndex;
                entity.TargetIndex = this.TargetIndex;
            }
            this.Messager.Success(this.Language["<LinkImage><Success>!"]);
        }


        public void Action(Album album, int[] indexes) {
            foreach(var i in indexes) {
                //确保链接指向的文件索引正确
                if(this.TargetIndex > album.List.Count - 1 || this.TargetIndex < 0 || this.TargetIndex == i) {
                    continue; //确保链接文件索引正确
                }
                if(i > album.List.Count - 1 || i < 0) {
                    continue;
                }
                var sprite = album.List[i];
                sprite.Image = null; //删除贴图对象
                sprite.ColorFormat = ColorFormats.LINK; //修改为链接类型
                sprite.TargetIndex = this.TargetIndex; //指向指定文件
            }
        }
    }
}