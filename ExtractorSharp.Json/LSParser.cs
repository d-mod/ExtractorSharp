using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Json {
    public sealed class LSParser {

        private int index;

        private char[] source;

        private LSToken token = LSToken.None;
        private Dictionary<LSObject, string> dictionary { set; get; }

        public LSObject Decode(string source) {
            this.source = source.ToCharArray();
            this.index = 0;
            this.dictionary = new Dictionary<LSObject, string>();
            this.LookAhead();
            if(this.token == LSToken.LBrace || this.token == LSToken.LBracket) {
                return this.ParseObject();
            }

            return new LSObject();
        }

        private LSObject ParseObject() {
            this.Consume();
            var obj = new LSObject();
            var name = new StringBuilder();
            while(true) {
                switch(this.LookAhead()) {
                    case LSToken.Comma:
                        this.Consume();
                        //分隔符,清除name
                        name.Clear();
                        break;
                    case LSToken.RBracket: //]} 对象终止
                    case LSToken.RBrace:
                        this.Consume();
                        //处理未能识别的表达式
                        foreach(var t in this.dictionary.Keys.ToArray()) {
                            var t2 = obj.Find(this.dictionary[t]);
                            if(t2 != null) {
                                if(t.ValueType == LSType.Object) {
                                    t2.CopyTo(t);
                                } else if(t.ValueType == LSType.String && t2.ValueType != LSType.Object) {
                                    var s = t.Value as string;
                                    t.Value = s.Replace("${" + this.dictionary[t] + "}", t2.Value?.ToString());
                                }
                                this.dictionary.Remove(t);
                            }
                        }
                        return obj;
                    case LSToken.Colon: //: 
                        this.Consume();
                        if(this.LookAhead() != LSToken.Expression) {
                            var value = this.ParseValue();
                            var t = obj.Add(name.ToString(), value);
                            if(value is string s) {
                                var match = Regex.Match(s, @"\$\{.*\}");
                                var exp = match.Value;
                                if(exp != string.Empty && (match.Index < 1 || s[match.Index - 1] != '\\')) {
                                    this.dictionary.Add(t, exp.Substring(2, exp.Length - 3));
                                }
                            }
                            name.Clear();
                        }
                        break;
                    case LSToken.Dot:
                        this.Consume();
                        //refrence
                        name.Append(".");
                        break;
                    case LSToken.Expression:
                        var key = this.ParseExpression();
                        var current = obj.Find(key);
                        var tp = obj.Add(name.ToString(), current);
                        if(current == null) {
                            this.dictionary.Add(tp, key);
                        }

                        current = null;
                        name.Clear();
                        break;
                    case LSToken.Identifier:
                        name.Append(this.ParseIdentifier());
                        break;
                    case LSToken.Charcter: // 字符/字符串
                    case LSToken.String:
                        var val = this.ParseValue();
                        if(this.LookAhead() == LSToken.Colon) {
                            //兼容字符串命名
                            name.Clear();
                            name.Append(val + "");
                        } else {
                            obj.Add(val);
                        }
                        break;
                    case LSToken.LBrace:
                    case LSToken.LBracket:
                    case LSToken.True:
                    case LSToken.False:
                    case LSToken.Number:
                    case LSToken.Null:
                        obj.Add(this.ParseValue());
                        break;
                    default:
                        throw new Exception($"无法识别的字符!'{this.source[this.index]}'");
                }
            }
        }

        private object ParseValue() {
            switch(this.LookAhead()) {
                case LSToken.Number:
                    return this.ParseNumber();
                case LSToken.Charcter:
                case LSToken.String:
                    return this.ParseString();
                case LSToken.LBracket:
                case LSToken.LBrace:
                    return this.ParseObject();
                case LSToken.True:
                    this.Consume();
                    return true;
                case LSToken.False:
                    this.Consume();
                    return false;
                case LSToken.Null:
                    this.Consume();
                    break;
            }
            return null;
        }

        /// <summary>
        ///     分析标识符
        /// </summary>
        /// <returns></returns>
        private string ParseIdentifier() {
            return this.ParseString(".[]:,; \t\n\r");
        }

        /// <summary>
        ///     分析表达式 格式{....}
        /// </summary>
        /// <returns></returns>
        private string ParseExpression() {
            this.index++;
            var s = this.ParseString("}");
            this.index++;
            return s;
        }


        /// <summary>
        ///     分析字符串 "...."
        /// </summary>
        /// <returns></returns>
        private string ParseString() {
            var c = this.source[this.index - 1];
            var s = this.ParseString(c + "");
            this.index++;
            return s;
        }


        /// <summary>
        ///     分析所有文本，包括字符串和标识符
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        private string ParseString(string end) {
            this.Consume();
            var str_buf = new StringBuilder();
            var runIndex = -1;
            var l = this.source.Length;
            var p = this.source;
            while(this.index < l) {
                var c = p[this.index++];
                if(end.IndexOf(c) > -1) {
                    this.index--;
                    if(runIndex != -1) {
                        if(str_buf.Length == 0) {
                            return this.source.Substring(runIndex, this.index - runIndex);
                        }
                        str_buf.Append(this.source, runIndex, this.index - runIndex);
                    }
                    return str_buf.ToString();
                }
                if(this.index == l) {
                    break;
                }
                if(runIndex != -1) {
                    str_buf.Append(this.source, runIndex, this.index - runIndex - 1);
                    runIndex = -1;
                }
                if(c != '\\') {
                    if(runIndex == -1) {
                        runIndex = this.index - 1;
                    }
                    continue;
                }
                switch(p[this.index++]) {
                    case '\\':
                    case '"':
                    case '\'':
                    case 'b':
                    case 'f':
                    case 'n':
                    case 'r':
                    case 't':
                        str_buf.Append(p[this.index - 1].ToString().Format());
                        break;
                    case '$':
                        str_buf.Append(@"\$");
                        break;
                    case 'u':
                        if(l - this.index < 4) {
                            break;
                        }
                        var codePoint = this.ParseUnicode(p[this.index], p[this.index + 1], p[this.index + 2], p[this.index + 3]);
                        str_buf.Append((char)codePoint);
                        this.index += 4;
                        break;
                }
            }
            throw new Exception("Unexpectedly reached end of string");
        }

        private uint ParseSingleChar(char c1, uint multipliyer) {
            uint p1 = 0;
            if(c1 >= '0' && c1 <= '9') {
                p1 = (uint)(c1 - '0') * multipliyer;
            } else if(c1 >= 'A' && c1 <= 'F') {
                p1 = (uint)(c1 - 'A' + 10) * multipliyer;
            } else if(c1 >= 'a' && c1 <= 'f') {
                p1 = (uint)(c1 - 'a' + 10) * multipliyer;
            }
            return p1;
        }

        /// <summary>
        ///     处理unicode编码
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        /// <param name="c4"></param>
        /// <returns></returns>
        private uint ParseUnicode(char c1, char c2, char c3, char c4) {
            var p1 = this.ParseSingleChar(c1, 0x1000);
            var p2 = this.ParseSingleChar(c2, 0x100);
            var p3 = this.ParseSingleChar(c3, 0x10);
            var p4 = this.ParseSingleChar(c4, 1);
            return p1 + p2 + p3 + p4;
        }


        /// <summary>
        ///     分析数字
        /// </summary>
        /// <returns></returns>
        private object ParseNumber() {
            this.Consume();
            // Need to start back one place because the first digit is also a token and would have been consumed
            var startIndex = this.index - 1;
            var dec = false;
            do {
                if(this.index == this.source.Length) {
                    break;
                }
                var c = this.source[this.index];

                if(char.IsNumber(c) || c == '.' || c == '-' || c == '+' || c == 'e' || c == 'E') {
                    if(c == '.' || c == 'e' || c == 'E') {
                        dec = true;
                    }
                    if(++this.index == this.source.Length) {
                        break; //throw new Exception("Unexpected end of string whilst parsing number");
                    }
                    continue;
                }
                break;
            } while(true);
            var len = this.index - startIndex;
            var s = this.source.Substring(startIndex, len);
            if(dec) {
                return decimal.Parse(s, NumberFormatInfo.InvariantInfo);
            }
            if(len < 9) {
                return int.Parse(s);
            }
            if(len >= 9 && len < 20) {
                return long.Parse(s);
            }
            return decimal.Parse(s, NumberFormatInfo.InvariantInfo);
        }

        private LSToken LookAhead() {
            return this.token = this.token != LSToken.None ? this.token : this.NextToken();
        }


        private void Consume() {
            this.token = LSToken.None;
        }

        private LSToken NextToken() {
            char c;
            // Skip past whitespace
            do {
                c = this.source[this.index];
                if(c == '/' && this.source[this.index + 1] == '/') {
                    // c++ style single line comments
                    this.index += 2;
                    do {
                        c = this.source[this.index];
                        if(c == '\r' || c == '\n') {
                            break; // read till end of line
                        }
                    } while(++this.index < this.source.Length);
                }
                if(c > ' ') {
                    break;
                }

                if(c != '\n' && c != '\r' && c != ' ' && c != '\t') {
                    break;
                }
            } while(++this.index < this.source.Length);

            if(this.index == this.source.Length) {
                throw new Exception("Reached end of string unexpectedly");
            }

            c = this.source[this.index];

            switch(c) {
                case '{':
                    this.index++;
                    return LSToken.LBrace;
                case '}':
                    this.index++;
                    return LSToken.RBrace;
                case '[':
                    this.index++;
                    return LSToken.LBracket;
                case ']':
                    this.index++;
                    return LSToken.RBracket;
                case ',':
                    this.index++;
                    return LSToken.Comma;
                case ':':
                    this.index++;
                    return LSToken.Colon;
                case '\'':
                    this.index++;
                    return LSToken.Charcter;
                case '"':
                    this.index++;
                    return LSToken.String;
                case '$': //引用
                    if(this.source[++this.index] == '{') {
                        return LSToken.Expression;
                    }
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                case '+':
                    this.index++;
                    return LSToken.Number;
                case '.':
                    if(char.IsNumber(this.source[this.index++])) {
                        return LSToken.Number;
                    }
                    return LSToken.Dot;
                default:
                    if(this.IsKeyword("true")) {
                        return LSToken.True;
                    }
                    if(this.IsKeyword("false")) {
                        return LSToken.False;
                    }
                    if(this.IsKeyword("null")) {
                        return LSToken.Null;
                    }
                    //identifier
                    if(IsIdentifier(c)) {
                        return LSToken.Identifier;
                    }
                    break;
            }
            return LSToken.None;
        }


        private static bool IsIdentifier(char c) {
            return char.IsLetter(c) || c == '_';
        }


        private bool IsKeyword(string keyword) {
            if(this.source.Length - this.index >= keyword.Length) {
                if(this.source.Substring(this.index, keyword.Length).EqualsIgnoreCase(keyword)) {
                    if(this.source.Length - this.index >= keyword.Length) {
                        var c = this.source[this.index + keyword.Length];
                        //避免关键字前缀的标识符,例如trueab 或者 false123之类
                        if(IsIdentifier(c) && char.IsNumber(c)) {
                            return false;
                        }
                    }
                    this.index += keyword.Length;
                    return true;
                }
            }
            return false;
        }
    }
}