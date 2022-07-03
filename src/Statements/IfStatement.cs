using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    public class IfStatement : Statement
    {
        private Expression _condition;
        private ArrayList _trueStatements;
        private ArrayList _elseStatements;

        public IfStatement(Expression condition, ArrayList truePart, ArrayList elsePart)
        {
            _condition = condition;
            _trueStatements = truePart;
            _elseStatements = elsePart;
        }

        public Expression GetCondition() => _condition;
        public ArrayList GetTruePart() => _trueStatements;
        public ArrayList GetElsePart() => _elseStatements;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            System.Reflection.Emit.Label trueLabel, falseLabel;
            trueLabel = cont.CodeOutput.DefineLabel();
            falseLabel = cont.CodeOutput.DefineLabel();

            _condition.Compile(cont);

            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
            cont.CodeOutput.Emit(OpCodes.Ceq);

            cont.CodeOutput.Emit(OpCodes.Brfalse, falseLabel);

            foreach (Statement s in _trueStatements)
            {
                s.Compile(cont);
            }

            cont.CodeOutput.Emit(OpCodes.Br, trueLabel);

            cont.CodeOutput.MarkLabel(falseLabel);

            if (_elseStatements != null)
            {
                foreach (Statement s in _elseStatements)
                {
                    s.Compile(cont);
                }
            }
            cont.CodeOutput.MarkLabel(trueLabel);
            return true;
        }
    }
}