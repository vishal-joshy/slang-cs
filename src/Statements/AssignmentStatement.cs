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

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }
    }
}