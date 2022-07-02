using System.Collections;

namespace SLANG
{
      public class ProcedureBuilder
    {
        private string _procedureName = "";
        private COMPILATION_CONTEXT _ctx = null;
        private ArrayList _formals = new ArrayList();
        private ArrayList _statements = new ArrayList();
        private TYPE_INFO _type = TYPE_INFO.ILLEGAL;

        public ProcedureBuilder(string name, COMPILATION_CONTEXT cont)
        {
            _ctx = cont;
            _procedureName = name;
        }

        public bool AddLocal(SYMBOL info)
        {
            _ctx.TABLE.Add(info);
            return true;
        }

        public bool AddFormals(SYMBOL info)
        {
            _formals.Add(info);
            return true;
        }

        public TYPE_INFO TypeCheck(Expression e)
        {
            return e.TypeCheck(_ctx);
        }

        public void AddStatement(Statement st)
        {
            _statements.Add(st);
        }

        public SYMBOL GetSymbol(string strname)
        {
            return _ctx.TABLE.Get(strname);
        }

        public TYPE_INFO TYPE
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public SymbolTable TABLE
        {
            get
            {
                return _ctx.TABLE;
            }
        }

        public COMPILATION_CONTEXT Context
        {
            get
            {
                return _ctx;
            }
        }

        public string Name
        {
            get
            {
                return _procedureName;
            }

            set
            {
                _procedureName = value;
            }
        }

        public Procedure GetProcedure()
        {
            Procedure ret = new Procedure(_procedureName, _formals, _statements, _ctx.TABLE, _type);
            return ret;
        }
    }
}