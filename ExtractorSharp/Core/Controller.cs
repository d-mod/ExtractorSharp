using System;
using System.Collections.Generic;
using System.Linq;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Exceptions;
using ExtractorSharp.Core;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     命令控制器
    ///     <see cref="ICommand" />
    /// </summary>
    public class Controller : IDisposable {
        public delegate void ActionHandler(object o, ActionEventArgs e);

        public delegate void CommandHandler(object sender, CommandEventArgs e);

        private readonly ActionEventArgs actArgs;

        private readonly Dictionary<string, Type> Dic;
        private readonly Stack<ICommand> redoStack;

        private readonly Dictionary<Type, string> ReserveDic;

        private readonly Stack<ICommand> undoStack;

        private readonly Dictionary<string, IExecutable> ExecuteMap;

        private static CommandParser CommandParser => Program.CommandParser;

        public Controller() {
            actArgs = new ActionEventArgs();
            actArgs.Queues = new List<IAction>();
            undoStack = new Stack<ICommand>();
            redoStack = new Stack<ICommand>();
            Dic = new Dictionary<string, Type>();
            ReserveDic = new Dictionary<Type, string>();
            ExecuteMap = new Dictionary<string, IExecutable>();
        }

        private IConnector Connector => Program.Connector;

        /// <summary>
        ///     动作序列
        /// </summary>
        public List<IAction> Macro => actArgs.Queues;


        /// <summary>
        ///     当前执行的操作
        /// </summary>
        public ICommand Current { get; private set; }

        /// <summary>
        ///     正在录制宏
        /// </summary>
        public bool IsRecord { private set; get; }


        public ICommand[] History {
            get {
                var list = new List<ICommand>();
                var undo = undoStack.ToArray();
                Array.Reverse(undo);
                list.AddRange(undo);
                list.AddRange(redoStack);
                return list.ToArray();
            }
        }

        /// <summary>
        ///     当前操作的位置
        /// </summary>
        public int Index => undoStack.Count;


        /// <summary>
        ///     操作执行事件
        /// </summary>
        public event CommandHandler CommandDid;

        /// <summary>
        ///     操作撤销事件
        /// </summary>
        public event CommandHandler CommandUndid;

        /// <summary>
        ///     操作重做事件
        /// </summary>
        public event CommandHandler CommandRedid;

        /// <summary>
        ///     操作清空事件
        /// </summary>
        public event CommandHandler CommandCleared;

        private void OnCommandClear(CommandEventArgs e) {
            CommandCleared?.Invoke(this, e);
        }

        private void OnComandDid(CommandEventArgs e) {
            CommandDid?.Invoke(this, e);
        }

        private void OnComandUndid(CommandEventArgs e) {
            CommandUndid?.Invoke(this, e);
        }

        private void OnCommandRedid(CommandEventArgs e) {
            CommandRedid?.Invoke(this, e);
        }

        /// <summary>
        ///     动作记录
        /// </summary>
        public event ActionHandler ActionChanged;

        /// <summary>
        ///     动作执行
        /// </summary>
        public event ActionHandler ActionDid;

        private void OnActionChanged(ActionEventArgs e) {
            ActionChanged?.Invoke(this, e);
        }

        private void OnActionDid(ActionEventArgs e) {
            ActionChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     注册命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        public void Registry(string cmd, Type type) {
            if (Dic.ContainsKey(cmd)) {
                Dic.Remove(cmd);
            }
            Dic.Add(cmd, type);
            ReserveDic.Add(type, cmd);
        }

        public void Registry(string cmd, IExecutable exe) {
            if (ExecuteMap.ContainsKey(cmd)) {
                ExecuteMap.Remove(cmd);
            }
            ExecuteMap.Add(cmd, exe);
        }


        public void ClearMacro() {
            Macro.Clear();
            actArgs.Mode = QueueChangeMode.RemoveRange;
            OnActionChanged(actArgs);
        }


        #region 宏命令

        public void Record() {
            IsRecord = true;
            Macro.Clear();
            actArgs.Action = null;
            actArgs.Mode = QueueChangeMode.Clear; //清空模式
            OnActionChanged(actArgs); //触发队列更改事件
        }

        public void Pause() {
            IsRecord = !IsRecord;
        }


        public void AddMacro(ICommand command) {
            if (command is IAction action) {
                //可宏
                Macro.Add(action);
                actArgs.Mode = QueueChangeMode.Add;
                actArgs.Action = action;
                OnActionChanged(actArgs);
            }
        }

        /// <summary>
        ///     执行宏命令
        /// </summary>
        /// <param name="allImage"></param>
        /// <param name="als"></param>
        internal void Run(bool allImage, params Album[] als) {
            IsRecord = false;
            foreach (var cmd in Macro) {
                switch (cmd) {
                    //判断Action的类型
                    case IMutipleAciton mutipleAction:
                        mutipleAction.Action(als);
                        break;
                    case ISingleAction singleAction:
                        foreach (var al in als) {
                            var indexes = singleAction.Indices;
                            if (allImage) {
                                indexes = new int[al.List.Count];
                                for (var i = 0; i < al.List.Count; i++) {
                                    indexes[i] = i;
                                }
                            }

                            singleAction.Action(al, indexes);
                        }

                        break;
                }

                OnActionDid(actArgs); //触发动作执行事件
            }

            Connector.FileListFlush();
            Connector.ImageListFlush(); //刷新画布
            ClearCommand();
            Connector.SendMessage(MessageType.Success, Language.Default["ActionRun"]);
        }

        /// <summary>
        ///     移出动作序列
        /// </summary>
        /// <param name="range"></param>
        internal void Delete(params IAction[] range) {
            foreach (var item in range) Macro.Remove(item);
            actArgs.Mode = QueueChangeMode.RemoveRange;
            OnActionChanged(actArgs);
        }

        #endregion

        #region 撤销重做

        /// <summary>
        /// </summary>
        /// <param name="step"></param>
        public void Move(int step) {
            if (step > 0) {
                for (var i = 0; i < step; i++) {
                    Redo();
                }
            } else {
                for (var i = step; i < 0; i++) {
                    Undo();
                }
            }
            Connector.ImageListFlush();
        }


        public object Dispatch(string name,params object[] args) {
            if (ExecuteMap.ContainsKey(name)) {
                var exe = ExecuteMap[name];
                return exe.Execute(args);
            } else {
                throw new CommandException("NotExistCommand");
            }
        }

        /// <summary>
        ///     执行命令
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        public void Do(string key, params object[] args) {
            if (Dic.ContainsKey(key)) {
                var type = Dic[key];
                if (type != null && typeof(ICommand).IsAssignableFrom(type)) {
                    var cmd = type.CreateInstance() as ICommand;

                    cmd.Do(args);
                    if (cmd.CanUndo) {
                        undoStack.Push(cmd);
                    }
                    if (IsRecord) {
                        AddMacro(cmd);
                    }
                    redoStack.Clear();
                    OnComandDid(new CommandEventArgs {
                        Name = key,
                        Command = cmd,
                        Type = CommandEventType.Do
                    });
                    Current = cmd;
                }
            } else {
                throw new CommandException("NotExistCommand");
            }
        }

        /// <summary>
        ///     撤销
        /// </summary>
        private void Undo() {
            if (undoStack.Count > 0) {
                var cmd = undoStack.Pop();
                cmd.Undo();
                redoStack.Push(cmd);
                OnComandUndid(new CommandEventArgs {
                    Name = ReserveDic[cmd.GetType()],
                    Command = cmd,
                    Type = CommandEventType.Undo
                });
            }
        }

        /// <summary>
        ///     重做
        /// </summary>
        private void Redo() {
            if (redoStack.Count > 0) {
                var cmd = redoStack.Pop();
                cmd.Redo();
                undoStack.Push(cmd);
                OnCommandRedid(new CommandEventArgs {
                    Name = ReserveDic[cmd.GetType()],
                    Command = cmd,
                    Type = CommandEventType.Redo
                });
            }
        }

        #endregion


        #region

        public void ClearCommand() {
            undoStack.Clear();
            redoStack.Clear();
            OnCommandClear(new CommandEventArgs {
                Type = CommandEventType.Clear
            });
        }

        public void Dispose() {
            ClearCommand();
            ClearMacro();
        }

        #endregion
    }
}