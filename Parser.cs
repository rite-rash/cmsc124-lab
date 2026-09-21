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
            return tokens[current-1];
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
            } else
            {
                return peek().Type == type;
            }
        }

        private bool match(params TokenType[] types)
        {
            foreach (TokenType t in types)
            {
                if (check(t)) {
                    advance();
                    return true;
                }
            }

            return false;
        }
        
    }
}