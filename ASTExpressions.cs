using System;

namespace SLANG
{
  // Expression base class
  public abstract class Expression
  {
    public abstract Symbol Evaluate(RuntimeContext cont);
    public abstract TYPE TypeCheck(CompilationContext cont);
    public abstract TYPE Get_Type();
  }



  //Boolena constant node , stores true or false value
  public class BooleanConstant : Expression
  {
    private Symbol info;

    public BooleanConstant(bool val)
    {
      info = new Symbol();
      info.Name = null;
      info.BooleanValue = val;
      info.Type = TYPE.BOOL;
    }

    public override Symbol Evaluate(RuntimeContext cont) => info;
    public override TYPE TypeCheck(CompilationContext cont) => info.Type;
    public override TYPE Get_Type() => info.Type;
  }

  // Numeric constant node , stores numeric value
  public class NumericConstant : Expression
  {
    private Symbol info;

    public NumericConstant(double val)
    {
      info = new Symbol();
      info.Name = null;
      info.DoubleValue = val;
      info.Type = TYPE.NUMERIC;
    }

    public override Symbol Evaluate(RuntimeContext cont) => info;
    public override TYPE Get_Type() => info.Type;
    public override TYPE TypeCheck(CompilationContext cont) => info.Type;
  }

  // String Literal node , stores string value
  public class StringLiteral : Expression
  {
    private Symbol info;

    public StringLiteral(string val)
    {
      info = new Symbol();
      info.Name = null;
      info.StringValue = val;
      info.Type = TYPE.STRING;
    }

    public override Symbol Evaluate(RuntimeContext cont) => info;
    public override TYPE Get_Type() => info.Type;
    public override TYPE TypeCheck(CompilationContext cont) => info.Type;
  }

  // Variable node , stores variable info
  public class Variable : Expression
  {
    private string _name;
    public TYPE _type;

    public Variable(Symbol info)
    {
      _name = info.Name;
    }

    public Variable(CompilationContext ct, string name, double val)
    {
      Symbol s = new Symbol();
      s.Name = name;
      s.DoubleValue = val;
      s.Type = TYPE.NUMERIC;
      _name = name;
    }

    public Variable(CompilationContext ct, string name, string val)
    {
      Symbol s = new Symbol();
      s.Name = name;
      s.StringValue = val;
      s.Type = TYPE.STRING;
      _name = name;
    }

    public Variable(CompilationContext ct, string name, bool val)
    {
      Symbol s = new Symbol();
      s.Name = name;
      s.BooleanValue = val;
      s.Type = TYPE.BOOL;
      _name = name;
    }

    public string GetName() => _name;

    public string Name
    {
      get => _name;
      set => _name = value;
    }

    public override Symbol Evaluate(RuntimeContext cont)
    {
      if (cont.TABLE == null)
      {
        return null;
      }
      else
      {
        Symbol result = cont.TABLE.Get(_name);
        return result;
      }
    }

    public override TYPE TypeCheck(CompilationContext cont)
    {
      if (cont.TABLE == null)
      {
        return TYPE.ILLEGAL;
      }
      else
      {
        Symbol result = cont.TABLE.Get(_name);
        if (result != null)
        {
          _type = result.Type;
          return result.Type;
        }
        return TYPE.ILLEGAL;
      }
    }

    public override TYPE Get_Type()
    {
      return _type;
    }
  }

  // Binary operations
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

    public override TYPE Get_Type()
    {
      return _type;
    }
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

    public override TYPE Get_Type()
    {
      return _type;
    }
  }

  public class BinaryMult : Expression
  {
    private Expression _exp1, _exp2;
    private TYPE _type;
    public BinaryMult(Expression e1, Expression e2)
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
    public override TYPE Get_Type()
    {
      return _type;
    }
  }

  public class BinaryDiv : Expression
  {
    private Expression _exp1, _exp2;
    private TYPE _type;
    public BinaryDiv(Expression e1, Expression e2)
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
    public override TYPE Get_Type()
    {
      return _type;
    }
  }

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
    public override TYPE Get_Type()
    {
      return _type;
    }
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

    public override TYPE Get_Type()
    {
      return _type;
    }
  }

}



//   public class BinaryExpression : Expression
//   {
//     private OPERATOR _operator;
//     private Expression _expression1, _expression2;
//     private double _value;

//     public BinaryExpression(Expression ex1, OPERATOR op, Expression ex2)
//     {
//       _expression1 = ex1;
//       _operator = op;
//       _expression2 = ex2;
//     }

//     public override double Evaluate(RuntimeContext cont)
//     {
//       switch (_operator)
//       {
//         case OPERATOR.PLUS:
//           return _expression1.Evaluate(cont) + _expression2.Evaluate(cont);
//         case OPERATOR.MINUS:
//           return _expression1.Evaluate(cont) - _expression2.Evaluate(cont);
//         case OPERATOR.MULT:
//           return _expression1.Evaluate(cont) * _expression2.Evaluate(cont);
//         case OPERATOR.DIV:
//           return _expression1.Evaluate(cont) / _expression2.Evaluate(cont);
//         default:
//           return Double.NaN;
//       }
//     }
//   }



//   public class UnaryExpression : Expression
//   {
//     private OPERATOR _operator;
//     private Expression _expression;

//     public UnaryExpression(OPERATOR op, Expression ex)
//     {
//       _operator = op;
//       _expression = ex;
//     }

//     public override double Evaluate(RuntimeContext cont)
//     {
//       switch (_operator)
//       {
//         case OPERATOR.PLUS:
//           return _expression.Evaluate(cont);
//         case OPERATOR.MINUS:
//           return -_expression.Evaluate(cont);
//         default:
//           return Double.NaN;
//       }
//     }
//   }
// }