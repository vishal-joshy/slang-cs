using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace SLANG
{
    public class DNET_EXECUTABLE_GENERATION_CONTEXT : CONTEXT
    {
        private ILGenerator _ILout;
        private ArrayList _variables = new ArrayList();
        private SymbolTable _table = new SymbolTable();
        private MethodBuilder _mBuilder = null;
        private TypeBuilder _tBuilder = null;
        private Procedure _procedure = null;
        private TModule _program;

        public DNET_EXECUTABLE_GENERATION_CONTEXT(TModule program, Procedure proc, TypeBuilder bld)
        {
            _procedure = proc;
            _tBuilder = bld;
            _program = program;

            System.Type[] s = new System.Type[_procedure.FORMALS.Count];

            int i = 0;
            foreach (SYMBOL ts in _procedure.FORMALS)
            {
                if (ts.Type == TYPE_INFO.BOOL)
                {
                    s[i] = typeof(bool);
                }
                else if (ts.Type == TYPE_INFO.NUMERIC)
                {
                    s[i] = typeof(double);
                }
                else
                {
                    s[i] = typeof(string);
                }
                i = i + 1;
            }

            if (_procedure.FORMALS.Count == 0)
            {
                s = null;
            }

            System.Type returnType = null;

            if (_procedure.TYPE == TYPE_INFO.BOOL)
                returnType = typeof(bool);
            else if (_procedure.TYPE == TYPE_INFO.STRING)
                returnType = typeof(string);
            else
                returnType = typeof(double);

            if (_procedure.Name.Equals("MAIN"))
            {
                returnType = null;
                _mBuilder = _tBuilder.DefineMethod(_procedure.Name, MethodAttributes.Public | MethodAttributes.Static, returnType, s);
            }
            else
            {
                _mBuilder = _tBuilder.DefineMethod(_procedure.Name, MethodAttributes.Public | MethodAttributes.Static, returnType, s);
            }

            _ILout = _mBuilder.GetILGenerator();
        }

        public string MethodName { get => _procedure.Name; }
        public MethodBuilder MethodHandle { get => _mBuilder; }
        public TypeBuilder TYPEBUILDER { get => _tBuilder; }

        public override SymbolTable TABLE { get => _table; set => _table = value; }
        public override ILGenerator CodeOutput { get => _ILout; }
        public override TModule GetProgram() => _program;

        public override int DeclareLocal(System.Type type)
        {
            LocalBuilder lb = _ILout.DeclareLocal(type);
            return _variables.Add(lb);
        }

        public override LocalBuilder GetLocal(int s)
        {
            return _variables[s] as LocalBuilder;
        }
    }
}
