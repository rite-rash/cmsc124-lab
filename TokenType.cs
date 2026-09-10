namespace Ck;

public enum TokenType
{
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    PLUS, MINUS, STAR, SLASH,

    EQUAL, EQUAL_EQUAL,
    BANG, BANG_EQUAL,
    LESS, LESS_EQUAL,
    GREATER, GREATER_EQUAL,

    IDENTIFIER, STRING, PIECE,

    VAR, PRINT,   //temporary, will define var types lateroror


    NEWLINE, EOF
}