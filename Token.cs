namespace Ck
{
    public class Token
    {
        public string Lexeme { get; }
        public int Line { get; }
        public object? Literal { get; }
        public TokenType Type { get; }
        //define token properties
        public Token(string lexeme, int line, object? literal, TokenType type)
        {
            Lexeme = lexeme;
            Line = line;
            Literal = literal;
            Type = type;
        }

        public override string ToString()
        {
            string literalS = (Literal == null) ? "null" : Literal.ToString();

            //clean for output
            string safeLex = Lexeme.Replace("\n", "\\n");
            string safeLit = literalS.Replace("\n", "\\n");

            return $"Token(type={Type}, lexeme={safeLex}, literal={safeLit}, line={Line})";
        }
    }
}
    