using System.Reflection.Emit;

namespace SLANG
{
    public class PrintStatement : Statement
    {
        private Expression _exp;
        private bool printLine;
        public PrintStatement(Expression e, bool line)
        {
            _exp = e;
            printLine = line;
        }

        public override SYMBOL Execute(RUNTIME_CONTEXT cont)
        {
            SYMBOL eval = _exp.Evaluate(cont);
            string result = eval.GetValueAsString();

            if (printLine)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.Write(result);
            }
            return null;
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

            if (printLine)
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