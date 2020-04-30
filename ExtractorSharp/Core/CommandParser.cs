using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.Core
{

    public class Token
    {
        public string Text; // 本段token对应的文本
        public Token(string text)
        {
            Text = text;
        }
    }

    public class BlockToken : Token
    {
        public BlockToken(string text) : base(text) { }
        public List<Token> ContentTokens = new List<Token>();
    }
    public class LineEndToken : Token
    {
        public LineEndToken() : base(";") { }
    }

    public class CommandParser
    {
        private static IConnector Connector => Program.Connector;
        public static Dictionary<string, object> _context = new Dictionary<string, object>(); // 包含中间变量之类的, 全局唯一
        public Dictionary<string, object> _status = new Dictionary<string, object>(); // 包含解析时的中间过程, 每个解析实例唯一
        public Dictionary<string, Func<object, object>> FuncMap;


        public CommandParser()
        {
            FuncMap = new Dictionary<string, Func<object, object>>
            {
                ["asNull"] = arg => null,
                ["addOne"] = arg => (int)arg + 1 as object,
                ["toBool"] = arg => (arg as string) != "false",
                ["toArray"] = arg => (arg as string)?.Split(','),
                ["toInt"] = arg =>
                {
                    switch (arg)
                    {
                        case string iStr:
                            arg = int.Parse(iStr);
                            break;
                        case object[] iObjects:
                            arg = iObjects.Select(x => int.Parse(x as string ?? string.Empty)).ToArray();
                            break;
                    }
                    return arg;
                },
                ["LoadFile"] = arg => Connector.LoadFile((arg as string)?.Replace("|LoadFile", "")),
                ["exit"] = arg =>
                {
                    var code = 0;
                    switch (arg)
                    {
                        case string iStr:
                            code = int.Parse(iStr);
                            break;
                        case int iInt:
                            code = iInt;
                            break;
                    }
                    Environment.Exit(code);
                    return arg;
                },
                ["message"] = arg =>
                {
                    var msg = "";
                    switch (arg)
                    {
                        case object[] argArray:
                            MessageBox.Show(string.Join(",", argArray.Select(x => x.ToString())));
                            break;
                        default:
                            MessageBox.Show(arg.ToString());
                            break;
                    }
                    return arg;
                },
                ["asVar"] = arg =>
                {
                    if (CheckSwitch("hasAsVar"))
                    {
                        throw new Exception("Can't use the command multiple times");
                    }
                    SetSwitch("lastVarValue", arg);
                    SetSwitch("hasAsVar", true);
                    return arg;
                },
                ["useVar"] = arg =>
                {
                    if (CheckSwitch("hasUseVar"))
                        throw new Exception("Can't use the command multiple times");

                    SetSwitch("hasUseVar", true);
                    return arg;
                }
            };
        }

        /// <summary>
        ///     用于检查某个开关是否打开
        /// </summary>
        public bool CheckSwitch(string name)
        {
            return _status.ContainsKey(name) && _status[name] as string == "on";
        }
        /// <summary>
        ///     用于设置某个开关状态
        /// </summary>
        public bool SetSwitch(string name, bool status)
        {
            _status[name] = status ? "on" : "off";
            return status;
        }
        /// <summary>
        ///     用于设置某个开关的值
        /// </summary>
        public object SetSwitch(string name, object status)
        {
            _status[name] = status;
            return status;
        }
        /// <summary>
        ///     用于获取某个开关的值
        /// </summary>
        public object GetSwitch(string name, object defaultValue = null)
        {
            return _status.TryGetValue(name, out var value) ? value : defaultValue;
        }
        /// <summary>
        ///     解析但行命令行, 目前包含:
        ///     无副作用命令(不影响上下文):
        ///         toList toInt LoadFile
        ///         toBool asNull exit
        ///         message
        ///     用法如:
        ///         1|toBool => true
        ///         |asNull => null
        ///         1,2,3|toList => {"1", "2", "3"}
        ///         1,2,3|toList|toInt => {1, 2, 3}
        ///         D:\WeGameApps\地下城与勇士\ImagePacks2\sprite_interface_monstercard.NPK|LoadFile => Album
        ///         asd|message => MessageBox.Show("asd")
        ///         0|exit => 以错误代码 0 退出
        ///         1|exit => 以错误代码 1 退出
        ///     上下文命令(影响上下文): 
        ///         asVar| useVar| $
        ///         forEach| 
        ///     用法如:
        ///         1,2,3|toList|toInt|asVar|list 定义一个变量名作 list, 值为 {1, 2, 3}
        ///         |useVar|list 使用名为 list 的变量
        ///         定义和使用变量有简写形式, 1,2,3|toList|toInt|$list 为赋值 list 为 {1, 2, 3}
        ///         单书写 |$list 为使用变量 list, 即 |useVar|list
        ///         |$list|message => MessageBox.Show(list)
        ///         |useVar|list|message => MessageBox.Show(list)
        /// 
        ///         |$list|forEach|i|{
        ///             i|message;
        ///             i|addOne;
        ///         }
        ///         
        /// </summary>
        /// <param name="command"></param>
        public object ParseInvoke(string command)
        {
            var args = command.Split('|');
            object arg = args[0];
            foreach (var sigCommand in args.Skip(1))
            {
                if (CheckSwitch("hasAsVar"))
                {
                    _context[sigCommand] = GetSwitch("lastVarValue") ?? throw new Exception($"Missing the name of asVar");
                    SetSwitch("hasAsVar", false);
                    SetSwitch("lastVarValue", false);
                }
                else if (CheckSwitch("hasUseVar"))
                {
                    if (!_context.TryGetValue(sigCommand, out var value))
                    {
                        throw new Exception($"Missing the name of UseVar");
                    }

                    arg = value;
                    SetSwitch("hasUseVar", false);
                }
                else
                {
                    if (FuncMap.TryGetValue(sigCommand, out var fun))
                    {
                        arg = fun(arg);
                    }
                    else
                    {
                        throw new Exception($"Can't find the command {arg} in code {string.Join("", args)}");
                    }
                }

            }

            return arg;
        }

        /// <summary>
        /// 解析多行命令
        /// </summary>
        /// <param name="codes"></param>
        /// <returns />
        public List<Token> ParseBlock(string codes)
        {
            var tokens = new List<Token>(codes.Length / 10); // 假若平均每 10 个字符是一个 token

            var leftBraceCount = 0;
            var currentToken = new StringBuilder(10); // 假若平均每 10 个字符是一个 token
            foreach (var theChar in codes)
            {
                switch (theChar)
                {

                    case '{':
                        leftBraceCount += 1;
                        tokens.Add(new Token(currentToken.ToString()));
                        currentToken.Clear();

                        break;
                    case '}':
                        leftBraceCount -= 1;
                        if (leftBraceCount == 0)
                        {
                            var currentBlock = new BlockToken(currentToken.ToString());
                            currentBlock.ContentTokens = ParseBlock(currentBlock.Text);
                            tokens.Add(currentBlock);
                            currentToken.Clear();
                        }
                        break;
                    case char s when leftBraceCount > 0:
                        currentToken.Append(s);
                        break;
                    case '|':
                        tokens.Add(new Token(currentToken.ToString()));
                        currentToken.Clear();
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

        public string GetAST(List<Token> tokens, int spaceCount = 0)
        {
            var ret = new StringBuilder(tokens.Count * 10);
            foreach (var t in tokens)
            {
                foreach (var i in Enumerable.Range(0, spaceCount))
                {
                    ret.Append(" ");
                }

                switch (t)
                {
                    case LineEndToken _t:
                        ret.Append(_t.Text);
                        break;
                    case BlockToken _t:
                        ret.Append(GetAST(_t.ContentTokens, spaceCount + 4));
                        break;
                    case Token _t:
                        ret.Append(_t.Text);
                        break;

                }
                ret.Append("\n");
            }

            return ret.ToString();
        }
    }
}
