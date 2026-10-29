
//DESCRIPTION: define all valid token types (categ)
//      - an enum assigns symbolic label to each syntax element (like the keys for mapping later )


namespace Ck;

public enum TokenType
{
    //single charater
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE, SEMICOLON,
    MIX, MINUS, STAR, SLASH, EQUAL, BANG, LESS, GREATER, COLON,

    //multi-character
    EQUAL_EQUAL,
    CHECK_IF,
    LESS_EQUAL,
    GREATER_EQUAL,


    LABEL, // identifier

    //literal
    LYRIC,     // string 
    BEAT,       // numeric 

    // keywords
    TRACK,      // Variable declaration ('var')
    STREAM,     // output statement
    ALBUM,      // function definition 
    REPEAT,     //loop
    LIVE,       //true
    MUTE,       //false
    REST,       //null/nil
    CHORD,      //and
    SOLO,        //or

    EOF //end

}