namespace SLANG
{
  public abstract class Stmt
  {
    public abstract Symbol Execute(RuntimeContext con);
  }


  // Print statements
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

  // Variable Decalaration Statement
  public class VariableDecalataionStatement : Stmt
  {
    private Symbol _info;
    private Variable _var;

    public VariableDecalataionStatement(Symbol s)
    {
      _info = s;
    }

    public override Symbol Execute(RuntimeContext con)
    {
      con.TABLE.Add(_info);
      _var = new Variable(_info);
      return null;
    }
  }

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