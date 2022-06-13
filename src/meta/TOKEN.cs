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
    SEMI,

    // variables
    VAR_NUMBER,
    VAR_STRING,
    VAR_BOOLEAN,
    ASSIGN,

    NUMERIC,
    BOOLEAN_TRUE,
    BOOLEAN_FALSE,
    STRING,


  }
}