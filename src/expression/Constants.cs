namespace SLANG
{
  // Boolean constant node , stores true or false value
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
}