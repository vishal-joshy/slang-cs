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
    public Expression GetExpression() => _exp;
    public override Symbol accept(RuntimeContext cont, Visitor v)
    {
      return v.visit(cont, this);
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
    public Expression GetExpression() => _exp;
    public override Symbol accept(RuntimeContext cont, Visitor v)
    {
      return v.visit(cont, this);
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
