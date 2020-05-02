using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.Core {

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
    public class CommandParser {
        private static IConnector Connector => Program.Connector;
        public static Dictionary<string, object> _context = new Dictionary<string, object>(); // 包含中间变量之类的, 全局唯一
        public Dictionary<string, object> _status = new Dictionary<string, object>(); // 包含解析时的中间过程, 每个解析实例唯一
        public Dictionary<string, Func<TokenInvokeParameter, TokenInvokeResult>> FuncMap;


        public CommandParser() {
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
                ["LoadFile"] = arg => new TokenInvokeResult {Ret = Connector.LoadFile(arg.CurrentArg as string).ToArray()},
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
                    if (!_context.TryGetValue(arg.NextToken.Text, out var value)) {
                        throw new Exception($"Missing the name of UseVar");
                    }

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
                        Console.WriteLine(i);
                        InvokeToken(block.ContentTokens);
                    }

                    return new TokenInvokeResult { Ret = iterationVariable, NewCursor = arg.Cursor + 2 };
                },
                ["@"] = arg =>
                {
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
                        switch (t)
                        {
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
                    Connector.Do(APIName, APIPars.ToArray());
                    return new TokenInvokeResult { Ret = null, NewCursor = arg.Cursor + 1 };
                }
            };
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
        public object InvokeToken(List<Token> tokens) {
            var result = new TokenInvokeResult { NewCursor = 0, Ret = "" };
            var parameter = new TokenInvokeParameter { CurrentArg = result.Ret, Cursor = 0, Tokens = tokens };

            for (var i = 0; i < tokens.Count; i++) {
                parameter.Cursor = i;

                if (parameter.CurrentToken is LineEndToken) {
                    continue;
                }

                if (FuncMap.TryGetValue(parameter.CurrentToken.Text, out var fun)) {
                    result = fun(parameter);
                    i = result.NewCursor ?? i;
                } else {
                    // throw new Exception($"Can't find the command {Parameter.CurrentToken.Text} in code {GetAST(tokens)}");
                    result.Ret = parameter.CurrentToken.Text;
                }
                parameter.CurrentArg = result.Ret;
            }

            return result.Ret;
        }

        public object InvokeToken(string tokens) {
            return InvokeToken(ParseBlock(tokens));
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
