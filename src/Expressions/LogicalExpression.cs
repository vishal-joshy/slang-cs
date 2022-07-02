using System.Reflection.Emit;

namespace SLANG
{
    class LogicalExp : Expression
    {
        private Expression _lExp, _rExp;
        private TOKEN _operator;
        private TYPE_INFO _type;

        public LogicalExp(TOKEN op, Expression e1, Expression e2)
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

            if (lEval.Type != rEval.Type)
                throw new Exception("Type mismatch");

            if (lEval.Type != TYPE_INFO.BOOL)
                throw new Exception("Invalid type for logical operation");

            if (_operator == TOKEN.AND)
                result.BooleanValue = (lEval.BooleanValue && rEval.BooleanValue);
            else if (_operator == TOKEN.OR)
                result.BooleanValue = (lEval.BooleanValue || rEval.BooleanValue);
            else
                throw new Exception("Invalid Logical Operator");

            return result;
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

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _lExp.Compile(cont);
            _rExp.Compile(cont);

            if (_operator == TOKEN.AND)
                cont.CodeOutput.Emit(OpCodes.And);
            else if (_operator == TOKEN.OR)
                cont.CodeOutput.Emit(OpCodes.Or);

            return true;
        }

        public override TYPE_INFO get_type() => _type;
    }


    class LogicalNot : Expression
    {
        private Expression _exp;
        private TYPE_INFO _type;

        public LogicalNot(Expression e1)
        {
            _exp = e1;
        }

        public override SYMBOL Evaluate(RUNTIME_CONTEXT cont)
        {
            SYMBOL eval = _exp.Evaluate(cont);

            if (eval.Type == TYPE_INFO.BOOL)
            {
                SYMBOL ret_val = new SYMBOL("", TYPE_INFO.BOOL);
                ret_val.BooleanValue = !eval.BooleanValue;
                return ret_val;
            }
            else
            {
                throw new Exception("Invalid type for logical Expression");
            }
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

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _exp.Compile(cont);

            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
            cont.CodeOutput.Emit(OpCodes.Ceq);
            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
            cont.CodeOutput.Emit(OpCodes.Ceq);

            return true;
        }

        public override TYPE_INFO get_type() => _type;
    }
}