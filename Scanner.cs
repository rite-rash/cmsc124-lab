using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;

namespace Ck
{
    public class Scanner{
        
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();

        private  int start = 0;
        private int current = 0;
        private int line = 1;

        private static readonly Dictionary<string, TokenType> Keywords = new()
        {
            { "var", TokenType.VAR },
            { "print", TokenType.PRINT },
        };
        public Scanner(String source){
            _source = source;
        }

        public List<Token> scanTokens()
        {
            while (!atEnd())
            {
                start = current;
                scanToken();
            }

            _tokens.Add(new Token("", line, null, TokenType.EOF));
            return _tokens;

        }

        //--------- HELPERS -------
                private bool atEnd()
        {
            return current >= _source.Length; //check if current cursor is at the end
        }

        private char advance()
        {
            char c = _source[current++];
            return c;
        }

        private char peek()
        {
            if (atEnd())
            {
                return '\0';
            }
            return _source[current];
        }

        private void addToken(TokenType type)
        {
            string lexeme = _source.Substring(start, current - start);
            _tokens.Add(new Token(lexeme, line, null, type)); //null for single character tokens
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '+': addToken(TokenType.PLUS); //change
                    break;
                case '-': addToken(TokenType.MINUS);
                    break;
                case '*': addToken(TokenType.STAR);
                    break;
                case '/': addToken(TokenType.SLASH);
                    break;
                case '=': addToken(TokenType.EQUAL);
                    break;
                case '(': addToken(TokenType.LEFT_PAREN);
                    break;
                case ')': addToken(TokenType.RIGHT_PAREN);
                    break;
                case '{': addToken(TokenType.LEFT_BRACE);
                    break;
                case '}': addToken(TokenType.RIGHT_BRACE);
                    break;
                case ' ':
                case '\t':
                    break;
                default:
                    break;
            }
        }

    }
}