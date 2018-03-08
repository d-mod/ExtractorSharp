
using System;
using System.Collections.Generic;
using ExtractorSharp.Command;
using ExtractorSharp.Command.ImageCommand;
using ExtractorSharp.Command.ImgCommand;
using ExtractorSharp.Command.LayerCommand;
using ExtractorSharp.Command.MergeCommand;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Command.PaletteCommand;
using ExtractorSharp.Command.DrawCommand;
using ExtractorSharp.Data;
using ExtractorSharp.Command.FileCommand;

namespace ExtractorSharp.Core {
    /// <summary>
    /// 命令控制器
    /// <see cref="ICommand"/>
    /// </summary>
    public class Controller : IDisposable {
        
        private IConnector Data => Program.Connector;

        private readonly Stack<ICommand> undoStack;
        private readonly Stack<ICommand> redoStack;

        private readonly Dictionary<string, Type> Dic;

        private readonly CommandEventArgs cmdArgs;

        public delegate void CommandHandler(object sender, CommandEventArgs e);

        /// <summary>
        /// 操作执行事件
        /// </summary>
        public event CommandHandler CommandDid;

        /// <summary>
        /// 操作撤销事件
        /// </summary>
        public event CommandHandler CommandUndid;

        /// <summary>
        /// 操作重做事件
        /// </summary>
        public event CommandHandler CommandRedid;

        /// <summary>
        /// 操作清空事件
        /// </summary>
        public event CommandHandler CommandCleared;

        private void OnCommandClear(CommandEventArgs e) => CommandCleared?.Invoke(this, e);

        private void OnComandDid(CommandEventArgs e) => CommandDid?.Invoke(this, e);

        private void OnComandUndid(CommandEventArgs e) => CommandUndid?.Invoke(this, e);

        private void OnCommandRedid(CommandEventArgs e) => CommandRedid?.Invoke(this, e);

        public delegate void ActionHandler(object o, ActionEventArgs e);

        private readonly ActionEventArgs actArgs;

        /// <summary>
        /// 动作记录
        /// </summary>
        public event ActionHandler ActionChanged;
        /// <summary>
        /// 动作执行
        /// </summary>
        public event ActionHandler ActionDid;

        private void OnActionChanged(ActionEventArgs e) => ActionChanged?.Invoke(this, e);

        private void OnActionDid(ActionEventArgs e) => ActionChanged?.Invoke(this, e);

        /// <summary>
        /// 动作序列
        /// </summary>
        public List<IAction> Macro => actArgs.Queues;


        /// <summary>
        /// 当前执行的操作
        /// </summary>
        public ICommand Current { get; private set; }

        /// <summary>
        /// 正在录制宏
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
        /// 当前操作的位置
        /// </summary>
        public int Index => undoStack.Count;
   

        public Controller() {
            cmdArgs = new CommandEventArgs();
            actArgs = new ActionEventArgs();
            actArgs.Queues = new List<IAction>();
            undoStack = new Stack<ICommand>();
            redoStack = new Stack<ICommand>();
            Dic = new Dictionary<string, Type>();
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        public void Regisity(string cmd, Type type) {
            if (Dic.ContainsKey(cmd)) {
                Dic.Remove(cmd);
            }
            Dic.Add(cmd, type);
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
            actArgs.Mode = QueueChangeMode.Clear;//清空模式
            OnActionChanged(actArgs);//触发队列更改事件
        }

        public void Pause() => IsRecord = !IsRecord;
        

        /// <summary>
        /// 执行宏命令
        /// </summary>
        /// <param name="allImage"></param>
        /// <param name="als"></param>
        internal void Run(bool allImage, params Album[] als) {
            IsRecord = false;
            foreach (var cmd in Macro) {
                switch (cmd) {//判断Action的类型
                    case IMutipleAciton mutipleAction:
                        mutipleAction.Action(als);
                        break;
                    case ISingleAction singleAction:
                        foreach (var al in als) {
                            var indexes = singleAction.Indices;
                            if (allImage) {
                                indexes = new int[al.List.Count];
                                for (var i = 0; i < al.List.Count; i++)
                                    indexes[i] = i;
                            }
                            singleAction.Action(al, indexes);
                        }
                        break;
                }
                OnActionDid(actArgs);//触发动作执行事件
            }
            Data.FileListFlush();
            Data.ImageListFlush();//刷新画布
            Messager.ShowMessage(Msg_Type.Operate, Language.Default["ActionRun"]);
        }

        /// <summary>
        /// 移出动作序列
        /// </summary>
        /// <param name="range"></param>
        internal void Delete(params IAction[] range) {
            foreach (var item in range) {
                Macro.Remove(item);
            }
            actArgs.Mode = QueueChangeMode.RemoveRange;
            OnActionChanged(actArgs);
        }





        #endregion

        #region 撤销重做
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        public void Move(int step) {
            if (step > 0) {
                for (int i = 0; i < step; i++) {
                    Redo();
                }
            } else {
                for (var i = step; i < 0; i++) {
                    Undo();
                }
            }
            Data.ImageListFlush();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        public void Do(string key, params object[] args) {
            if (Dic.ContainsKey(key)) {
                var type = Dic[key];
                if (type != null && typeof(ICommand).IsAssignableFrom(type)) {
                    var cmd = type.CreateInstance() as ICommand;
                    cmd.Do(args);
                    if (cmd.CanUndo) {//可撤销
                        undoStack.Push(cmd);
                    }
                    if (IsRecord && cmd is IAction action) {//可宏
                        Macro.Add(action);
                        actArgs.Mode = QueueChangeMode.Add;
                        actArgs.Action = action;
                        OnActionChanged(actArgs);
                    }
                    if (cmd.IsFlush) {
                        Data.FileListFlush();
                    }
                    if (cmd.IsChanged) {//发生更改
                        Data.OnSaveChanged();
                    }
                    redoStack.Clear();
                    OnComandDid(cmdArgs);
                    Current = cmd;
                }
            } else {
                Messager.ShowMessage(Msg_Type.Error, $"不存在的命令[{key}]");
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        private void Undo() {
            if (undoStack.Count > 0) {
                var cmd = undoStack.Pop();
                cmd.Undo();
                redoStack.Push(cmd);
                OnComandUndid(cmdArgs);
                if (cmd.IsFlush) {
                    Data.FileListFlush();
                }
                if (cmd.IsChanged) {
                    Data.OnSaveChanged();
                }
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        private void Redo() {
            if (redoStack.Count > 0) {
                var cmd = redoStack.Pop();
                cmd.Redo();
                undoStack.Push(cmd);
                OnCommandRedid(cmdArgs);
                if (cmd.IsFlush) {
                    Data.FileListFlush();
                }
                if (cmd.IsChanged) {
                    Data.OnSaveChanged();
                }
            }
        }

        #endregion
        


        #region


        
        public void Dispose() {
            undoStack.Clear();
            redoStack.Clear();
            OnCommandClear(cmdArgs);
        }


        #endregion
    }
}
