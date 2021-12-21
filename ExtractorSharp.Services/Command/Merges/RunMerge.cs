using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {



    [ExportCommand("RunMerge")]
    internal class RunMerge : InjectService, ICommand {
        private IProgress<ProgressEventArgs> Progress;

        public void Do(CommandContext context) {
            context.Get(out this.Progress);
            if(this.Progress == null) {
                this.Progress = new MergeProcess(this.Store);
            }

            this.Store.Get("/merge/queue", out List<Album> queues)
                .Get("/merge/version", out int targetVersion);
            var list = queues.ToList();

            // PreHandles.ForEach(e => version = e.Invoke(list, version));
            list.Reverse(); //序列反转
            var count = 0;
            var version = targetVersion > 0 ? (ImgVersion)targetVersion : ImgVersion.Ver2;
            foreach(var al in list) {
                if(al.List.Count > count) {
                    count = al.List.Count;
                }
                if(targetVersion == 0 && al.Version > version) {
                    version = al.Version;
                }
            }
            var file = new Album {
                Version = version
            };
            file.InitHandle(null);
            var eventArgs = new ProgressEventArgs {
                Maximum = count + 1
            };
            this.Progress.Report(eventArgs); //启动拼合事件
            var sprites = new List<Sprite>();
            for(var i = 0; i < count; i++) {
                var sprite = new Sprite(file);
                var width = 1;
                var height = 1;
                var max_width = 0;
                var max_height = 0;
                var x = 800;
                var y = 600;
                var type = ColorFormats.ARGB_1555;
                foreach(var al in list) {
                    if(i < al.List.Count) {
                        var source = al.List[i];
                        if(source.ColorFormat == ColorFormats.LINK) {
                            source = source.Target;
                        }
                        if(source.FrameWidth > max_width) {
                            max_width = source.FrameHeight;
                        }
                        if(source.FrameHeight > max_height) {
                            max_height = source.FrameHeight;
                        }
                        if(source.IsHidden) {
                            continue;
                        }
                        if(source.Width + source.X > width) {
                            width = source.Width + source.X;
                        }
                        if(source.Height + source.Y > height) {
                            height = source.Height + source.Y;
                        }
                        if(source.X < x) {
                            x = source.X;
                        }
                        if(source.Y < y) {
                            y = source.Y;
                        }
                        if(source.ColorFormat > type && source.ColorFormat < ColorFormats.LINK) {
                            type = source.ColorFormat;
                        }
                    }
                }

                width -= x; //获得上下左右两端的差,即宽高
                height -= y;
                width = width > 1 ? width : 1; //防止宽高小于1
                height = height > 1 ? height : 1;
                if(width * height > 1) {
                    sprite.CompressMode = CompressMode.ZLIB;
                }
                sprite.ColorFormat = type;
                sprite.Index = sprites.Count;
                sprite.Location = new Point(x, y);
                sprite.FrameSize = new Size(max_width, max_height);
                var image = new Bitmap(width, height);
                using(var g = Graphics.FromImage(image)) {
                    foreach(var al in list) {
                        if(i < al.List.Count) {
                            var source = al.List[i];
                            if(source.ColorFormat == ColorFormats.LINK) {
                                source = source.Target;
                            }
                            g.DrawImage(source.Image, source.X - x, source.Y - y); //绘制贴图                       

                        }
                    }
                }
                sprite.ReplaceImage(type, image); //替换贴图
                eventArgs.Value++; //拼合进度自增
                this.Progress.Report(eventArgs);
                sprites.Add(sprite);
            }
            file.List.AddRange(sprites);
            eventArgs.Result = file;
            eventArgs.Value++;
            this.Progress.Report(eventArgs); //拼合完成
        }

        private class MergeProcess : IProgress<ProgressEventArgs> {

            private Store Store { get; }

            public MergeProcess(Store Store) {
                this.Store = Store;
            }

            public void Report(ProgressEventArgs e) {
                this.Store.Set("/merge/progress", e.Value);
                if(e.IsCompleted) {
                    this.Store.Set("/merge/result", e.Result);
                }
            }
        }


    }
}