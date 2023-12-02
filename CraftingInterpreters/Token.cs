using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftingInterpreters
{
    internal class Token
    {
        readonly TokenType type;
        readonly String lexeme;
        readonly Object literal;
        readonly int line;

        public Token(TokenType type, String lexeme, Object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public string to_string()
        {
            return type + " " + lexeme + " " + literal;
        }
    }
}
