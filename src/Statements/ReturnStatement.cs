using System.Reflection.Emit;

namespace SLANG
{
    public class ReturnStatement : Statement
    {
        private Expression _exp;
        private SYMBOL _inf = null;

        public ReturnStatement(Expression e)
        {
            _exp = e;
        }

        public Expression GetExpression() => _exp;
        public void SetSymbol(SYMBOL inf)
        {
            _inf = inf;
        }

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }
    }
}