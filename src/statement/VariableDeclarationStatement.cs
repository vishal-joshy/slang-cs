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

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      System.Type type = null;
      if(_info.Type == TYPE.NUMERIC){
        type = typeof(double);
      }else if(_info.Type == TYPE.STRING){
        type = typeof(string);
      }else {
        type = typeof(bool);
      }
      Console.Write(type.ToString());
      int s = dtx.DeclareLocal(type);
      _info.loc_position = s;
      dtx.TABLE.Add(_info);
      return true;
    }
  }
}