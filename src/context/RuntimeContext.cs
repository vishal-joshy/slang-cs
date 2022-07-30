using System.Reflection.Emit;

namespace SLANG
{
    public class RUNTIME_CONTEXT : CONTEXT
    {
        private SymbolTable _table;
        private TModule _program = null;

        public RUNTIME_CONTEXT(TModule p)
        {
            _table = new SymbolTable();
            _program = p;
        }

        public override TModule GetProgram() => _program;

        public override SymbolTable TABLE
        {
            get => _table;
            set => _table = value;
        }

        public override LocalBuilder GetLocal(int s)
        {
            throw new Exception("GetLocal not available for Runtime Context");
        }
        public override int DeclareLocal(System.Type type)
        {
            throw new Exception("DeclareLocal not available for Runtime Context");
        }

        public override ILGenerator CodeOutput
        {
            get;
        }
    }
}