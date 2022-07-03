namespace SLANG
{
  public class RUNTIME_CONTEXT
  {
    private SymbolTable _table;
    private TModule _program = null;

    public RUNTIME_CONTEXT(TModule p)
    {
      _table = new SymbolTable();
      _program = p;
    }

    public TModule GetProgram() => _program;

    public SymbolTable TABLE
    {
      get => _table;
      set => _table = value;
    }
  }
}