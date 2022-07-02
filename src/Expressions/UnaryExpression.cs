using System.Reflection.Emit;

namespace SLANG
{
    class UnaryExpression : Expression
    {
        private Expression _exp;
        private TYPE_INFO _type;
        private ARITHMETIC_OPERATOR _operator;

        public UnaryExpression(ARITHMETIC_OPERATOR op, Expression e1)
        {
            _operator = op;
            _exp = e1;
        }

        public override SYMBOL Evaluate(RUNTIME_CONTEXT cont)
        {
            SYMBOL eval = _exp.Evaluate(cont);
            SYMBOL result = new SYMBOL("", TYPE_INFO.NUMERIC);
            if (eval.Type != TYPE_INFO.NUMERIC)
            {
                throw new Exception("Type mismatch");
            }
            if (_operator == ARITHMETIC_OPERATOR.PLUS)
            {
                result.DoubleValue = eval.DoubleValue;
            }
            else
            {
                result.DoubleValue = -eval.DoubleValue;
            }
            return result;
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