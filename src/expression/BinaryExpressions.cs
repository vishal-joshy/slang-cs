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

    public override Symbol Evaluate(RuntimeContext cont)
    {
      Symbol lEval = _exp1.Evaluate(cont);
      Symbol rEval = _exp2.Evaluate(cont);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue + rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else if (lEval.Type == TYPE.STRING && rEval.Type == TYPE.STRING)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.STRING;
        result.StringValue = lEval.StringValue + rEval.StringValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary plus");
      }
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

    public override Symbol Evaluate(RuntimeContext cont)
    {
      Symbol lEval = _exp1.Evaluate(cont);
      Symbol rEval = _exp2.Evaluate(cont);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue - rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary minus");
      }
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

  public class BinaryMultiplication : Expression
  {
    private Expression _exp1, _exp2;
    private TYPE _type;

    public BinaryMultiplication(Expression e1, Expression e2)
    {
      _exp1 = e1;
      _exp2 = e2;
    }

    public override Symbol Evaluate(RuntimeContext cont)
    {
      Symbol lEval = _exp1.Evaluate(cont);
      Symbol rEval = _exp2.Evaluate(cont);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue * rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary minus");
      }
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

    public override Symbol Evaluate(RuntimeContext cont)
    {
      Symbol lEval = _exp1.Evaluate(cont);
      Symbol rEval = _exp2.Evaluate(cont);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue / rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary minus");
      }
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