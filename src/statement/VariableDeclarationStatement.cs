namespace SLANG
{
  // Variable Declaration Statement
  public class VariableDeclarationStatement : Stmt
  {
    private Symbol _info;
    private Variable _var;

    public VariableDeclarationStatement(Symbol s)
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
}