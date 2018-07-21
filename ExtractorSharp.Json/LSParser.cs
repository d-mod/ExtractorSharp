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
            index = 0;
            dictionary = new Dictionary<LSObject, string>();
            LookAhead();
            if (token == LSToken.LBrace || token == LSToken.LBracket) return ParseObject();
            return new LSObject();
        }

        private LSObject ParseObject() {
            Consume();
            var obj = new LSObject();
            var name = new StringBuilder();
            while (true) {
                switch (LookAhead()) {
                    case LSToken.Comma:
                        Consume();
                        //分隔符,清除name
                        name.Clear();
                        break;
                    case LSToken.RBracket: //]} 对象终止
                    case LSToken.RBrace:
                        Consume();
                        //处理未能识别的表达式
                        foreach (var t in dictionary.Keys.ToArray()) {
                            var t2 = obj.Find(dictionary[t]);
                            if (t2 != null) {
                                if (t.ValueType == LSType.Object) {
                                    t2.CopyTo(t);
                                } else if (t.ValueType == LSType.String && t2.ValueType != LSType.Object) {
                                    var s = t.Value as string;
                                    t.Value = s.Replace("${" + dictionary[t] + "}", t2.Value?.ToString());
                                }
                                dictionary.Remove(t);
                            }
                        }
                        return obj;
                    case LSToken.Colon: //: 
                        Consume();
                        if (LookAhead() != LSToken.Expression) {
                            var value = ParseValue();
                            var t = obj.Add(name.ToString(), value);
                            if (value is string s) {
                                var match = Regex.Match(s, @"\$\{.*\}");
                                var exp = match.Value;
                                if (exp != string.Empty && (match.Index < 1 || s[match.Index - 1] != '\\')) {
                                    dictionary.Add(t, exp.Substring(2, exp.Length - 3));
                                }
                            }
                            name.Clear();
                        }
                        break;
                    case LSToken.Dot:
                        Consume();
                        //refrence
                        name.Append(".");
                        break;
                    case LSToken.Expression:
                        var key = ParseExpression();
                        var current = obj.Find(key);
                        var tp = obj.Add(name.ToString(), current);
                        if (current == null) dictionary.Add(tp, key);
                        current = null;
                        name.Clear();
                        break;
                    case LSToken.Identifier:
                        name.Append(ParseIdentifier());
                        break;
                    case LSToken.Charcter: // 字符/字符串
                    case LSToken.String:
                        var val = ParseValue();
                        if (LookAhead() == LSToken.Colon) {
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
                        obj.Add(ParseValue());
                        break;
                    default:
                        throw new Exception($"无法识别的字符!'{source[index]}'");
                }
            }
        }

        private object ParseValue() {
            switch (LookAhead()) {
                case LSToken.Number:
                    return ParseNumber();
                case LSToken.Charcter:
                case LSToken.String:
                    return ParseString();
                case LSToken.LBracket:
                case LSToken.LBrace:
                    return ParseObject();
                case LSToken.True:
                    Consume();
                    return true;
                case LSToken.False:
                    Consume();
                    return false;
                case LSToken.Null:
                    Consume();
                    break;
            }
            return null;
        }

        /// <summary>
        ///     分析标识符
        /// </summary>
        /// <returns></returns>
        private string ParseIdentifier() {
            return ParseString(".[]:,; \t\n\r");
        }

        /// <summary>
        ///     分析表达式 格式{....}
        /// </summary>
        /// <returns></returns>
        private string ParseExpression() {
            index++;
            var s = ParseString("}");
            index++;
            return s;
        }


        /// <summary>
        ///     分析字符串 "...."
        /// </summary>
        /// <returns></returns>
        private string ParseString() {
            var c = source[index - 1];
            var s = ParseString(c + "");
            index++;
            return s;
        }


        /// <summary>
        ///     分析所有文本，包括字符串和标识符
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        private string ParseString(string end) {
            Consume();
            var str_buf = new StringBuilder();
            var runIndex = -1;
            var l = source.Length;
            var p = source;
            while (index < l) {
                var c = p[index++];
                if (end.IndexOf(c) > -1) {
                    index--;
                    if (runIndex != -1) {
                        if (str_buf.Length == 0) {
                            return source.Substring(runIndex, index - runIndex);
                        }
                        str_buf.Append(source, runIndex, index - runIndex);
                    }
                    return str_buf.ToString();
                }
                if (index == l) {
                    break;
                }
                if (runIndex != -1) {
                    str_buf.Append(source, runIndex, index - runIndex - 1);
                    runIndex = -1;
                }
                if (c != '\\') {
                    if (runIndex == -1) {
                        runIndex = index - 1;
                    }
                    continue;
                }
                switch (p[index++]) {
                    case '\\':
                    case '"':
                    case '\'':
                    case 'b':
                    case 'f':
                    case 'n':
                    case 'r':
                    case 't':
                        str_buf.Append(p[index - 1].ToString().Format());
                        break;
                    case '$':
                        str_buf.Append(@"\$");
                        break;
                    case 'u':
                        if (l - index < 4) {
                            break;
                        }
                        var codePoint = ParseUnicode(p[index], p[index + 1], p[index + 2], p[index + 3]);
                        str_buf.Append((char) codePoint);
                        index += 4;
                        break;
                }
            }
            throw new Exception("Unexpectedly reached end of string");
        }

        private uint ParseSingleChar(char c1, uint multipliyer) {
            uint p1 = 0;
            if (c1 >= '0' && c1 <= '9') {
                p1 = (uint) (c1 - '0') * multipliyer;
            } else if (c1 >= 'A' && c1 <= 'F') {
                p1 = (uint) (c1 - 'A' + 10) * multipliyer;
            } else if (c1 >= 'a' && c1 <= 'f') {
                p1 = (uint) (c1 - 'a' + 10) * multipliyer;
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
            var p1 = ParseSingleChar(c1, 0x1000);
            var p2 = ParseSingleChar(c2, 0x100);
            var p3 = ParseSingleChar(c3, 0x10);
            var p4 = ParseSingleChar(c4, 1);
            return p1 + p2 + p3 + p4;
        }


        /// <summary>
        ///     分析数字
        /// </summary>
        /// <returns></returns>
        private object ParseNumber() {
            Consume();
            // Need to start back one place because the first digit is also a token and would have been consumed
            var startIndex = index - 1;
            var dec = false;
            do {
                if (index == source.Length) {
                    break;
                }
                var c = source[index];

                if (char.IsNumber(c) || c == '.' || c == '-' || c == '+' || c == 'e' || c == 'E') {
                    if (c == '.' || c == 'e' || c == 'E') {
                        dec = true;
                    }
                    if (++index == source.Length) {
                        break; //throw new Exception("Unexpected end of string whilst parsing number");
                    }
                    continue;
                }
                break;
            } while (true);
            var len = index - startIndex;
            var s = source.Substring(startIndex, len);
            if (dec) {
                return decimal.Parse(s, NumberFormatInfo.InvariantInfo);
            }
            if (len < 9) {
                return int.Parse(s);
            }
            if (len >= 9 && len < 20) {
                return long.Parse(s);
            }
            return decimal.Parse(s, NumberFormatInfo.InvariantInfo);
        }

        private LSToken LookAhead() {
            return token = token != LSToken.None ? token : NextToken();
        }


        private void Consume() {
            token = LSToken.None;
        }

        private LSToken NextToken() {
            char c;
            // Skip past whitespace
            do {
                c = source[index];
                if (c == '/' && source[index + 1] == '/') {
                    // c++ style single line comments
                    index += 2;
                    do {
                        c = source[index];
                        if (c == '\r' || c == '\n') {
                            break; // read till end of line
                        }
                    } while (++index < source.Length);
                }
                if (c > ' ') break;
                if (c != '\n' && c != '\r' && c != ' ' && c != '\t') break;
            } while (++index < source.Length);

            if (index == source.Length) {
                throw new Exception("Reached end of string unexpectedly");
            }

            c = source[index];

            switch (c) {
                case '{':
                    index++;
                    return LSToken.LBrace;
                case '}':
                    index++;
                    return LSToken.RBrace;
                case '[':
                    index++;
                    return LSToken.LBracket;
                case ']':
                    index++;
                    return LSToken.RBracket;
                case ',':
                    index++;
                    return LSToken.Comma;
                case ':':
                    index++;
                    return LSToken.Colon;
                case '\'':
                    index++;
                    return LSToken.Charcter;
                case '"':
                    index++;
                    return LSToken.String;
                case '$': //引用
                    if (source[++index] == '{') {
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
                    index++;
                    return LSToken.Number;
                case '.':
                    if (char.IsNumber(source[index++])) {
                        return LSToken.Number;
                    }
                    return LSToken.Dot;
                default:
                    if (IsKeyword("true")) {
                        return LSToken.True;
                    }
                    if (IsKeyword("false")) {
                        return LSToken.False;
                    }
                    if (IsKeyword("null")) {
                        return LSToken.Null;
                    }
                    //identifier
                    if (IsIdentifier(c)) {
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
            if (source.Length - index >= keyword.Length) {
                if (source.Substring(index, keyword.Length).EqualsIgnoreCase(keyword)) {
                    if (source.Length - index >= keyword.Length) {
                        var c = source[index + keyword.Length];
                        //避免关键字前缀的标识符,例如trueab 或者 false123之类
                        if (IsIdentifier(c) && char.IsNumber(c)) {
                            return false;
                        }
                    }
                    index += keyword.Length;
                    return true;
                }
            }
            return false;
        }
    }
}