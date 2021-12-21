using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Exceptions;

namespace ExtractorSharp.Composition.Core {
    /// <summary>
    ///     命令控制器
    ///     <see cref="ICommand" />
    /// </summary>
    [Export]
    public class Controller : IDisposable {



        private readonly Stack<KeyValuePair<IRollback, ICommandMetadata>> redoStack;

        private readonly Stack<KeyValuePair<IRollback, ICommandMetadata>> undoStack;


        /// <summary>
        ///     动作序列
        /// </summary>
        public readonly List<IMacro> Macro;

        [ImportMany(typeof(ICommand))]
        private IEnumerable<ExportFactory<ICommand, ICommandMetadata>> commands;

        [ImportMany(typeof(ICommandListener))]
        private IEnumerable<Lazy<ICommandListener, IListenerMetadata>> listeners;

        public Controller() {

            this.undoStack = new Stack<KeyValuePair<IRollback, ICommandMetadata>>();

            this.redoStack = new Stack<KeyValuePair<IRollback, ICommandMetadata>>();

            this.Macro = new List<IMacro>();
        }



        /// <summary>
        ///     当前执行的操作
        /// </summary>
        public IRollback Current { get; private set; }

        /// <summary>
        ///     正在录制宏
        /// </summary>
        public bool IsRecord { private set; get; }


        public IEnumerable<KeyValuePair<IRollback, IName>> History {
            get {
                var list = new List<KeyValuePair<IRollback, IName>>();
                var undo = this.undoStack.Select(e => new KeyValuePair<IRollback, IName>(e.Key, e.Value));
                var redo = this.redoStack.Select(e => new KeyValuePair<IRollback, IName>(e.Key, e.Value));
                undo.Reverse();
                list.AddRange(undo);
                list.AddRange(redo);
                return list;
            }
        }

        /// <summary>
        ///     当前操作的位置
        /// </summary>
        public int Index => this.undoStack.Count;

        public bool CanUndo => this.undoStack.Count > 0;

        public bool CanRedo => this.redoStack.Count > 0;

        /// <summary>
        ///     操作执行事件
        /// </summary>
        public event CommandHandler CommandChanged;


        private void OnCommandChanged(CommandEventArgs e) {
            CommandChanged?.Invoke(this, e);
        }


        /// <summary>
        ///     动作记录
        /// </summary>
        public event ActionHandler ActionChanged;

        private void OnActionChanged(ActionEventArgs e) {
            ActionChanged?.Invoke(this, e);
        }

        public event ActionHandler ActionDid;

        private void OnActionDid(ActionEventArgs e) {
            ActionDid?.Invoke(this, e);
        }

        public void ClearMacro() {
            this.Macro.Clear();
            this.OnActionChanged(new ActionEventArgs {
                Mode = QueueChangeMode.Clear
            });
        }


        #region 宏命令

        public void Record() {
            this.IsRecord = true;
            this.Macro.Clear();
            this.OnActionChanged(new ActionEventArgs {
                Mode = QueueChangeMode.Clear
            }); //触发队列更改事件
        }

        public void Pause() {
            this.IsRecord = !this.IsRecord;
        }


        public void AddMacro(IMacro action) {
            //可宏
            this.Macro.Add(action);
            this.OnActionChanged(new ActionEventArgs {
                Mode = QueueChangeMode.Add,
                Action = action
            });
        }

        /// <summary>
        ///     执行宏命令
        /// </summary>
        /// <param name="allImage"></param>
        /// <param name="als"></param>
        public void Run(bool allImage, params Album[] als) {
            this.IsRecord = false;
            foreach(var cmd in this.Macro) {
                switch(cmd) {
                    //判断Action的类型
                    case IMutipleMacro mutipleAction:
                        mutipleAction.Action(als);
                        break;
                    case ISingleMacro singleAction:
                        foreach(var al in als) {
                            var indexes = singleAction.Indices;
                            if(allImage) {
                                indexes = new int[al.List.Count];
                                for(var i = 0; i < al.List.Count; i++) {
                                    indexes[i] = i;
                                }
                            }

                            singleAction.Action(al, indexes);
                        }

                        break;
                }

                this.OnActionDid(new ActionEventArgs {
                    Queues = this.Macro.ToList()

                }); //触发动作执行事件
            }

            // Connector.FileListFlush();
            // Connector.ImageListFlush(); //刷新画布
            this.ClearCommand();
            // Connector.SendMessage(MessageType.Success, Language.Default["ActionRun"]);
        }

        /// <summary>
        ///     移出动作序列
        /// </summary>
        /// <param name="range"></param>
        public void Delete(params IMacro[] range) {
            this.Macro.RemoveAll(range.Contains);

            this.OnActionChanged(new ActionEventArgs {
                Mode = QueueChangeMode.RemoveRange
            });
        }

        #endregion

        #region 撤销重做

        /// <summary>
        /// </summary>
        /// <param name="step"></param>
        public void Move(int step) {
            if(step > 0) {
                for(var i = 0; i < step; i++) {
                    this.Redo();
                }
            } else {
                for(var i = step; i < 0; i++) {
                    this.Undo();
                }
            }
        }

        public Controller Do(string key, object args = null) {
            return this.Do(key, new CommandContext(args));
        }



        /// <summary>
        ///     执行命令
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        public Controller Do(string key, CommandContext context) {
            if(!string.IsNullOrEmpty(key)) {
                var factory = this.commands.FirstOrDefault(item => key.EqualsIgnoreCase(item.Metadata.Name));
                if(factory != null) {
                    var metadata = factory.Metadata;

                    var cmd = factory.CreateExport().Value;

                    var events = new List<ICommandListener>();
                    events = (from item in this.listeners
                              where
                              metadata.Listeners.Contains(item.Metadata.Name) || item.Metadata.IsGlobal
                              select item.Value).ToList();


                    var e = new CommandEventArgs {
                        Metadata = metadata,
                        Name = key,
                        Context = context,
                        Command = cmd,
                        Type = CommandEventType.Do
                    };

                    events.ForEach(l => l.Before(e));

                    ////是否已中断执行
                    if(context.IsCancel) {
                        return this;
                    }

                    cmd.Do(context);

                    if(context.IsCancel) {
                        return this;
                    }


                    events.ForEach(l => l.After(e));


                    if(cmd is IRollback rollback) {
                        this.undoStack.Push(new KeyValuePair<IRollback, ICommandMetadata>(rollback, metadata));
                        this.Current = rollback;
                    }

                    if(this.IsRecord && cmd is IMacro action) {
                        this.AddMacro(action);
                    }
                    while(this.redoStack.Count > 0) {
                        var redo = this.redoStack.Pop().Key;
                        if(redo is IDisposable disposable) {
                            disposable.Dispose();
                        }
                    }

                    this.OnCommandChanged(e);

                }
            } else {
                throw new CommandException("NotExistCommand");
            }
            return this;
        }

        /// <summary>
        ///     撤销
        /// </summary>
        public Controller Undo() {
            if(this.undoStack.Count > 0) {
                var pair = this.undoStack.Pop();
                var cmd = pair.Key;
                var metadata = pair.Value;
                cmd.Undo();
                this.redoStack.Push(pair);
                this.OnCommandChanged(new CommandEventArgs {
                    Metadata = metadata,
                    Name = metadata.Name,
                    Command = cmd,
                    Type = CommandEventType.Undo
                }); ;
            }
            return this;
        }

        /// <summary>
        ///     重做
        /// </summary>
        public Controller Redo() {
            if(this.redoStack.Count > 0) {
                var pair = this.redoStack.Pop();
                var cmd = pair.Key;
                var metadata = pair.Value;
                cmd.Redo();
                this.undoStack.Push(pair);
                this.OnCommandChanged(new CommandEventArgs {
                    Metadata = metadata,
                    Name = metadata.Name,
                    Command = cmd,
                    Type = CommandEventType.Redo
                });
            }
            return this;
        }
        #endregion


        #region

        public void ClearCommand() {
            this.undoStack.Clear();
            this.redoStack.Clear();
            this.OnCommandChanged(new CommandEventArgs {
                Type = CommandEventType.Clear
            });
        }

        public void Dispose() {
            this.ClearCommand();
            this.ClearMacro();
        }

        #endregion


        public delegate void ActionHandler(object o, ActionEventArgs e);

        public delegate void CommandHandler(object sender, CommandEventArgs e);


    }

}