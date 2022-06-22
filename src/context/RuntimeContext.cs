namespace SLANG{
  public class RuntimeContext{
    private SymbolTable _table;
    private TModule _module=null;

    public RuntimeContext(TModule module){
      _module = module;
      _table= new SymbolTable();
    }

    public TModule GetModule() => _module;

    public SymbolTable TABLE{
      get=>_table;
      set=>_table=value;
    }
  }
}