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

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }
    }
}
