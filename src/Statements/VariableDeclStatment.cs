namespace SLANG
{
    public class VariableDeclStatement : Statement
    {
        private SYMBOL _inf = null;
        private Variable _var = null;

        public VariableDeclStatement(SYMBOL s)
        {
            _inf = s;
        }

        public SYMBOL GetSymbol() => _inf;
        public void SetVar(SYMBOL s)
        {
            _var = new Variable(s);
        }

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }
    }
}