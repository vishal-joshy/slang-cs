using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace SLANG {
  public class DNET_EXECUTABLE_GENERATION_CONTEXT {
    private ArrayList _localVariables = new ArrayList();
    private ILGenerator _ILOut;
    private SymbolTable _sTable = new SymbolTable();
    private MethodBuilder _methodBuilder = null;
    private TypeBuilder _typeBuilder = null;
    private Procedure _procedure = null;
    private TModule _program;

    public DNET_EXECUTABLE_GENERATION_CONTEXT(TModule program , Procedure procedure, TypeBuilder typeBuilder){
      _program = program;
      _procedure = procedure;
      _typeBuilder = typeBuilder;

      System.Type[] s = null;
      System.Type returnType = null;
      _methodBuilder = _typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, returnType, s);
      _ILOut = _methodBuilder.GetILGenerator();
    }

    public string MethodName {
      get=> _procedure.Name;
  }
    public MethodBuilder MethodHandle {
      get => _methodBuilder;
    }
    public TypeBuilder GetTypeBuilder() => _typeBuilder;

    public SymbolTable TABLE {
     get => _sTable;
    }

    public ILGenerator CodeOutput{
      get => _ILOut;
    }

    public int DeclareLocal(System.Type type){
      LocalBuilder localBuilder = _ILOut.DeclareLocal(type);
      return _localVariables.Add(localBuilder);
    }

    public LocalBuilder GetLocalVariables(int s){
      return _localVariables[s] as LocalBuilder;
    }
  }
}