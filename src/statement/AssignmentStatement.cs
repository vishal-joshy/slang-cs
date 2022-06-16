namespace SLANG
{
  // Variable assignment statement
  public class AssignmentStatement : Stmt
  {
    private Variable _variable;
    private Expression _expression;

    public AssignmentStatement(Variable var, Expression ex)
    {
      _variable = var;
      _expression = ex;
    }

    public AssignmentStatement(Symbol s, Expression ex)
    {
      _variable = new Variable(s);
      _expression = ex;
    }

    public override Symbol Execute(RuntimeContext con)
    {
      Symbol result = _expression.Evaluate(con);
      con.TABLE.Assign(_variable, result);
      return null;
    }
  }
}