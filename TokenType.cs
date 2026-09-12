

namespace Ck;

public enum TokenType
{
    //single charater
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    MIX, MINUS, STAR, SLASH, EQUAL, BANG, LESS, GREATER,

    //multi-character
    EQUAL_EQUAL,
    CHECK_IF,
    LESS_EQUAL,
    GREATER_EQUAL,

    // literals 
    LABEL, // identifier
    LYRIC,     // string 
    BEAT,       // numeric 

    // keywords
    TRACK,      // Variable declaration ('var')
    STREAM,     // output statement
    ALBUM,      // function definition 
    REPEAT,     //loop

    EOF //end

}