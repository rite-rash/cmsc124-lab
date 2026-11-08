using System;
using System.Collections.Generic;


namespace Ck
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private readonly List<HashSet<string>> scopes = new() { new HashSet<string>() }; //List of sets to represent scopes
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

        //allow errors to be reported and then continue parsing, instead of stopping at the first error
        private void synchronize()
        {
            while (!atEnd())
            {
                if (advance().Type == TokenType.SEMICOLON) return;
            }
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

            //makes x a variable if it matches a label token,
            //then creates a new variable node with same label
            if (match(TokenType.LABEL)) return new Variable(previous());

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

        // -------- STATEMENT RULES --------
        private Statement? declaration()
        {
            try
            {
                if (match(TokenType.TRACK)) return varDeclaration();
                return statement();
            }
            catch (Exception)
            {
                synchronize();
                return null;
            }
        }

        private Statement varDeclaration()
        {
            Token name = consume(TokenType.LABEL, "Expect variable name.");
            Node? initializer = null;
            if (match(TokenType.EQUAL)) initializer = expression();
            consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            declare(name);
            return new TrackStatement(name, initializer);
        }

        private Statement statement()
        {
            if (match(TokenType.STREAM)) return printStatement();
            return expressionStatement();
        }

        private Statement printStatement()
        {
            Node value = expression();
            consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new StreamStatement(value);
        }

        private Statement expressionStatement()
        {
            Node expr = expression();
            consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new ExpressionStatement(expr);
        }


// -------- SCOPE MANAGEMENT --------

        //if variable is already declared in the current scope, report an error
        private void declare(Token name)
        {
            if (!scopes[^1].Add(name.Lexeme)) //innermost set, which is the last one in the list (global scope prioritized)
                Errors.ReportError(name.Line, $"Already a variable named '{name.Lexeme}' in this scope.");
        }
        public Tree parse()
        {
            var statements = new List<Statement>();
            while (!atEnd())
            {
                Statement? stmt = declaration();
                if (stmt != null) statements.Add(stmt);
            }
            return new Tree(statements);
        }
    }
}