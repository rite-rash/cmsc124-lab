//DESCRIPTION: represents a single token produced by scanner
//              - data container holding the metadata about a recognized word or symbol

namespace Ck
{
    public class Token
    {
        // properties (metadata)
        public string Lexeme { get; } // raw string extracted
        public int Line { get; } 
        //evaluated value for literals
        // null if no literal value like for keywords and operators)
        public object? Literal { get; } 

        //categ of token from the toketype enum
        public TokenType Type { get; }


        //token obj containing the metadata after assignment
        public Token(string lexeme, int line, object? literal, TokenType type)
        {
            Lexeme = lexeme;
            Line = line;
            Literal = literal;
            Type = type;
        }

        // convert to string for printing output
        public override string ToString()
        {
            string literalS = (Literal == null) ? "null" : Literal.ToString();

            //clean for output
            string safeLex = Lexeme.Replace("\r\n", "\\n").Replace("\n", "\\n"); 
            string safeLit = literalS.Replace("\r\n", "\\n").Replace("\n", "\\n");

            
            return $"Token(type={Type}, lexeme={safeLex}, literal={safeLit}, line={Line})";
        }
    }
}
    