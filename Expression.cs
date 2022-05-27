using System;

namespace SLANG
{
  public abstract class Expression
  {
    public abstract double Evaluate(RuntimeContext cont);
  }

  public class RuntimeContext
  {
    public RuntimeContext()
    {

    }
  }

  public class NumericConstant : Expression
  {
    private double _value;

    public NumericConstant(double v)
    {
      _value = v;
    }

    public override double Evaluate(RuntimeContext cont)
    {
      return _value;
    }
  }

  public class BinaryExpression : Expression
  {
    private OPERATOR _operator;
    private Expression _expression1, _expression2;
    private double _value;

    public BinaryExpression(Expression ex1, OPERATOR op,  Expression ex2)
    {
      _expression1 = ex1;
      _operator = op;
      _expression2 = ex2;
    }

    public override double Evaluate(RuntimeContext cont)
    {
      switch (_operator)
      {
        case OPERATOR.PLUS:
          return _expression1.Evaluate(cont) + _expression2.Evaluate(cont);
        case OPERATOR.MINUS:
          return _expression1.Evaluate(cont) - _expression2.Evaluate(cont);
        case OPERATOR.MULT:
          return _expression1.Evaluate(cont) * _expression2.Evaluate(cont);
        case OPERATOR.DIV:
          return _expression1.Evaluate(cont) / _expression2.Evaluate(cont);
        default:
          return Double.NaN;
      }
    }
  }



  public class UnaryExpression : Expression
  {
    private OPERATOR _operator;
    private Expression _expression;

    public UnaryExpression(OPERATOR op, Expression ex)
    {
      _operator = op;
      _expression = ex;
    }

    public override double Evaluate(RuntimeContext cont)
    {
      switch (_operator)
      {
        case OPERATOR.PLUS:
          return _expression.Evaluate(cont);
        case OPERATOR.MINUS:
          return -_expression.Evaluate(cont);
        default:
          return Double.NaN;
      }
    }
  }
}