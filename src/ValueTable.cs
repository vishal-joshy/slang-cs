namespace SLANG
{
    // Store keywords for lexical analysis
    public struct ValueTable
    {
        public TOKEN tok;          // Token id
        public string Value;       // Token string

        public ValueTable(TOKEN tok, string Value)
        {
            this.tok = tok;
            this.Value = Value;
        }
    }
}