namespace SLANG
{
    public abstract class Expression
    {
        public abstract TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont);
        public abstract TYPE_INFO get_type();
        public abstract SYMBOL accept(CONTEXT cont, IVisitor v);
    }
}