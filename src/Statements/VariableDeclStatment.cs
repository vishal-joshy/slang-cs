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

        public override SYMBOL Execute(RUNTIME_CONTEXT cont)
        {
            cont.TABLE.Add(_inf);
            _var = new Variable(_inf);
            return null;
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            System.Type type = (_inf.Type == TYPE_INFO.BOOL) ?
                typeof(bool) : (_inf.Type == TYPE_INFO.NUMERIC) ?
                typeof(double) : typeof(string);

            int s = cont.DeclareLocal(type);
            _inf.loc_position = s;
            cont.TABLE.Add(_inf);
            return true;
        }
    }
}