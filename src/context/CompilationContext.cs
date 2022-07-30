using System.Reflection.Emit;

namespace SLANG
{
    public class COMPILATION_CONTEXT : CONTEXT
    {
        private SymbolTable _table;

        public COMPILATION_CONTEXT()
        {
            _table = new SymbolTable();
        }

        public override SymbolTable TABLE
        {
            get => _table;
            set => _table = value;
        }

        public override TModule GetProgram() => null;
        public override LocalBuilder GetLocal(int s)
        {
            throw new Exception("GetLocal not available for compilation Context");
        }
        public override int DeclareLocal(System.Type type)
        {
            throw new Exception("DeclareLocal not available for compilation Context");
        }

        public override ILGenerator CodeOutput
        {
            get;
        }
    }
}