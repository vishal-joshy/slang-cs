using System.Reflection.Emit;

namespace SLANG
{
    public class LogicalExpression : Expression
    {
        private Expression _lExp, _rExp;
        private TOKEN _operator;
        private TYPE_INFO _type;

        public LogicalExpression(TOKEN op, Expression e1, Expression e2)
        {
            _operator = op;
            _lExp = e1;
            _rExp = e2;
        }

        public Expression GetLExp() => _lExp;
        public Expression GetRExp() => _rExp;
        public TOKEN GetOperator() => _operator;

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            TYPE_INFO lEval = _lExp.TypeCheck(cont);
            TYPE_INFO rEval = _rExp.TypeCheck(cont);

            if (lEval == rEval && lEval == TYPE_INFO.BOOL)
            {
                _type = TYPE_INFO.BOOL;
                return _type;
            }
            else
            {
                throw new Exception("Wrong Type in expression");
            }
        }

        public override TYPE_INFO get_type() => _type;
    }


    public class LogicalNot : Expression
    {
        private Expression _exp;
        private TYPE_INFO _type;

        public LogicalNot(Expression e1)
        {
            _exp = e1;
        }

        public Expression GetExpression() => _exp;


        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            TYPE_INFO lEval = _exp.TypeCheck(cont);

            if (lEval == TYPE_INFO.BOOL)
            {
                _type = TYPE_INFO.BOOL;
                return _type;
            }
            else
            {
                throw new Exception("Wrong Type in expression");
            }
        }

        public override TYPE_INFO get_type() => _type;
    }
}