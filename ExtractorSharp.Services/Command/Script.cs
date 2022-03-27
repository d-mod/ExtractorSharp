using System;
using System.Collections.Generic;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using QuickJS;
using QuickJS.Native;
using static QuickJS.JSPropertyFlags;
using static QuickJS.JSGetPropertyNamesFlags;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Command {
    [ExportCommand("RunScript")]
    internal class Script: InjectService, ICommand {
        [CommandParameter("Codes", IsRequired = true)]
        private string Codes;

        private static QuickJSRuntime _runtime;
        private static QuickJSContext _context;

        private QuickJSRuntime QJSRuntime {
            get {
                if(_runtime == null) {
                    _runtime = new QuickJSRuntime();
                }
                return _runtime;
            }
        }
        private QuickJSContext QJSContext {
            get {
                if(_context == null) {
                    _context = QJSRuntime.CreateContext();
                    QuickJSValue globalObj = _context.GetGlobal();
                    globalObj.DefineFunction("print", Print, 1, CWE);
                    globalObj.DefineFunction("Do", JSDo, 2, CWE);
                }
                return _context;
            }
        }

        private static dynamic JSValueToClr(JSContext ctx, JSValue value) {
            switch(value.Tag) {
                case JSTag.Bool:
                    return value.ToBoolean();
                case JSTag.Null:
                    return null;
                case JSTag.String:
                    return value.ToString(ctx);
                case JSTag.Int:
                    return value.ToInt32();
                case JSTag.Float64:
                    return value.ToDouble();
                case JSTag.Undefined:
                    return null;
                case JSTag.Object:
                    QuickJSContext qctx;
                    QuickJSContext.TryWrap(ctx, out qctx);
                    var qv = QuickJSValue.Wrap(qctx, value);

                    if(qv.IsArray) {
                        var len = (int)qv.GetProperty("length");
                        if(len == 0) {
                            return new dynamic[0];
                            //return new List<dynamic>().ToArray();
                        } else {
                            var firstElm = qv.GetProperty(0);
                            var firstType = firstElm.GetType();
                            var listType = typeof(List<>).MakeGenericType(firstType);
                            dynamic res = Activator.CreateInstance(listType);

                            for(int i = 0; i < len; i++) {
                                dynamic v = qv.GetProperty(i);
                                res.Add(v);
                            }
                            return res.ToArray();
                        }
                    } else {
                        var keys = qv.GetOwnPropertyNames(StringMask);
                        var res = new Dictionary<string, dynamic>();
                        foreach(var key in keys) {
                            var v = qv.GetProperty(key);
                            if(v is JSValue jsv) {
                                res[key] = JSValueToClr(ctx, jsv);
                            } else if(v is QuickJSValue qjsv) {
                                res[key] = JSValueToClr(ctx, qjsv.NativeInstance);
                            } else {
                                res[key] = v;
                            }
                        }
                        return res;
                    }


                default:
                    throw new InvalidCastException();
            }
        }
        private JSValue JSDo(JSContext ctx, JSValue thisArg, int argc, JSValue[] argv) {
            if(argc == 0) {
                return JSValue.False;
            } else if(argc == 1) {
                var funName = argv[0].ToString(ctx);
                this.Controller.Do(funName);
                return JSValue.True;
            } else {
                var actionName = argv[0].ToString(ctx);

                var param = JSValueToClr(ctx, argv[1]);
                if(param is Dictionary<string, dynamic> dicParam) {
                    var cc = new CommandContext {
                        dicParam
                    };
                    this.Controller.Do(actionName, cc);
                    return JSValue.True;
                } else {
                    return JSValue.False;
                }
            }
        }
        private static JSValue Print(JSContext ctx, JSValue thisArg, int argc, JSValue[] argv) {
            for(int i = 0; i < argc; i++) {
                Console.Write(argv[i].ToString(ctx));
                Console.Write(' ');
            }
            Console.WriteLine();
            return JSValue.Undefined;
        }

        public void Do(CommandContext context) {
            context.Export(this);
            try {
                object result = QJSContext.Eval(this.Codes, "script.js", JSEvalFlags.Global);
                this.Store.Set(StoreKeys.SCRIPT_LAST_RESULT, result);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
