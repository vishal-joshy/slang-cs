using System.Reflection.Emit;
namespace SLANG{
  public class ReturnStatement : Stmt{
    private Expression _expression;
    private Symbol _s;

    public ReturnStatement(Expression ex){
      _expression = ex;
    }

    public Expression GetExpression() => _expression;
    public Symbol inf{
      get=>_s;
      set=> _s = value;
    }

    public override Symbol accept(RuntimeContext rtx,Visitor v){
      return v.visit(rtx, this);
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx){
      if(_expression != null){
        _expression.Compile(dtx);
      }
      dtx.CodeOutput.Emit(OpCodes.Ret);
      return true;
    }
  }
}