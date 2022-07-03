namespace SLANG
{
  public class COMPILATION_CONTEXT
  {
    private SymbolTable _table;

    public COMPILATION_CONTEXT()
    {
      _table = new SymbolTable();
    }

    public SymbolTable TABLE
    {
      get => _table;
      set => _table = value;
    }
  }
}