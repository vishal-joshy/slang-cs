using System.Reflection.Emit;

namespace SLANG
{
    public abstract class CONTEXT
    {
        public abstract ILGenerator CodeOutput
        {
            get;
        }

        public abstract TModule GetProgram();
        public abstract LocalBuilder GetLocal(int s);
        public abstract int DeclareLocal(System.Type type);

        public abstract SymbolTable TABLE
        {
            get;
            set;
        }
    }
}
