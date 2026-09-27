using System.Collections.Generic;
using System.Security.Principal;
using System.Text.Json;
using System.Xml;

namespace Ck
{
    public class Parser
    {
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


        private Exception err(Token token, String msg)
        {
            Console.Error.WriteLine($"[Line {token.Line}] Error: {msg}");
            throw new Exception(msg);
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
            Console.Error.WriteLine($"[line {peek().Line}] Error: Expect expression.");
            throw new Exception("Expect expression.");


        }

        private Node expression() => term();
        
        private Node term()
        {
            Node expr = primary(); 
            while (match(TokenType.MINUS, TokenType.MIX))
                {
                    Token op = previous();
                    Node right = primary();
                    expr = new Binary(expr, op, right);
                }

            return expr;
        }

        public Node parse() => expression();

    }


}