namespace SLANG
{
  public class PrintStatement : Stmt
  {
    private Expression _expression;

    public PrintStatement(Expression ex)
    {
      _expression = ex;
    }

    public override Symbol accept(Visitor v, RuntimeContext rtx)
    {
      return v.visit(rtx,this);
    }

    public Expression GetExpression() => _expression;
  }


  public class PrintLineStatement : Stmt
  {
    private Expression _expression;

    public PrintLineStatement(Expression ex)
    {
      _expression = ex;
    }

    public override Symbol accept(Visitor v, RuntimeContext rtx)
    {
      return v.visit(rtx, this);
    }

    public Expression GetExpression() => _expression;
  }
}