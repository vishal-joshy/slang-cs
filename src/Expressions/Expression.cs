namespace SLANG
{
    public abstract class Expression
    {
        public abstract TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont);
        public abstract TYPE_INFO get_type();
        public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont);
        public abstract SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v);
    }
}