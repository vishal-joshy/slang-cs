using System.Collections;

namespace SLANG{
  public class TModuleBuilder:AbstractBuilder{
    private ArrayList _procedures;
    private ArrayList _prototypes = null;

    public TModuleBuilder(){
      _procedures = new ArrayList();
      _prototypes = null;
    }

    public bool AddProcedure (Procedure p ){
      _procedures.Add(p);
      return true;
    }

    public TModule GetProgram(){
      return new TModule(_procedures);
    }

    public Procedure GetProcedure(string procedureName){
      foreach(Procedure p in _procedures){
        if(p.Name.Equals(procedureName)){
          return p;
        }
      }
      return null;
    }
  }
}