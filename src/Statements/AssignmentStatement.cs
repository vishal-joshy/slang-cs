using System.Reflection.Emit;

namespace SLANG
{
    public class AssignmentStatement : Statement
    {
        private Variable _variable;
        private Expression _exp;

        public AssignmentStatement(Variable var, Expression e)
        {
            _variable = var;
            _exp = e;
        }

        public AssignmentStatement(SYMBOL var, Expression e)
        {
            _variable = new Variable(var);
            _exp = e;
        }

        public Expression GetExpression() => _exp;
        public Variable GetVariable() => _variable;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            if (!_exp.Compile(cont))
            {
                throw new Exception("Compilation in error string");
            }
            SYMBOL info = cont.TABLE.Get(_variable.Name);
            LocalBuilder lb = cont.GetLocal(info.loc_position);
            cont.CodeOutput.Emit(OpCodes.Stloc, lb);
            return true;
        }
    }
}