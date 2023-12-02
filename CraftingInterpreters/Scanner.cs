using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CraftingInterpreters.TokenType;

namespace CraftingInterpreters
{
    internal class Scanner
    {
        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;
        private static readonly Dictionary<String, TokenType> keywords = new Dictionary<String, TokenType>
        {
            {"and", AND },
            {"class", CLASS },
            {"else", ELSE },
            {"false", FALSE },
            {"true", TRUE },
            {"for", FOR },
            {"fun", FUN },
            {"if", IF },
            {"nil", NIL },
            {"or", OR },
            {"print", PRINT },
            {"return", RETURN },
            {"super", SUPER },
            {"this", THIS },
            {"var", VAR },
            {"while", WHILE }
        };

        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> scan_tokens()
        {
            while (!is_at_end())
            {
                start = current;
                scan_token();
            }
            tokens.Add(new Token(EOF, "", null, line));
            return tokens;
        }
        public bool is_at_end()
        {
            return current >= source.Length;
        }
        private void scan_token()
        {
            char c = advance();
            switch (c)
            {
                case '(': add_token(LEFT_PAREN); break;
                case ')': add_token(RIGHT_PAREN); break;
                case '{': add_token(LEFT_BRACE); break;
                case '}': add_token(RIGHT_BRACE); break;
                case ',': add_token(COMMA); break;
                case '.': add_token(DOT); break;
                case '-': add_token(MINUS); break;
                case '+': add_token(PLUS); break;
                case ';': add_token(SEMICOLON); break;
                case '*': add_token(STAR); break;
                case '!':
                    if (match('=')) { add_token(BANG_EQUAL); break; }
                    else { add_token(BANG); break; }
                case '=':
                    if (match('=')) { add_token(EQUAL); break; }
                    else { add_token(EQUAL); break; }
                case '<':
                    if (match('=')) { add_token(LESS_EQUAL); break; }
                    else { add_token(LESS); break; }
                case '>':
                    if (match('=')) { add_token(GREATER_EQUAL); break; }
                    else { add_token(GREATER); break; }
                case '/':
                    if (match('/'))
                    {
                        while (peek() != '\n' && !is_at_end()) { advance(); }
                    }
                    else { add_token(SLASH); }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;
                case '"': str(); break;

                default:
                    if (is_digit(c)) { number(); }
                    else if (is_alpha(c)) {  identifier(); }
                    else { Slox.error(line, "Unexpected character."); }
                    break;

            }
        }
        private void identifier()
        {
            while (is_aphanumeric(peek())) { advance(); }

            String text = source.Substring(start, current);
            TokenType type = IDENTIFIER;
            if (keywords.ContainsKey(text)) { type = keywords[text]; }
            add_token(type);
        }
        private bool is_alpha(char c)
        {
            if (c >= 'a' && c <= 'z') { return true; }
            if (c >= 'A' &&  c <= 'Z') { return true; }
            if (c == '_') { return true; }
            else { return false; }
        }
        private bool is_aphanumeric(char c)
        {
            if(is_alpha(c) || is_digit(c)) { return true; }
            else { return false; }
        }
        private void number()
        {
            while(is_digit(peek())) { advance(); }
            if (peek() == '.' && is_digit(peek_next()))
            {
                advance();
                while(is_digit(peek())) { advance(); }
            }
            add_token(NUMBER, double.Parse(source.Substring(start, current)));
        }
        private bool is_digit(char c)
        {
            if (c >= '0' && c <= '9') { return true; } 
            else { return false; }
        }
        private void str()
        {
            while (peek() != '"' && !is_at_end())
            {
                if (peek() == '\n') { line++; }
                advance();
            }
            if(is_at_end())
            {
               Slox.error(line, "Unterminated string.");
               return;
            }

            advance();
            String value = source.Substring(start + 1, current - 1);
            add_token(STRING, value);   
            
        }
        private bool match(char expeted)
        {
            if (is_at_end()) { return false; }
            if (source[current] != expeted) { return false; }

            current++;
            return true;
        }
        private char peek()
        {
            if(is_at_end()) { return '\0'; }
            return source[current];
        }
        private char peek_next()
        {
            if (current+1 >= source.Length) { return '\0'; }
            return source[current++];
        }
        private char advance()
        {
            return source[current++];
        }
        private void add_token(TokenType type)
        {
            add_token(type, null);
        }
        private void add_token(TokenType type, Object literal)
        {
            string text = source.Substring(start, current);
            tokens.Add(new Token(type, text, literal, line));
        }
    }
}
