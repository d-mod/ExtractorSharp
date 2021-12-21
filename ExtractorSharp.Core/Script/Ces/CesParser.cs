using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtractorSharp.Script.Ces {
    public partial class CesParser {

        /// <summary>
        /// 上下文
        /// </summary>
        private Dictionary<string, object> _context = new Dictionary<string, object>();

        internal class Token {
            private string Value { get; }
            public Token(string value) {
                this.Value = value;
            }

            public override string ToString() {
                return this.Value;
            }

        }

        public enum SymbolType {
            PLUS,
            MINUS,
            STAR,
            DIVISION
        }

        internal class SymbolToken : Token {
            public SymbolToken(SymbolType symbol) : base(symbol.ToString()) {

            }
        }

        internal class BlockToken : Token {
            private List<Token> Content { get; }

            public BlockToken(string value, List<Token> content) : base(value) {
                this.Content = content;
            }
        }

        public object Invoke(string content) {
            var tokens = this.ParseBlock(content);
            return this._context;
            Console.WriteLine("1");
        }




        /// <summary>
        /// 解析多行命令
        /// </summary>
        /// <param name="codes"></param>
        /// <returns />
        private List<Token> ParseBlock(string codes) {
            var tokens = new List<Token>(codes.Length / 10); // 假若平均每 10 个字符是一个 token

            var leftBraceCount = 0;
            var isString = false;
            var currentToken = new StringBuilder(10); // 假若平均每 10 个字符是一个 token
            foreach(var theChar in codes) {
                switch(theChar) {
                    case '{':
                        leftBraceCount += 1;
                        if(leftBraceCount == 1) {
                            var currentTokenText = currentToken.ToString();
                            if(currentTokenText.Length > 0) {
                                tokens.Add(new Token(currentTokenText));
                                currentToken.Clear();
                            }
                        } else {
                            currentToken.Append(theChar);
                        }
                        break;
                    case '}':
                        leftBraceCount -= 1;
                        if(leftBraceCount == 0) {
                            var text = currentToken.ToString();
                            var currentBlock = new BlockToken(text, this.ParseBlock(text));
                            tokens.Add(currentBlock);
                            currentToken.Clear();
                        } else {
                            currentToken.Append(theChar);
                        }
                        break;
                    case '"':
                        isString = !isString;
                        if(!isString) {
                            tokens.Add(new Token(currentToken.ToString()));
                        }
                        break;
                    case char s when leftBraceCount > 0 || isString:
                        currentToken.Append(s);
                        break;
                    case char s when "\r\n\t\0; ".Contains(s):
                        tokens.Add(new Token(currentToken.ToString()));
                        currentToken.Clear();
                        break;
                    case char s when "+-*/".Contains(s):
                        tokens.Add(new Token(currentToken.ToString()));
                        break;
                    case char s:
                        currentToken.Append(s);
                        break;
                }
            }
            return tokens;

        }

    }
}
