namespace SLANG {
  public class RuntimeContext {
    private SymbolTable _table;

    public RuntimeContext() {
      _table = new SymbolTable();
    }

    public SymbolTable TABLE {
      get => _table;
      set => _table = value;
    }
  }

  public class CompilationContext {
    private SymbolTable _table;

    public CompilationContext() {
      _table = new SymbolTable();
    }

    public SymbolTable TABLE {
      get => _table;
      set => _table = value;
    }
  }
}