using System.Reflection.Emit;

namespace SLANG
{
    public class PrintStatement : Statement
    {
        private Expression _exp;
        private bool _isPrintLine;
        public PrintStatement(Expression e, bool line)
        {
            _exp = e;
            _isPrintLine = line;
        }

        public Expression GetExpression() => _exp;
        public bool GetIsPrintLine() => _isPrintLine;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _exp.Compile(cont);

            System.Type SType = Type.GetType("System.Console");
            Type[] parameters = new Type[1];

            TYPE_INFO tData = _exp.get_type();

            if (tData == TYPE_INFO.STRING)
                parameters[0] = typeof(string);
            else if (tData == TYPE_INFO.NUMERIC)
                parameters[0] = typeof(double);
            else
                parameters[0] = typeof(bool);

            if (_isPrintLine)
            {
                cont.CodeOutput.Emit(OpCodes.Call, SType.GetMethod("WriteLine", parameters));
            }
            else
            {
                cont.CodeOutput.Emit(OpCodes.Call, SType.GetMethod("Write", parameters));
            }
            return true;
        }
    }
}