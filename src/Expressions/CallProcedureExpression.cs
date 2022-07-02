using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    class CallProcedureExpression : Expression
    {
        private Procedure _procedures;
        private ArrayList _actuals;
        private string _procedureName;
        private bool _isRecurse;
        TYPE_INFO _type;

        public CallProcedureExpression(Procedure proc, ArrayList actuals)
        {
            _procedures = proc;
            _actuals = actuals;
        }

        public CallProcedureExpression(string name, bool recurse, ArrayList actuals)
        {
            _procedureName = name;
            if (recurse)
                _isRecurse = true;

            _actuals = actuals;
            _procedures = null;
        }

        public override SYMBOL Evaluate(RUNTIME_CONTEXT cont)
        {
            if (_procedures != null)
            {
                RUNTIME_CONTEXT ctx = new RUNTIME_CONTEXT(cont.GetProgram());
                ArrayList lst = new ArrayList();
                foreach (Expression ex in _actuals)
                {
                    lst.Add(ex.Evaluate(cont));
                }
                return _procedures.Execute(ctx, lst);
            }
            else
            {
                _procedures = cont.GetProgram().FindProcedure(_procedureName);
                RUNTIME_CONTEXT ctx = new RUNTIME_CONTEXT(cont.GetProgram());
                ArrayList lst = new ArrayList();

                foreach (Expression ex in _actuals)
                {
                    lst.Add(ex.Evaluate(cont));
                }
                return _procedures.Execute(ctx, lst);
            }
        }
        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            if (_procedures != null)
            {
                _type = _procedures.TypeCheck(cont);
            }
            return _type;
        }

        public override TYPE_INFO get_type() => _type;


        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            if (_procedures == null)
            {
                _procedures = cont.GetProgram().FindProcedure(_procedureName);
            }

            string name = _procedures.Name;

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
