using System.Reflection.Emit;

namespace SLANG
{
    public class RelationalExpression : Expression
    {
        private RELATIONAL_OPERATOR _operator;
        private Expression _lExp, _rExp;
        private TYPE_INFO _type;
        private TYPE_INFO _opType;

        public RelationalExpression(RELATIONAL_OPERATOR op, Expression e1, Expression e2)
        {
            _operator = op;
            _lExp = e1;
            _rExp = e2;
        }

        public Expression GetLExp() => _lExp;
        public Expression GetRExp() => _rExp;
        public RELATIONAL_OPERATOR GetOperator() => _operator;

        public override TYPE_INFO get_type() => _type;

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            TYPE_INFO lEval = _lExp.TypeCheck(cont);
            TYPE_INFO rEval = _rExp.TypeCheck(cont);

            if (lEval != rEval)
            {
                throw new Exception("Wrong Type in expression");
            }

            if (lEval == TYPE_INFO.STRING && (!(_operator == RELATIONAL_OPERATOR.EQUALITY || _operator == RELATIONAL_OPERATOR.NOTEQUALITY)))
            {
                throw new Exception("Only == amd != supported for string type ");
            }

            if (lEval == TYPE_INFO.BOOL && (!(_operator == RELATIONAL_OPERATOR.EQUALITY || _operator == RELATIONAL_OPERATOR.NOTEQUALITY)))
            {
                throw new Exception("Only == amd != supported for boolean type ");
            }
            _opType = lEval;
            _type = TYPE_INFO.BOOL;
            return _type;
        }
    }
}