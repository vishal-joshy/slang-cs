using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
  public abstract class CompilationUnit
  {
    public abstract Symbol Execute(RuntimeContext rtx);
    public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx);
  }

  public abstract class PROC
  {
    public abstract Symbol Execute(RuntimeContext rtx);
    public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx);
  }

  public class TModule : CompilationUnit
  {
    private ArrayList _procedures = null;
    private ArrayList _compiledProcedures = null;
    private ExecutableGenerator _exeGenerator = null;

    public TModule(ArrayList procedures)
    {
      _procedures = procedures;
    }

    public bool CreateExecutable(string exeName)
    {
      _exeGenerator = new ExecutableGenerator(this, exeName);
      Compile(null);
      _exeGenerator.Save();
      return true;
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      _compiledProcedures = new ArrayList();
      foreach (Procedure p in _procedures)
      {
        DNET_EXECUTABLE_GENERATION_CONTEXT context = new DNET_EXECUTABLE_GENERATION_CONTEXT(this, p, _exeGenerator.Type_Builder);
        _compiledProcedures.Add(context);
        p.Compile(context);
      }
      return true;
    }

    public override Symbol Execute(RuntimeContext rtx)
    {
      Procedure procedure = FindProcedure("Main");
      if (procedure != null)
      {
        return procedure.Execute(rtx);
      }
      return null;
    }

    public MethodBuilder GetEntryPoint(string functionName)
    {
      foreach (DNET_EXECUTABLE_GENERATION_CONTEXT dtx in _compiledProcedures)
      {
        if (dtx.MethodName.Equals(functionName))
        {
          return dtx.MethodHandle;
        }
      }
      return null;
    }

    public Procedure FindProcedure(string name)
    {
      foreach (Procedure p in _procedures)
      {
        string pName = p.Name;
        if (pName.ToUpper().CompareTo(name.ToUpper()) == 0)
        {
          return p;
        }
      }
      return null;
    }
  }

  public class Procedure:PROC{
    public string Name;
    public ArrayList Formals;
    public ArrayList Statements;
    public SymbolTable LocalVariables;
    public Symbol ReturnValue=null;
    public TYPE Type = TYPE.ILLEGAL;

    public Procedure(string name, ArrayList statements, SymbolTable table , TYPE returnType){
      Name = name;
      Statements = statements;
      LocalVariables = table;
      Type = returnType;
    }

    public override Symbol Execute(RuntimeContext rtx)
    {
      Interpreter i = new Interpreter();
      foreach(Stmt s in Statements){
        s.accept(i, rtx);
      }
      return null;
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      foreach(Stmt s in Statements){
        s.Compile(dtx);
      }
      dtx.CodeOutput.Emit(OpCodes.Ret);
      return true;
    }

  }



}