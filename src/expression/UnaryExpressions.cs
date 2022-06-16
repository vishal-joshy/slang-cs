namespace SLANG
{
  // Unary Operations
  public class UnaryPlus : Expression
  {
    private Expression _exp;
    private TYPE _type;
    public UnaryPlus(Expression e)
    {
      _exp = e;
    }
    public override Symbol Evaluate(RuntimeContext cont)
    {
      Symbol eval = _exp.Evaluate(cont);
      if (eval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = eval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid type for unary minus");
      }
    }
    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE eval = _exp.TypeCheck(cont);
      if (eval == TYPE.NUMERIC)
      {
        _type = eval;
        return _type;
      }
      else
      {
        throw new Exception("Invalid type for unary minus");
      }
    }
    public override TYPE Get_Type() => _type;
  }

  public class UnaryMinus : Expression
  {
    private Expression _exp;
    private TYPE _type;

    public UnaryMinus(Expression e)
    {
      _exp = e;
    }

    public override Symbol Evaluate(RuntimeContext cont)
    {
      Symbol eval = _exp.Evaluate(cont);

      if (eval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = -eval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid type for unary minus");
      }
    }

    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE eval = _exp.TypeCheck(cont);

      if (eval == TYPE.NUMERIC)
      {
        _type = eval;
        return _type;
      }
      else
      {
        throw new Exception("Invalid type for unary minus");
      }
    }

    public override TYPE Get_Type() => _type;
  }
}
