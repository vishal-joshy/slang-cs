using System.Reflection.Emit;

namespace SLANG
{
    public class BinaryExpression : Expression
    {
        private Expression lExp, rExp;
        TYPE_INFO _type;
        ARITHMETIC_OPERATOR _operator;

        public BinaryExpression(ARITHMETIC_OPERATOR op, Expression e1, Expression e2)
        {
            lExp = e1;
            rExp = e2;
            _operator = op;
        }

        public override SYMBOL Evaluate(RUNTIME_CONTEXT cont)
        {
            SYMBOL lEval = lExp.Evaluate(cont);
            SYMBOL rEval = rExp.Evaluate(cont);

            if (lEval.Type == TYPE_INFO.STRING)
            {
                SYMBOL result = new SYMBOL("", TYPE_INFO.STRING);
                result.StringValue = lEval.StringValue + rEval.StringValue;
                return result;
            }
            else if (lEval.Type == TYPE_INFO.NUMERIC)
            {
                SYMBOL result = new SYMBOL("", TYPE_INFO.NUMERIC);
                switch (_operator)
                {
                    case ARITHMETIC_OPERATOR.PLUS: result.DoubleValue = lEval.DoubleValue + rEval.DoubleValue; break;
                    case ARITHMETIC_OPERATOR.MINUS: result.DoubleValue = lEval.DoubleValue - rEval.DoubleValue; break;
                    case ARITHMETIC_OPERATOR.MULT: result.DoubleValue = lEval.DoubleValue * rEval.DoubleValue; break;
                    case ARITHMETIC_OPERATOR.DIV: result.DoubleValue = lEval.DoubleValue / rEval.DoubleValue; break;
                    default: throw new Exception("Invalid operator for arithmetic operation");
                }
                return result;
            }
            else
            {
                throw new Exception("Invalid type");
            }
        }


        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            TYPE_INFO lEval = lExp.TypeCheck(cont);
            TYPE_INFO rEval = rExp.TypeCheck(cont);

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
            lExp.Compile(cont);
            rExp.Compile(cont);

            if (_type == TYPE_INFO.STRING)
            {
                Type[] str2 = { typeof(string), typeof(string) };
                cont.CodeOutput.Emit(OpCodes.Call, typeof(String).GetMethod("Concat", str2));
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
