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

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
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

        public override TYPE_INFO get_type() => _type;

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _exp.Compile(cont);
            if (_operator == ARITHMETIC_OPERATOR.MINUS)
            {
                cont.CodeOutput.Emit(OpCodes.Neg);
            }
            return true;
        }
    }
}