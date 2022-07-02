using System.Reflection.Emit;

namespace SLANG
{
    class ReturnStatement : Statement
    {
        private Expression _exp;
        private SYMBOL inf = null;

        public ReturnStatement(Expression e)
        {
            _exp = e;
        }

        public override SYMBOL Execute(RUNTIME_CONTEXT cont)
        {
            inf = (_exp == null) ? null : _exp.Evaluate(cont);
            return inf;
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            if (_exp != null)
            {
                _exp.Compile(cont);
            }
            cont.CodeOutput.Emit(OpCodes.Ret);
            return true;
        }
    }
}