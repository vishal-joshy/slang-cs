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

    public Variable GetVariable() => _variable;
    public Expression GetExpression() => _expression;

    public override Symbol accept(Visitor v, RuntimeContext rtx)
    {
      return v.visit(rtx,this);
    }
  }
}