using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     保存贴图
    ///     不可撤销
    ///     可宏命令
    /// </summary>
    /// 
    [ExportCommand("SaveImage")]
    internal class SaveImage : InjectService, ISingleMacro {

        [CommandParameter(IsDefault = true)]
        private Album file;

        [CommandParameter(IsRequired = false)]
        private int Digit = 0;

        [CommandParameter(IsRequired = false)]
        private bool FullPath;

        [CommandParameter(IsRequired = false)]
        private int Increment = -1;

        [CommandParameter(IsRequired = false)]
        private bool AllImage;

        [ImportMany(typeof(IEffect))]
        private List<IEffect> Effects;

        [CommandParameter]
        private string Target;

        [CommandParameter(IsRequired =false)]
        private string Prefix = string.Empty;

        [CommandParameter]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Action(this.file, this.Indices);
            this.Messager.Success(this.Language["<SaveImage><Success>"]);
        }


        public void Action(Album album, int[] indices) {
            //是否加入文件的路径
            var dir = $"{this.Target}/{(this.FullPath ? album.Path : album.Name)}/{this.Prefix}";
            dir = dir.Replace('\\', '/');
            var index = dir.LastIndexOf("/");
            dir = dir.Substring(0, index + 1);
            var prefix = dir.Substring(index);
            if(File.Exists(dir)) {
                dir += "_";
            }
            if(!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            if(this.AllImage) {
                indices = new int[album.List.Count];
                for(var i = 0; i < indices.Length; i++) {
                    indices[i] = i;
                }
            }
            var max = Math.Min(indices.Length, album.List.Count);
            for(var i = 0; i < max; i++) {
                if(indices[i] < 0) {
                    continue;
                }
                var sprite = album.List[indices[i]];
                var name = (this.Increment == -1 ? indices[i] : this.Increment + i).ToString();
                while(name.Length < this.Digit) {
                    name = string.Concat("0", name);
                }
                var path = $"{dir}{prefix}{name}.png"; //文件名格式:文件路径/贴图索引.png
                var image = sprite.Image;

                foreach(var effect in this.Effects) {
                    effect.Handle(sprite, ref image);
                    image = image ?? sprite.Image;
                }
                image.Save(path); //保存贴图
            }
        }

    }
}