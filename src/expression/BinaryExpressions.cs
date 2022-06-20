namespace SLANG
{
  public class BinaryPlus : Expression
  {
    private Expression _exp1, _exp2;
    private TYPE _type;

    public BinaryPlus(Expression e1, Expression e2)
    {
      _exp1 = e1;
      _exp2 = e2;
    }

    public override Symbol accept(RuntimeContext cont,Visitor v)
    {
      return v.visit(cont,this);
    }

    public Expression GetLExpression() => _exp1;
    public Expression GetRExpression() => _exp2;

    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE lEval = _exp1.TypeCheck(cont);
      TYPE rEval = _exp2.TypeCheck(cont);

      if (lEval == rEval && lEval != TYPE.BOOL)
      {
        _type = lEval;
        return _type;
      }
      else
      {
        throw new Exception("Invalid types for binary plus");
      }
    }

    public override TYPE Get_Type()=> _type;
  }

  public class BinaryMinus : Expression
  {
    private Expression _exp1, _exp2;
    private TYPE _type;

    public BinaryMinus(Expression e1, Expression e2)
    {
      _exp1 = e1;
      _exp2 = e2;
    }

    public override Symbol accept(RuntimeContext cont,Visitor v)
    {
      return v.visit(cont,this);
    }
    public Expression GetLExpression() => _exp1;
    public Expression GetRExpression() => _exp2;

    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE lEval = _exp1.TypeCheck(cont);
      TYPE rEval = _exp2.TypeCheck(cont);

      if (lEval == rEval && lEval != TYPE.BOOL)
      {
        _type = lEval;
        return _type;
      }
      else
      {
        throw new Exception("Invalid types for binary minus");
      }
    }

    public override TYPE Get_Type()=> _type;
  }

  public class BinaryMultiplication : Expression
  {
    private Expression _exp1, _exp2;
    private TYPE _type;

    public BinaryMultiplication(Expression e1, Expression e2)
    {
      _exp1 = e1;
      _exp2 = e2;
    }

    public Expression GetLExpression() => _exp1;
    public Expression GetRExpression() => _exp2;

    public override Symbol accept(RuntimeContext cont,Visitor v)
    {
      return v.visit(cont,this);
    }

    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE lEval = _exp1.TypeCheck(cont);
      TYPE rEval = _exp2.TypeCheck(cont);

      if (lEval == rEval && lEval != TYPE.BOOL)
      {
        _type = lEval;
        return _type;
      }
      else
      {
        throw new Exception("Invalid types for binary minus");
      }
    }

    public override TYPE Get_Type()=> _type;
  }

  public class BinaryDivision : Expression
  {
    private Expression _exp1, _exp2;
    private TYPE _type;

    public BinaryDivision(Expression e1, Expression e2)
    {
      _exp1 = e1;
      _exp2 = e2;
    }

    public Expression GetLExpression() => _exp1;
    public Expression GetRExpression() => _exp2;

    public override Symbol accept(RuntimeContext cont,Visitor v)
    {
      return v.visit(cont,this);
    }


    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE lEval = _exp1.TypeCheck(cont);
      TYPE rEval = _exp2.TypeCheck(cont);

      if (lEval == rEval && lEval != TYPE.BOOL)
      {
        _type = lEval;
        return _type;
      }
      else
      {
        throw new Exception("Invalid types for binary minus");
      }
    }

    public override TYPE Get_Type()=> _type;
  }
}