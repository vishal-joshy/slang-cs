using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    public class WhileStatment: Stmt
    {
        private ArrayList _statements;
        private Expression _condition;

        public ArrayList GetStatements() => _statements;
        public Expression GetCondition() => _condition;

        public WhileStatment(Expression cond, ArrayList s)
        {
            _condition = cond;
            _statements = s;
        }

        public override Symbol accept(RuntimeContext con, Visitor v)
        {
            return v.visit(con, this);
        }

    public override bool Compile( DNET_EXECUTABLE_GENERATION_CONTEXT dtx ) {
      Label trueLabel, falseLabel;
      trueLabel = dtx.CodeOutput.DefineLabel();
      falseLabel = dtx.CodeOutput.DefineLabel();
      dtx.CodeOutput.MarkLabel(trueLabel);
      _condition.Compile(dtx);
      dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
      dtx.CodeOutput.Emit(OpCodes.Ceq);
      dtx.CodeOutput.Emit(OpCodes.Brfalse, falseLabel);

      foreach (Stmt s in _statements) {
        s.Compile(dtx);
      }
      dtx.CodeOutput.Emit(OpCodes.Br, trueLabel);
      dtx.CodeOutput.MarkLabel(falseLabel);
      return true;
    }
  }
}
