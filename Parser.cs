using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.Json;
using System.Xml;

namespace Ck
{
    public class Parser
    {
        private class ParseError : Exception { }
        public bool HadError { get; private set; }
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        // --------- HELPERS -------
        private Token peek()
        {
            return tokens[current];
        }

        private Token previous()
        {
            return tokens[current - 1];
        }

        private bool atEnd()
        {
            return peek().Type == TokenType.EOF;
        }

        private Token advance()
        {
            if (!atEnd())
            {
                current++;
            }
            return previous();

        }

        private bool check(TokenType type)
        {
            if (atEnd())
            {
                return false;
            }
            else
            {
                return peek().Type == type;
            }
        }

        private bool match(params TokenType[] types)
        {
            foreach (TokenType t in types)
            {
                if (check(t))
                {
                    advance();
                    return true;
                }
            }

            return false;
        }

        //provide msg if not match
        private Token consume(TokenType type, String msg)
        {
            if (check(type)) return advance();
            throw err(peek(), msg);
        }


        private ParseError err(Token token, string msg)
        {
            string where = token.Type == TokenType.EOF ? "end" : $"'{token.Lexeme}'";
            Console.Error.WriteLine($"[line {token.Line}] Error at {where}: {msg}");
            HadError = true;
            return new ParseError();
        }


        //rules
        private Node primary()
        {
            //mark the node as a literal if it's a number or a string
            if (match(TokenType.BEAT, TokenType.LYRIC))
            {
                return new Literal(previous().Literal);
            }
            // tries to group and create expression of valid when seen open paren
            if (match(TokenType.LEFT_PAREN))
            { 
                Node exp = expression();
                consume(TokenType.RIGHT_PAREN, "Expects ')' after expression"); // consume until closiing paren found
                Node newExpression = new Grouping(exp);
                return newExpression;
            }

            //if none matched
            throw err(peek(), "Expect expression.");


        }

        private Node expression() => term();

        private Node term()
        {
            Node expr = factor();
            while (match(TokenType.MINUS, TokenType.MIX))
            {
                Token op = previous();
                Node right = factor();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }


        private Node factor()
        {
            Node expr = unary();
            while (match(TokenType.SLASH, TokenType.STAR))
            {
                Token op= previous();
                Node right = unary();
                expr=new Binary(expr, op, right);
            }
            return expr;
        }


        private Node unary()
        {
            if (match(TokenType.BANG, TokenType.MINUS))
            {
                Token op= previous();
                Node right = unary();        
                return new Unary(op, right);
            }
            return primary();
        }

        public List<Node> parse()
        {
            var results = new List<Node>();
            while (!atEnd())
            {
                try
                {
                    results.Add(expression());
                    if (!atEnd() && peek().Line == previous().Line)
                        throw err(peek(), "Expect end of line after expression.");
                }
                catch (ParseError)
                {
                    synchronize();
                }
            }
            return results;
        }

        private void synchronize()
        {
            int errLine = peek().Line;
            advance();
            while (!atEnd() && peek().Line == errLine)
                advance();
        }

    }


}