using System.Reflection.Emit;

namespace SLANG
{
    public class RelationExp : Expression
    {
        private RELATIONAL_OPERATOR _operator;
        private Expression _lExp, _rExp;
        private TYPE_INFO _type;
        private TYPE_INFO _opType;

        public RelationExp(RELATIONAL_OPERATOR op, Expression e1, Expression e2)
        {
            _operator = op;
            _lExp = e1;
            _rExp = e2;
        }

        public override SYMBOL Evaluate(RUNTIME_CONTEXT cont)
        {
            SYMBOL lEval = _lExp.Evaluate(cont);
            SYMBOL rEval = _rExp.Evaluate(cont);

            SYMBOL result = new SYMBOL("", TYPE_INFO.BOOL);

            if (lEval.Type == TYPE_INFO.NUMERIC && rEval.Type == TYPE_INFO.NUMERIC)
            {
                if (_operator == RELATIONAL_OPERATOR.EQUALITY)
                    result.BooleanValue = lEval.DoubleValue == rEval.DoubleValue;
                else if (_operator == RELATIONAL_OPERATOR.NOTEQUALITY)
                    result.BooleanValue = lEval.DoubleValue != rEval.DoubleValue;
                else if (_operator == RELATIONAL_OPERATOR.GREATER_THAN)
                    result.BooleanValue = lEval.DoubleValue > rEval.DoubleValue;
                else if (_operator == RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY)
                    result.BooleanValue = lEval.DoubleValue >= rEval.DoubleValue;
                else if (_operator == RELATIONAL_OPERATOR.LESS_THAN)
                    result.BooleanValue = lEval.DoubleValue < rEval.DoubleValue;
                else if (_operator == RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY)
                    result.BooleanValue = lEval.DoubleValue <= rEval.DoubleValue;

                return result;
            }

            else if (lEval.Type == TYPE_INFO.STRING && rEval.Type == TYPE_INFO.STRING)
            {
                if (_operator == RELATIONAL_OPERATOR.EQUALITY)
                {
                    result.BooleanValue = (String.Compare(lEval.StringValue, rEval.StringValue) == 0) ? true : false;
                }
                else if (_operator == RELATIONAL_OPERATOR.NOTEQUALITY)
                {
                    result.BooleanValue = String.Compare(lEval.StringValue, rEval.StringValue) != 0;
                }
                else
                {
                    result.BooleanValue = false;
                }
                return result;
            }

            if (lEval.Type == TYPE_INFO.BOOL && rEval.Type == TYPE_INFO.BOOL)
            {
                if (_operator == RELATIONAL_OPERATOR.EQUALITY)
                    result.BooleanValue = lEval.BooleanValue == rEval.BooleanValue;
                else if (_operator == RELATIONAL_OPERATOR.NOTEQUALITY)
                    result.BooleanValue = lEval.BooleanValue != rEval.BooleanValue;
                else
                    result.BooleanValue = false;

                return result;
            }
            return null;
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

        private bool CompileStringRelOp(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _lExp.Compile(cont);
            _rExp.Compile(cont);

            Type[] str2 = { typeof(string), typeof(string) };

            cont.CodeOutput.Emit(OpCodes.Call, typeof(String).GetMethod("Compare", str2));

            if (_operator == RELATIONAL_OPERATOR.EQUALITY)
            {
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            else
            {
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            return true;
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            if (_opType == TYPE_INFO.STRING)
            {
                return CompileStringRelOp(cont);
            }

            _lExp.Compile(cont);
            _rExp.Compile(cont);

            if (_operator == RELATIONAL_OPERATOR.EQUALITY)
                cont.CodeOutput.Emit(OpCodes.Ceq);
            else if (_operator == RELATIONAL_OPERATOR.GREATER_THAN)
                cont.CodeOutput.Emit(OpCodes.Cgt);
            else if (_operator == RELATIONAL_OPERATOR.LESS_THAN)
                cont.CodeOutput.Emit(OpCodes.Clt);
            else if (_operator == RELATIONAL_OPERATOR.NOTEQUALITY)
            {
                cont.CodeOutput.Emit(OpCodes.Ceq);
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            else if (_operator == RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY)
            {
                cont.CodeOutput.Emit(OpCodes.Clt);
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            else if (_operator == RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY)
            {
                cont.CodeOutput.Emit(OpCodes.Cgt);
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            return true;
        }

        public override TYPE_INFO get_type()
        {
            return _type;
        }
    }
}