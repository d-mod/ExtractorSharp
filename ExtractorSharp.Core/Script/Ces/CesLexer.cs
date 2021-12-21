using System.Collections.Generic;
using System.Linq;

namespace ExtractorSharp.Script.Ces {
    internal class CesLexer {

        private class Token {

        }

        private char[] Text;


        private int Length;

        private int Position = 0;

        private List<Token> Tokens = new List<Token>();


        public CesLexer(string text) {
            this.Text = text.ToArray();
            this.Length = text.Length;
            this.Position = 0;
        }


        public void ScanToEnd() {
            while(this.Position < this.Length) {
                this.Position = this.Scan(this.Position);
            }
        }

        public int Scan(int i) {
            var c = this.Text[i];
            switch(this.Text[i]) {
                case '{':
                case '}':
                case '[':
                case ']':
                case '(':
                case ')':
                case '+':
                case '-':
                case '*':
                case '/':
                case '&':
                case '|':
                    break;
                case '"':

                    break;
            }
            return ++i;
        }

        public void ScanBlock() {

        }


    }
}
