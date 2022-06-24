using System.Collections;
using System.Reflection.Emit;

namespace SLANG{
  public class IfStatement : Stmt{
    private Expression _condition;
    private ArrayList _trueStmts,_falseStmts;

    public IfStatement(Expression c, ArrayList t, ArrayList f){
      _condition = c;
      _trueStmts = t;
      _falseStmts = f;
    }

    public Expression GetCondition() => _condition;
    public ArrayList GetStatements(string s) => (s == "trueStatements") ? _trueStmts : _falseStmts;

    public override Symbol accept(RuntimeContext rtx, Visitor v)
    {
      return v.visit(rtx, this);
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      Label trueLabel, falseLabel;
      trueLabel = dtx.CodeOutput.DefineLabel();
      falseLabel = dtx.CodeOutput.DefineLabel();

      _condition.Compile(dtx);

      dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
      dtx.CodeOutput.Emit(OpCodes.Ceq);

      dtx.CodeOutput.Emit(OpCodes.Brfalse, falseLabel);

      foreach(Stmt s in _trueStmts){
        s.Compile(dtx);
      }

      dtx.CodeOutput.Emit(OpCodes.Br, trueLabel);
      dtx.CodeOutput.MarkLabel(falseLabel);

      if(_falseStmts!= null){
        foreach(Stmt s in _falseStmts){
          s.Compile(dtx);
        }
      }

      dtx.CodeOutput.MarkLabel(trueLabel);
      return true;
    }
  }
}