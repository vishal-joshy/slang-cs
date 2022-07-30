namespace SLANG
{
    public abstract class Statement
    {
        public abstract SYMBOL accept(CONTEXT cont, IVisitor v);
    }
}