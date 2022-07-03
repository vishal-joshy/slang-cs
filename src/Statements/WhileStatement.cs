using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    public class WhileStatement : Statement
    {
        private Expression _condition;
        private ArrayList _statements;

        public WhileStatement(Expression c, ArrayList s)
        {
            _condition = c;
            _statements = s;
        }

        public Expression GetCondition() => _condition;
        public ArrayList GetBody() => _statements;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            Label true_label, false_label;
            true_label = cont.CodeOutput.DefineLabel();
            false_label = cont.CodeOutput.DefineLabel();

            cont.CodeOutput.MarkLabel(true_label);
            _condition.Compile(cont);
            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
            cont.CodeOutput.Emit(OpCodes.Ceq);
            cont.CodeOutput.Emit(OpCodes.Brfalse, false_label);

            foreach (Statement rst in _statements)
            {
                rst.Compile(cont);
            }

            cont.CodeOutput.Emit(OpCodes.Br, true_label);
            cont.CodeOutput.MarkLabel(false_label);
            return true;
        }
    }


}
