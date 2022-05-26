using System;

namespace SLANG
{


  public abstract class Expression
  {
    public abstract double Evaluate(RuntimeContext cont);

    public static string test(){
    return "test";
    }
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

  public class BinaryConstant : Expression
  {
    private Operator _operator;
    private Expression _expression1, _expression2;
    private double _value;

    public BinaryConstant(Expression ex1, Operator op,  Expression ex2)
    {
      _expression1 = ex1;
      _operator = op;
      _expression2 = ex2;
    }

    public override double Evaluate(RuntimeContext cont)
    {
      switch (_operator)
      {
        case Operator.PLUS:
          return _expression1.Evaluate(cont) + _expression2.Evaluate(cont);
        case Operator.MINUS:
          return _expression1.Evaluate(cont) - _expression2.Evaluate(cont);
        case Operator.MULT:
          return _expression1.Evaluate(cont) * _expression2.Evaluate(cont);
        case Operator.DIV:
          return _expression1.Evaluate(cont) / _expression2.Evaluate(cont);
        default:
          return Double.NaN;
      }
    }
  }

  public enum Operator
  {
    PLUS, MINUS, MULT, DIV
  }

  public class UnaryConstant : Expression
  {
    private Operator _operator;
    private Expression _expression;

    public UnaryConstant(Operator op, Expression ex)
    {
      _operator = op;
      _expression = ex;
    }

    public override double Evaluate(RuntimeContext cont)
    {
      switch (_operator)
      {
        case Operator.PLUS:
          return _expression.Evaluate(cont);
        case Operator.MINUS:
          return -_expression.Evaluate(cont);
        default:
          return Double.NaN;
      }
    }
  }
}