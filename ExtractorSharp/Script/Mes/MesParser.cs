using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Script.Mes {

    public class Token {

        public string Text; // 本段token对应的文本
        public Token(string text) {
            Text = text;
        }
    }

    public class BlockToken : Token {
        public BlockToken(string text) : base(text) { }

        public List<Token> ContentTokens = new List<Token>();
    }
    public class LineEndToken : Token {
        public LineEndToken() : base(";") { }
    }

    public struct TokenInvokeParameter {

        public List<Token> Tokens;

        public int Cursor;

        public object CurrentArg;

        public Token LastToken => Tokens[Cursor - 1];
        public Token CurrentToken => Tokens[Cursor];
        public Token NextToken => Tokens[Cursor + 1];
    }

    public struct TokenInvokeResult {

        public object Ret;

        public int? NewCursor;
    }

    public class ExcutingEventArgs : EventArgs {

        public string Name;

        public object[] Args;

    }

    public class MesParser {
        private static Dictionary<string, object> _context = new Dictionary<string, object>(); // 包含中间变量之类的, 全局唯一
        private Dictionary<string, object> _status = new Dictionary<string, object>(); // 包含解析时的中间过程, 每个解析实例唯一
        private Dictionary<string, Func<TokenInvokeParameter, TokenInvokeResult>> FuncMap;

        /// <summary>
        /// Parser事件处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MesParserHandler(object sender, ExcutingEventArgs e);

       
        /// <summary>
        /// 执行命令事件
        /// </summary>
        public event MesParserHandler Executing;


        public MesParser() {
            FuncMap = new Dictionary<string, Func<TokenInvokeParameter, TokenInvokeResult>> {
                ["asNull"] = arg => new TokenInvokeResult { Ret = null },
                ["addOne"] = arg => new TokenInvokeResult { Ret = (int)arg.CurrentArg + 1 },
                ["toBool"] = arg => new TokenInvokeResult { Ret = arg.CurrentArg as string != "false" },
                ["toArray"] = arg => new TokenInvokeResult { Ret = (arg.CurrentArg as string)?.Split(',') },

                ["toInt"] = arg => {
                    var res = new TokenInvokeResult();
                    switch (arg.CurrentArg) {
                        case string iStr:
                            res.Ret = int.Parse(iStr);
                            break;
                        case object[] iObjects:
                            res.Ret = iObjects.Select(x => int.Parse(x as string ?? string.Empty)).ToArray();
                            break;
                    }
                    return res;
                },
/*                ["LoadFile"] = arg => new TokenInvokeResult { Ret = Connector.LoadFile(arg.CurrentArg as string).ToArray() },*/
                ["exit"] = arg => {
                    var code = 0;
                    switch (arg.CurrentArg) {
                        case string iStr:
                            code = int.Parse(iStr);
                            break;
                        case int iInt:
                            code = iInt;
                            break;
                    }
                    Environment.Exit(code);
                    return new TokenInvokeResult { Ret = null };
                },
                ["message"] = arg => {
                    var msg = "";
                    switch (arg.CurrentArg) {
                        case object[] argArray:
                            MessageBox.Show(string.Join(",", argArray.Select(x => x.ToString())));
                            break;
                        default:
                            MessageBox.Show(arg.ToString());
                            break;
                    }
                    return new TokenInvokeResult { Ret = arg.CurrentArg };
                },
                ["asVar"] = arg => {
                    _context[arg.NextToken.Text] = arg.CurrentArg;
                    return new TokenInvokeResult { Ret = arg.CurrentArg, NewCursor = arg.Cursor + 1 };
                },
                ["useVar"] = arg => {
                    var value = CalculatePropertyValue(arg.NextToken.Text);

                    return new TokenInvokeResult { Ret = value, NewCursor = arg.Cursor + 1 };
                },
                ["forEach"] = arg => {
                    var _iterationVariable = arg.CurrentArg;
                    var iterationVariableName = arg.NextToken.Text;
                    var _block = arg.Tokens[arg.Cursor + 2];
                    if (!(_block is BlockToken block)) {
                        throw new Exception(
                            $"Can't find a block in next next token, the token is {_block.Text} in fact"
                        );
                    }



                    if (!(_iterationVariable is Array iterationVariable)) {
                        throw new Exception(
                            $"The variable({_iterationVariable}) in front of forEach is not an array"
                        );
                    }


                    foreach (var i in iterationVariable) {
                        _context[iterationVariableName] = i;
                        // Console.WriteLine(i);
                        InvokeToken(block.ContentTokens);
                    }

                    return new TokenInvokeResult { Ret = iterationVariable, NewCursor = arg.Cursor + 2 };
                },
                ["@"] = arg => {
                    var _block = arg.Tokens[arg.Cursor + 1];
                    if (!(_block is BlockToken block)) {
                        throw new Exception(
                            $"Can't find a block in next next token, the token is {_block.Text} in fact"
                        );
                    }

                    var APIName = block.ContentTokens[0].Text;
                    var APIPars = new List<object>();
                    var tokens = new List<Token>();
                    foreach (var t in block.ContentTokens.Skip(2)) // 越过API名及其分号
                    {
                        switch (t) {
                            case LineEndToken token:

                                APIPars.Add(InvokeToken(tokens));
                                tokens.Clear();
                                break;
                            case Token token:
                                tokens.Add(token);
                                break;
                        }
                    }

                    InvokeToken(block.ContentTokens);
                    // 执行命令
                    Executing?.Invoke(this, new ExcutingEventArgs {
                        Name = APIName,
                        Args = APIPars.ToArray()
                    });
                    return new TokenInvokeResult { Ret = null, NewCursor = arg.Cursor + 1 };
                },
                ["Concat"] = arg => {
                    var ret = new TokenInvokeResult { Ret = arg.CurrentArg, NewCursor = arg.Cursor };

                    var nextTokenValue = InvokeToken(arg.Tokens.Skip(arg.Cursor + 1).ToList(), out var nextTokenResult, true);
                    ret.NewCursor += nextTokenResult.NewCursor;
                    // concat 左侧的值不应该因右侧的值改变类型.
                    switch (arg.CurrentArg) {
                        case string argStr:
                            ret.Ret = argStr + nextTokenValue;
                            break;
                        case int[] argArr:

                            var newArrayInt = argArr.ToList();
                            if (nextTokenValue is int valueInt) {
                                newArrayInt.Add(valueInt);
                            }
                            ret.Ret = newArrayInt.ToArray();
                            break;
                        case object[] argArr:

                            var newArrayObject = argArr.ToList();
                            if (nextTokenValue is int valueObject) {
                                newArrayObject.Add(valueObject);
                            }
                            ret.Ret = newArrayObject.ToArray();
                            break;
                    }

                    return ret;
                }
            };
        }

        public void Registry(string name, Func<TokenInvokeParameter, TokenInvokeResult> func) {
            FuncMap.Add(name, func);
        }


        /// <summary>
        ///     用于检查某个开关是否打开
        /// </summary>
        public bool CheckSwitch(string name) {
            return _status.ContainsKey(name) && _status[name] as string == "on";
        }
        /// <summary>
        ///     用于设置某个开关状态
        /// </summary>
        public bool SetSwitch(string name, bool status) {
            _status[name] = status ? "on" : "off";
            return status;
        }
        /// <summary>
        ///     用于设置某个开关的值
        /// </summary>
        public object SetSwitch(string name, object status) {
            _status[name] = status;
            return status;
        }
        /// <summary>
        ///     用于获取某个开关的值
        /// </summary>
        public object GetSwitch(string name, object defaultValue = null) {
            return _status.TryGetValue(name, out var value) ? value : defaultValue;
        }
        /// <summary>
        ///     解析运行命令行, 每句应当以 [;] 结尾, 以下演示中部分未使用
        ///     目前包含命令:
        ///     无副作用命令(不影响上下文):
        ///         toArray toInt LoadFile
        ///         toBool asNull exit
        ///         message
        ///     用法如:
        ///         1|toBool => true
        ///         |asNull => null
        ///         1,2,3|toArray => {"1", "2", "3"}
        ///         1,2,3|toArray|toInt => {1, 2, 3}
        ///         D:\WeGameApps\地下城与勇士\ImagePacks2\sprite_interface_monstercard.NPK|LoadFile => Album
        ///         asd|message => MessageBox.Show("asd")
        ///         0|exit => 以错误代码 0 退出
        ///         1|exit => 以错误代码 1 退出
        ///     上下文命令(影响上下文): 
        ///         asVar| useVar| $
        ///         forEach| 
        ///     用法如:
        ///         1,2,3|toArray|toInt|asVar|list 定义一个变量名作 list, 值为 {1, 2, 3}
        ///         |useVar|list 使用名为 list 的变量
        ///         定义和使用变量有简写形式, 1,2,3|toArray|toInt|$list 为赋值 list 为 {1, 2, 3}
        ///         单书写 |$list 为使用变量 list, 即 |useVar|list
        ///         |$list|message => MessageBox.Show(list)
        ///         |useVar|list|message => MessageBox.Show(list)
        /// 
        ///         |$list|forEach|i|{
        ///             i|message;
        ///             i|addOne|asVar|t;
        ///         }
        ///         为 list.forEach(i => {
        ///                 message(i);
        ///                 t = addOne(i);
        ///            })
        ///
        ///     对于 API 的调用, 应当使用 @ 开头, 并采用以下格式:
        ///         @{
        ///             saveImage; 
        ///             sprite_interface_monstercard.NPK|LoadFile;
        ///             1|toInt;
        ///             1|toArray|toInt;
        ///             Z:\output;
        ///             "";
        ///             0|toInt;
        ///             0|toInt;
        ///             false|toBool;
        ///             |asNull;
        ///             true|toBool;
        ///         }
        ///     第一个是 API 名, 接下来是参数, 参数可以一行写, 也可多行写, 暂不支持命名参数       
        /// </summary>
        /// <param name="command"></param>
        public object InvokeToken(List<Token> tokens, out TokenInvokeResult outResult, bool stopWhenLineEnd = false) {
            var result = new TokenInvokeResult { NewCursor = 0, Ret = "" };
            var parameter = new TokenInvokeParameter { CurrentArg = result.Ret, Cursor = 0, Tokens = tokens };

            for (var i = 0; i < tokens.Count; i++) {
                parameter.Cursor = i;

                if (parameter.CurrentToken is LineEndToken) {
                    if (stopWhenLineEnd)
                        break;
                    continue;
                }

                if (FuncMap.TryGetValue(parameter.CurrentToken.Text, out var fun)) {
                    result = fun(parameter);
                    i = result.NewCursor ?? i;
                } else {
                    // throw new Exception($"Can't find the command {Parameter.CurrentToken.Text} in code {GetAST(tokens)}");
                    result.Ret = parameter.CurrentToken.Text;
                }

                result.NewCursor = i + 1;
                parameter.CurrentArg = result.Ret;
            }

            outResult = result;
            return result.Ret;
        }

        public object InvokeToken(List<Token> tokens, bool stopWhenLineEnd = false) {
            TokenInvokeResult _;
            return InvokeToken(tokens, out _, stopWhenLineEnd);
        }
        public object InvokeToken(string tokens, bool stopWhenLineEnd = false) {
            return InvokeToken(ParseBlock(tokens), stopWhenLineEnd);
        }

        /// <summary>
        ///   计算属性, 类似 a.b 或者 a.c()
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public object CalculatePropertyValue(string text) {
            object ret;
            var strings = text.Split('.');
            var name = strings[0];

            if (_context.ContainsKey(name)) {
                ret = _context[name];
            } else {
                throw new Exception($"Can't find the instance named '{name}'");
            }

            if (strings.Length > 1) { // 大于 1 表明[.]后存在字符

                foreach (var s in strings.Skip(1)) {
                    if (ret == null) {
                        throw new Exception($"Can't read property '{s}' of {name}");
                    }

                    var theType = ret.GetType();

                    switch (s) {
                        case string property when property.EndsWith(")"):
                            // TODO: 支持方法传参
                            var funcParsMatch = Regex.Match(property, @"(?<=[(]).+(?=[)])");
                            var methodName = property.Split('(')[0]; // TODO: 此处改成regex的命名参数;

                            var funcPars = funcParsMatch.Groups[0].Success
                                ? funcParsMatch.Groups[0].Value.Split(',')
                                : new object[] { };

                            var funcParTypes = funcPars.Select(x => x.GetType()).ToArray();

                            ret = theType.GetMethod(methodName, funcParTypes)?.Invoke(ret, funcPars);
                            break;
                        case string property when theType.GetProperties().Select(x => x.Name == property).Count() != 0:
                            ret = theType.GetProperty(property).GetValue(ret);
                            break;
                        case string property when theType.GetFields().Select(x => x.Name == property).Count() != 0:
                            ret = theType.GetField(property).GetValue(ret);
                            break;
                        
                    }

                }

            }

            return ret;
        }
        /// <summary>
        /// 解析多行命令
        /// </summary>
        /// <param name="codes"></param>
        /// <returns />
        public List<Token> ParseBlock(string codes) {
            var tokens = new List<Token>(codes.Length / 10); // 假若平均每 10 个字符是一个 token

            var leftBraceCount = 0;
            var currentToken = new StringBuilder(10); // 假若平均每 10 个字符是一个 token
            foreach (var theChar in codes) {
                switch (theChar) {

                    case '{':
                        leftBraceCount += 1;
                        if (leftBraceCount == 1) {
                            var currentTokenText = currentToken.ToString();
                            if (currentTokenText.Length > 0) {
                                tokens.Add(new Token(currentTokenText));
                                currentToken.Clear();
                            }
                        } else {
                            currentToken.Append(theChar);
                        }
                        break;
                    case '}':
                        leftBraceCount -= 1;
                        if (leftBraceCount == 0) {
                            var currentBlock = new BlockToken(currentToken.ToString());
                            currentBlock.ContentTokens = ParseBlock(currentBlock.Text);
                            tokens.Add(currentBlock);
                            currentToken.Clear();
                        } else {
                            currentToken.Append(theChar);
                        }
                        break;
                    case char s when leftBraceCount > 0:
                        currentToken.Append(s);
                        break;
                    case '|':
                        var tokenText = currentToken.ToString();
                        if (tokenText.Length > 0) {
                            tokens.Add(new Token(currentToken.ToString()));
                            currentToken.Clear();
                        }

                        break;
                    case ';':
                        tokens.Add(new Token(currentToken.ToString()));
                        currentToken.Clear();
                        tokens.Add(new LineEndToken());
                        break;
                    case char s when "\r\n\t ".Contains(s):
                        // 目前只允许用\r\n\t<space>作为美观用字符 遇见后自动跳过, 虽然由于命令行格式导致不会有空格出现在这里
                        break;
                    case char s:
                        currentToken.Append(s);
                        break;
                }
            }

            return tokens;

        }

        public string GetAST(List<Token> tokens, int spaceCount = 0) {
            var ret = new StringBuilder(tokens.Count * 10);
            foreach (var t in tokens) {
                var currentLine = new StringBuilder(10);

                switch (t) {
                    case LineEndToken _t:
                        currentLine.Append(_t.Text);
                        goto default;
                    case BlockToken _t:
                        currentLine.Append(GetAST(_t.ContentTokens, spaceCount + 4));
                        break;
                    case Token _t:
                        currentLine.Append(_t.Text);
                        goto default;
                    default:
                        foreach (var _ in Enumerable.Range(0, spaceCount)) {
                            currentLine.Insert(0, " ");
                        }
                        break;

                }
                currentLine.Append("\n");
                ret.Append(currentLine);
            }

            return ret.ToString();
        }
    }
}
