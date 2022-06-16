namespace SLANG
{
  public class PrintStatement : Stmt
  {
    private Expression _expression;

    public PrintStatement(Expression ex)
    {
      _expression = ex;
    }

    public override Symbol Execute(RuntimeContext con)
    {
      Symbol result = _expression.Evaluate(con);
      Console.Write(result.GetValueAsString());
      return null;
    }
  }


  public class PrintLineStatement : Stmt
  {
    private Expression _expression;

    public PrintLineStatement(Expression ex)
    {
      _expression = ex;
    }

    public override Symbol Execute(RuntimeContext con)
    {
      Symbol result = _expression.Evaluate(con);
      Console.WriteLine(result.GetValueAsString());
      return null;
    }
  }
}