using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    public class CallProcedureExpression : Expression
    {
        private Procedure _procedure;
        private ArrayList _actuals;
        private string _procedureName;
        private bool _isRecurse;
        TYPE_INFO _type;

        public CallProcedureExpression(Procedure proc, ArrayList actuals)
        {
            _procedure = proc;
            _actuals = actuals;
        }

        public CallProcedureExpression(string name, bool recurse, ArrayList actuals)
        {
            _procedureName = name;
            if (recurse)
                _isRecurse = true;

            _actuals = actuals;
            _procedure = null;
        }

        public string GetProcedureName() => _procedureName;
        public ArrayList GetActuals() => _actuals;
        public Procedure GetProcedure() => _procedure;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            if (_procedure != null)
            {
                _type = _procedure.TypeCheck(cont);
            }
            return _type;
        }

        public override TYPE_INFO get_type() => _type;


        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            if (_procedure == null)
            {
                _procedure = cont.GetProgram().FindProcedure(_procedureName);
            }

            string name = _procedure.Name;

            TModule module = cont.GetProgram();
            MethodBuilder MBuilder = module.GetEntryPoint(name);

            foreach (Expression exp in _actuals)
            {
                exp.Compile(cont);
            }

            cont.CodeOutput.Emit(OpCodes.Call, MBuilder);
            return true;
        }
    }
}
