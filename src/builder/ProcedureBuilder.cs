using System.Collections;

namespace SLANG{
  public class ProcedureBuilder : AbstractBuilder{
    private string _procedureName = "";
    private CompilationContext _ctx = null;
    private ArrayList _formals = null;
    private ArrayList _statements = new ArrayList();
    private TYPE _info = TYPE.ILLEGAL;

    public ProcedureBuilder(string name, CompilationContext ctx){
      _procedureName = name;
      _ctx = ctx;
    }

    public bool AddLocal(Symbol s){
      _ctx.TABLE.Add(s);
      return true;
    }

    public TYPE TypeCheck(Expression e){
      return e.TypeCheck(_ctx);
    }

    public void AddStatement(Stmt statement){
      _statements.Add(statement);
    }

    public Symbol GetSymbol(string name){
      return _ctx.TABLE.Get(name);
    }

    public bool CheckPrototype(string name){
      return true;
    }

    public TYPE TYPE{
      get{
        return _info;
      }
      set{
        _info = value;
      }
    }

    public SymbolTable TABLE{
      get{
        return _ctx.TABLE;
      }
    }

    public CompilationContext Context{
      get{
        return _ctx;
      }
    }

    public string Name{
      get{
        return _procedureName;
      }
      set{
        _procedureName = value;
      }
    }

    public Procedure GetProcedure(){
      Procedure ret = new Procedure(_procedureName, _statements, _ctx.TABLE, _info);
      return ret;
    }
  }
}