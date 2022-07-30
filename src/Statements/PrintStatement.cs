using System.Reflection.Emit;

namespace SLANG
{
    public class PrintStatement : Statement
    {
        private Expression _exp;
        private bool _isPrintLine;
        public PrintStatement(Expression e, bool line)
        {
            _exp = e;
            _isPrintLine = line;
        }

        public Expression GetExpression() => _exp;
        public bool GetIsPrintLine() => _isPrintLine;

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }
    }
}