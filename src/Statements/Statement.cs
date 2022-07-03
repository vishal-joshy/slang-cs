namespace SLANG
{
    public abstract class Statement
    {
        public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont);
        public abstract SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v);
    }
}