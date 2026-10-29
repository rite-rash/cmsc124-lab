using System;
using System.Collections.Generic;


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

        private bool matchSameLine(params TokenType[] types)
        {
            if (!atEnd() && peek().Line != previous().Line)
                return false;
            return match(types);
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
            Errors.ReportError(peek().Line, msg);
            throw new Exception(msg);
        }


        //------- RULES ------
        private Node expression() => logicOr();

        //under equality grammar
        private Node logicOr()
        {
            Node expr = logicAnd();
            while (match(TokenType.SOLO))
            {
                Token op = previous();
                Node right = logicAnd();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Node logicAnd()
        {
            Node expr = equality();
            while (match(TokenType.CHORD))
            {
                Token op = previous();
                Node right = equality();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Node equality()
        {
            Node expr = comparison();
            while (match(TokenType.CHECK_IF, TokenType.EQUAL_EQUAL))
            {
                Token op = previous();
                Node right = comparison();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Node comparison()
        {
            Node expr = term();
            while (match(TokenType.GREATER, TokenType.GREATER_EQUAL,
                        TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = previous();
                Node right = term();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Node term()
        {
            Node expr = factor();
            while (matchSameLine(TokenType.MINUS, TokenType.MIX))
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
                Token op = previous();
                Node right = unary();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }


        private Node unary()
        {
            if (match(TokenType.BANG, TokenType.MINUS))
            {
                Token op = previous();
                Node right = unary();
                return new Unary(op, right);
            }
            return primary();
        }

        private Node primary()
        {
            //mark the node as a literal if it's a number or a string
            if (match(TokenType.LIVE)) return new Literal(true);
            if (match(TokenType.MUTE)) return new Literal(false);
            if (match(TokenType.REST)) return new Literal(null);
            if (match(TokenType.BEAT, TokenType.LYRIC))
            {
                return new Literal(previous().Literal);
            }
            // tries to group and create expression of valid when seen open paren
            if (match(TokenType.LEFT_PAREN))
            {
                Node exp = expression();
                consume(TokenType.RIGHT_PAREN, "Expected ')' after expression"); // consume until closiing paren found
                Node newExpression = new Grouping(exp);
                return newExpression;
            }

            //if none matched
            Errors.ReportError(peek().Line, "Expected expression.");
            throw new Exception("Expected expression.");


        }

        public Tree parse()
        {
            var nodes = new List<Node>();
            while (!atEnd())
            {
                try
                {
                    Node expr = expression();
                    consume(TokenType.SEMICOLON, "Expected ';' after expression.");
                    nodes.Add(expr);
                }
                catch (Exception)
                {
                    synchronize();
                }
            }
            return new Tree(nodes);
        }

    
        public List<Statement> parseProgram()
        {
            var statements = new List<Statement>();
            while (!atEnd())
            {
                try
                {
                    statements.Add(statement());
                }
                catch (Exception)
                {
                    synchronize(); //skip past next ;
                }
            }
            return statements;
        }


        private Statement statement()
        {
            if (match(TokenType.STREAM))
            {
                Node value = expression();
                consume(TokenType.SEMICOLON, "Expected ';' after value.");
                return new StreamStatement(value);
            }

            Node expr = expression();
            consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return new ExpressionStatement(expr);
        }

        private void synchronize()
        {
            while (!atEnd())
            {
                if (advance().Type == TokenType.SEMICOLON) return;
            }
        }




    }





}