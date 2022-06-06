namespace SLANG
{
  public enum TOKEN
  {
    // operators
    PLUS,
    MINUS,
    MULT,
    DIV,
    // numerics
    DOUBLE,
    // paranthesis
    OPAREN,
    CPAREN,

    ILLEGAL_TOKEN,
    NULL,

    PRINT, // Print
    PRINTLN, // PrintLine
    UNQUOTED_STRING,
    SEMI

  }
}