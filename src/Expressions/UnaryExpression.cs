using System.Reflection.Emit;

namespace SLANG
{
    public class UnaryExpression : Expression
    {
        private Expression _exp;
        private TYPE_INFO _type;
        private ARITHMETIC_OPERATOR _operator;

        public UnaryExpression(ARITHMETIC_OPERATOR op, Expression e1)
        {
            _operator = op;
            _exp = e1;
        }

        public Expression GetExpression() => _exp;
        public ARITHMETIC_OPERATOR GetOperator() => _operator;

        public override TYPE_INFO get_type() => _type;

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            TYPE_INFO eval = _exp.TypeCheck(cont);

            if (eval == TYPE_INFO.NUMERIC)
            {
                _type = eval;
                return _type;
            }
            else
            {
                throw new Exception("Type mismatch failure");
            }
        }

    }
}