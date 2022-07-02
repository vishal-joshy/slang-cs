using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    class IfStatement : Statement
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

        public override SYMBOL Execute(RUNTIME_CONTEXT cont)
        {
            SYMBOL condition = _condition.Evaluate(cont);

            if (condition == null || condition.Type != TYPE_INFO.BOOL)
                return null;

            SYMBOL result = null;

            if (condition.BooleanValue == true)
            {
                foreach (Statement s in _trueStatements)
                {
                    result = s.Execute(cont);
                    if (result != null)
                        return result;
                }
            }
            else if (_elseStatements != null) //condition == false and else statement exists
            {
                foreach (Statement s in _elseStatements)
                {
                    result = s.Execute(cont);
                    if (result != null)
                        return result;
                }
            }
            return null;
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