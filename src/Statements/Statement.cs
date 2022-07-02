namespace SLANG
{
    public abstract class Statement
    {
        public abstract SYMBOL Execute(RUNTIME_CONTEXT cont);
        public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont);
    }
}