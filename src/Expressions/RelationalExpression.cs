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

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
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

        private bool CompileStringRelOp(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _lExp.Compile(cont);
            _rExp.Compile(cont);

            Type[] str2 = { typeof(string), typeof(string) };

            cont.CodeOutput.Emit(OpCodes.Call, typeof(string).GetMethod("Compare", str2));

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