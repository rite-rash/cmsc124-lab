using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;

namespace Ck
{
    public class Scanner{
        // memory states
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        //for lookups of keywords
        private static readonly Dictionary<string, TokenType> Keywords = new()
        {
            { "track", TokenType.TRACK }, //var
            { "stream", TokenType.STREAM },//print
            {"album", TokenType.ALBUM }, //function
            {"repeat", TokenType.REPEAT}
        };

        // for creating an object Scanner
        public Scanner(String source)
        {
            _source = source;
        }

        // calls scanToken till not EOF then returns list of tokens 
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
        //check if the current char is at the end
        private bool atEnd()
        {
            return current >= _source.Length;
        }

        //consumes char first at given pos and moves the cursor
        private char advance()
        {
            char c = _source[current++];
            return c;
        }

        //checks current char
        private char peek()
        {
            if (atEnd())
            {
                return '\0';
            }
            return _source[current];
        }


        //checks char after current
        private char peekNext()
        {
            if (current + 1 >= _source.Length) return '\0';
            return _source[current + 1];
        }


        private void stringLiteral()
        {
            while (peek() != '"' && !atEnd())
            {
                if (peek() == '\n') line++;
                advance();
            }
            if (atEnd()) return;
            advance(); //consume closing quote

            string value = _source.Substring(start + 1, current - start - 2);
            addToken(TokenType.LYRIC, value); 
        }

        private void numericLiteral()
        {
            // consume the integer part
            while (isDigit(peek()))
            {
                advance();
            }

            if (peek() == '.' && isDigit(peekNext()))
            {
                advance(); // consume .
                //consume next digits
                while (isDigit(peek()))
                {
                    advance();
                }
            }

            string numberText = _source.Substring(start, current - start);
            double value = double.Parse(numberText);

            addToken(TokenType.BEAT, value);

        }
        private bool isAlphaNumeric(char c) {
            return (isAlpha(c)) || isDigit(c);
        }

        private void label()
        {
            while (isAlphaNumeric(peek()))
            {
                advance();
            }
            string textLabel = _source.Substring(start, current - start);

            TokenType type = Keywords.GetValueOrDefault(textLabel, TokenType.LABEL);
            addToken(type);
        }



        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '_') ;
        }


        private void addToken(TokenType type)
        {
            addToken(type, null);
        }
        //add tokens with format: lexeme, line, literal (object?), type
        private void addToken(TokenType type, object? literal)
        {
            string lexeme = _source.Substring(start, current - start);
            _tokens.Add(new Token(lexeme, line, literal, type));
        }

        private void scanToken()
        {
            char c = advance(); //consume curr char and move cursor by 1
            switch (c)
            {

                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case '+': addToken(TokenType.MIX); break;
                case '-': addToken(TokenType.MINUS); break;
                case '*': addToken(TokenType.STAR); break;
                case '"':
                    stringLiteral();
                    break;

                //handle both single and multicharacter 
                case '/':
                    if (peek() == '/') //if it's a comment consume the whole thing without adding as token
                    {
                        while (!atEnd() && peek() != '\n') //keep consuming as long as next char is not EOF or \n
                        {
                            advance();
                        }
                    }
                    else
                    {
                        addToken(TokenType.SLASH);
                    }
                    break;

                case '=':
                    if (peek() == '=')
                    {
                        advance();
                        addToken(TokenType.EQUAL_EQUAL);
                    }
                    else
                    {
                        addToken(TokenType.EQUAL);
                    }
                    break;
                case '!':
                    if (peek() == '=')
                    {
                        advance();
                        addToken(TokenType.CHECK_IF);
                    }
                    else
                    {
                        addToken(TokenType.BANG);
                    }
                    break;
                case '<':
                    if (peek() == '=')
                    {
                        advance();
                        addToken(TokenType.LESS_EQUAL);
                    }
                    else
                    {
                        addToken(TokenType.LESS);
                    }
                    break;
                case '>':
                    if (peek() == '=')
                    {
                        advance();
                        addToken(TokenType.GREATER_EQUAL);
                    }
                    else
                    {
                        addToken(TokenType.GREATER);
                    }
                    break;


                //special cases
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;


                default:
                    if (isDigit(c)) { numericLiteral(); }
                    else if (isAlpha(c)) { label(); }
                    else { }
                    break;
            }
        }

    }
}