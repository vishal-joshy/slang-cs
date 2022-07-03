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

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
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

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _lExp.Compile(cont);
            _rExp.Compile(cont);

            if (_type == TYPE_INFO.STRING)
            {
                Type[] str2 = { typeof(string), typeof(string) };
                cont.CodeOutput.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", str2));
            }
            else
            {
                switch (_operator)
                {
                    case ARITHMETIC_OPERATOR.PLUS: cont.CodeOutput.Emit(OpCodes.Add); break;
                    case ARITHMETIC_OPERATOR.MINUS: cont.CodeOutput.Emit(OpCodes.Sub); break;
                    case ARITHMETIC_OPERATOR.MULT: cont.CodeOutput.Emit(OpCodes.Mul); break;
                    case ARITHMETIC_OPERATOR.DIV: cont.CodeOutput.Emit(OpCodes.Div); break;
                    default: throw new Exception("Invalid operator for arithmetic operation");
                }
            }
            return true;
        }
    }
}
