using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExtractorSharp.Services.Command.Files {

    [ExportCommand("OpenFile")]
    internal class OpenFile : InjectService, IRollback {

        [CommandParameter(IsRequired = false, IsDefault = true)]
        private IEnumerable<string> Paths;

        private IEnumerable<Album> newFiles;

        private IEnumerable<Album> oldFiles;

        [CommandParameter(IsRequired = false)]
        private bool? IsClear = null;

        private string SavePath;

        private bool IsSaved;


        public void Do(CommandContext context) {
            context.Export(this);

            if(this.Paths == null || this.Paths.Count() == 0) {
                var dialog = new CommonOpenFileDialog {
                    Multiselect = true
                };
                dialog.Filters.Add(new CommonFileDialogFilter(this.Language["ImageResources"], "*.npk;*.img"));
                if(dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    this.Paths = dialog.FileNames;
                } else {
                    context.IsCancel = true;
                    return;
                }
            }

            if(this.Store.Get(StoreKeys.IS_LOCKED, false)) {
                this.Messager.Send("正在执行打开任务", MessageType.Warning);
                context.IsCancel = true;
                return;
            }

            if(this.IsClear == null) {
                var notips = this.Store.Get<bool>("/config/data/add-file-no-tips");
                if(!notips&&!this.Store.IsNullOrEmpty(StoreKeys.FILES)) {          
                    var result = this.MessageBox.Show(this.Language["Tips"], this.Language["Tips", "OpenFile"], CommonMessageBoxButton.YesNoCancel, CommonMessageBoxIcon.Question);
                    if(result == CommonMessageBoxResult.Cancel) {
                        context.IsCancel = true;
                        return;
                    }
                    this.IsClear = result == CommonMessageBoxResult.Yes;
                } else {
                    this.IsClear = true;
                }
            }

            this.Redo();
        }



        public void Redo() {

            this.Store.Set(StoreKeys.IS_LOCKED, true);

            var progress = new OpenFileProgress(Completed);

            var task = new Task(() => {
                this.Controller.Do("LoadFile", new CommandContext(this.Paths) {
                    {"Progress",progress }
                });
            });

            task.Start();

            this.Store
                .Get(StoreKeys.SAVE_PATH, out this.SavePath, string.Empty)
                .Get(StoreKeys.IS_SAVED, out this.IsSaved);

            var _savePath = this.SavePath;


            if(this.IsClear == true) {
                _savePath = string.Empty;
            }

            if(_savePath.Length == 0 && this.Paths.Count() > 0) {
                _savePath = this.Paths.FirstOrDefault(item => item.ToLower().EndsWith(".npk")) ?? string.Empty;
            }

            this.Store
                .Set(StoreKeys.SAVE_PATH, _savePath)
                .Set(StoreKeys.IS_SAVED, this.IsClear)
                .Use<List<string>>(StoreKeys.RECENTS, _recents => {
                    _recents.InsertRange(0, this.Paths);
                    return _recents.Distinct().ToList();
                });


        }

        public void Undo() {
            this.Store
                .Set(StoreKeys.SAVE_PATH, this.SavePath)
                .Set(StoreKeys.IS_SAVED, this.IsSaved)
                .Use<List<Album>>(StoreKeys.FILES, list => {
                    if(this.IsClear == true) {
                        list = new List<Album>(this.oldFiles);
                    } else if(this.newFiles != null) {
                        list.RemoveAll(this.newFiles.Contains);
                    }
                    return list;
                });
        }


        private void Completed() {

            this.Store
                .Get(StoreKeys.LOAD_FILES, out this.newFiles)
                .Use<List<Album>>(StoreKeys.FILES, list => {
                    if(this.IsClear == true) {
                        this.oldFiles = list.ToList();
                        list.Clear();
                    }
                    if(this.newFiles != null && this.newFiles.Count() > 0) {
                        list.AddRange(this.newFiles);
                    }
                    return list.ToList();
                })
                .Set(StoreKeys.IS_LOCKED, false);

        }

        public class OpenFileProgress : IProgress<ProgressEventArgs> {
            private Action Completed;

            public OpenFileProgress(Action Completed) {
                this.Completed = Completed;
            }


            public void Report(ProgressEventArgs e) {

                if(e.IsCompleted) {
                    this.Completed?.Invoke();
                }

            }



        }
    }
}
