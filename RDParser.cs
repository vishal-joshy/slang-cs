namespace SLANG
{
  public class RDParser : LexicalAnalyzer
  {
    private TOKEN _currentToken;

    public RDParser(String str) : base(str) { }

    public Expression CallExpr()
    {
      _currentToken = GetToken();
      return Expr();
    }

    //<Expr> ::= <Term> | Term { + | - } <Expr>
    public Expression Expr()
    {
      TOKEN token;
      Expression returnValue = Term();
      while (_currentToken == TOKEN.PLUS || _currentToken == TOKEN.MINUS)
      {
        token = _currentToken;
        _currentToken = GetToken();
        Expression e1 = Expr();
        returnValue = new BinaryExpression(returnValue,
                   token == TOKEN.PLUS ? OPERATOR.PLUS : OPERATOR.MINUS
                  , e1);
      }

      return returnValue;
    }

    // <Term> ::= <Factor> | <Factor> {*|/} <Term>
    public Expression Term()
    {
      TOKEN token;
      Expression returnValue = Factor();
      while (_currentToken == TOKEN.MULT || _currentToken == TOKEN.DIV)
      {
        token = _currentToken;
        _currentToken = GetToken();
        Expression e1 = Term();

        returnValue = new BinaryExpression(returnValue,
            token == TOKEN.MULT ? OPERATOR.MULT : OPERATOR.DIV, e1);
      }
      return returnValue;
    }

    // <Factor>::= <number> | ( <expr> ) | {+|-} <factor>
    public Expression Factor()
    {
      TOKEN token;
      Expression returnValue = null;
      // <number>
      if (_currentToken == TOKEN.DOUBLE)
      {
        returnValue = new NumericConstant(GetNumber());
        _currentToken = GetToken();
      }
      // ( <expr> ) brackets
      else if (_currentToken == TOKEN.OPAREN)
      {
        _currentToken = GetToken();
        returnValue = Expr();
        if (_currentToken == TOKEN.CPAREN)
        {
          _currentToken = GetToken();
        }
        else
        {
          throw new Exception("Missing closing parenthesis");
        }
      }

      // {+/-} factor
      else if (_currentToken == TOKEN.OPAREN)
      {
        _currentToken = GetToken();
        returnValue = Expr();

        if (_currentToken != TOKEN.CPAREN)
        {
          Console.WriteLine("Missing )");
          throw new Exception();
        }

        _currentToken = GetToken();
      }

      else if (_currentToken == TOKEN.PLUS || _currentToken == TOKEN.MINUS)
      {
        token = _currentToken;
        _currentToken = GetToken();
        returnValue = Factor();

        returnValue = new UnaryExpression(
             token == TOKEN.PLUS ? OPERATOR.PLUS : OPERATOR.MINUS, returnValue);
      }
      else
      {
        Console.WriteLine("Illegal Token");
        throw new Exception();
      }
      return returnValue;
    }
  }
}