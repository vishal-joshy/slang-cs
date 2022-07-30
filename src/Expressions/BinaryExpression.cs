using System.Reflection.Emit;

namespace SLANG
{
    public class BinaryExpression : Expression
    {
        private Expression _lExp, _rExp;
        TYPE_INFO _type;
        ARITHMETIC_OPERATOR _operator;

        public BinaryExpression(ARITHMETIC_OPERATOR op, Expression e1, Expression e2)
        {
            _lExp = e1;
            _rExp = e2;
            _operator = op;
        }

        public Expression GetLExp() => _lExp;
        public Expression GetRExp() => _rExp;
        public ARITHMETIC_OPERATOR GetOperator() => _operator;

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            TYPE_INFO lEval = _lExp.TypeCheck(cont);
            TYPE_INFO rEval = _rExp.TypeCheck(cont);

            if (lEval == rEval && lEval != TYPE_INFO.BOOL)
            {
                _type = lEval;
                return _type;
            }
            else
            {
                throw new Exception("Type mismatch failure");
            }
        }

        public override TYPE_INFO get_type() => _type;
    }
}
