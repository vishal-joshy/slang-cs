namespace SLANG
{
  // Variable Declaration Statement
  public class VariableDeclarationStatement : Stmt
  {
    private Symbol _info;
    public Variable Var { get; set; }

    public VariableDeclarationStatement(Symbol s)
    {
      _info = s;
    }

    public Symbol GetInfo() => _info;

    public override Symbol accept(Visitor v, RuntimeContext rtx)
    {
      return v.visit(rtx,this);
    }
  }
}