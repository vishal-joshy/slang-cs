using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    public abstract class CompilationUnit
    {
        public abstract SYMBOL Execute(RUNTIME_CONTEXT cont, ArrayList actuals);
        public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont);
    }

    public abstract class PROC
    {
        public abstract SYMBOL Execute(RUNTIME_CONTEXT cont, ArrayList formals);
        public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont);
    }

    public class TModule : CompilationUnit
    {
        private ArrayList _procedures = null;
        private ArrayList _compiledProcedures = null;
        private ExeGenerator _exe = null;

        public TModule(ArrayList procs)
        {
            _procedures = procs;
        }

        public bool CreateExecutable(string name)
        {
            _exe = new ExeGenerator(this, name);
            Compile(null);
            _exe.Save();
            return true;
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            _compiledProcedures = new ArrayList();
            foreach (Procedure p in _procedures)
            {
                DNET_EXECUTABLE_GENERATION_CONTEXT dtx = new DNET_EXECUTABLE_GENERATION_CONTEXT(this, p, _exe.type_builder);
                _compiledProcedures.Add(dtx);
                p.Compile(dtx);
            }
            return true;
        }

        public override SYMBOL Execute(RUNTIME_CONTEXT cont, ArrayList actuals)
        {
            Procedure p = FindProcedure("Main");
            if (p != null)
            {
                return p.Execute(cont, actuals);
            }
            return null;
        }

        public MethodBuilder GetEntryPoint(string funcName)
        {
            foreach (DNET_EXECUTABLE_GENERATION_CONTEXT cont in _compiledProcedures)
            {
                if (cont.MethodName.Equals(funcName))
                {
                    return cont.MethodHandle;
                }
            }
            return null;
        }

        public Procedure FindProcedure(string str)
        {
            foreach (Procedure p in _procedures)
            {
                string pName = p.Name;

                if (pName.ToUpper().CompareTo(str.ToUpper()) == 0)
                    return p;
            }
            return null;
        }
    }

    public class Procedure : PROC
    {
        private string _name;
        private ArrayList _formals = null;
        private ArrayList _statements = null;
        private SymbolTable _locals = null;
        private SYMBOL return_value = null;
        private TYPE_INFO _type = TYPE_INFO.ILLEGAL;

        public Procedure(string name, ArrayList formals, ArrayList stats, SymbolTable locals, TYPE_INFO type)
        {
            _name = name;
            _formals = formals;
            _statements = stats;
            _locals = locals;
            _type = type;
        }

        public TYPE_INFO TYPE
        {
            get
            {
                return _type;
            }
        }

        public ArrayList FORMALS
        {
            get
            {
                return _formals;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public SYMBOL ReturnValue()
        {
            return return_value;
        }

        public TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            return TYPE_INFO.NUMERIC;
        }


        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            if (_formals != null)
            {
                int i = 0;
                foreach (SYMBOL b in _formals)
                {
                    System.Type type = (b.Type == TYPE_INFO.BOOL) ? typeof(bool) : (b.Type == TYPE_INFO.NUMERIC) ? typeof(double) : typeof(string);
                    int s = cont.DeclareLocal(type);
                    b.loc_position = s;
                    cont.TABLE.Add(b);
                    cont.CodeOutput.Emit(OpCodes.Ldarg, i);
                    cont.CodeOutput.Emit(OpCodes.Stloc, cont.GetLocal(s));
                    i++;
                }
            }

            foreach (Statement e1 in _statements)
            {
                e1.Compile(cont);
            }

            cont.CodeOutput.Emit(OpCodes.Ret);
            return true;
        }

        public override SYMBOL Execute(RUNTIME_CONTEXT cont, ArrayList actuals)
        {
            ArrayList vars = new ArrayList();
            int i = 0;

            if (_formals != null && actuals != null)
            {
                i = 0;
                foreach (SYMBOL b in _formals)
                {
                    SYMBOL inf = actuals[i] as SYMBOL;
                    inf.Name = b.Name;
                    cont.TABLE.Add(inf);
                    i++;
                }
            }

            foreach (Statement e1 in _statements)
            {
                return_value = e1.Execute(cont);

                if (return_value != null)
                    return return_value;
            }
            return null;
        }
    }
}
