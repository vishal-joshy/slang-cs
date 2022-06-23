using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace SLANG {
  public class ExecutableGenerator {
    private AssemblyBuilder _assemblyBuilder = null;
    private ModuleBuilder _moduleBuilder = null;
    private TypeBuilder _typeBuilder = null;
    private string _executableName = "";
    private TModule _programToBeCompiled = null;

    public ExecutableGenerator(TModule program, string exeName){
      _programToBeCompiled = program;
      _executableName = exeName;

      AppDomain appDomain = Thread.GetDomain();
      AssemblyName _assemblyName = new AssemblyName();
      _assemblyName.Name = "MyAssembly";
      _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndSave);
      _moduleBuilder = _assemblyBuilder.DefineDynamicModule("DynamicModule1", _executableName, false);
      _typeBuilder = _moduleBuilder.DefineType("MainClass");
    }

    public TypeBuilder Type_Builder{
      get { return _typeBuilder; }
    }

    public void Save(){
      _typeBuilder.CreateType();
      MethodBuilder methodBuilder = _programToBeCompiled.GetEntryPoint("MAIN");

      if(methodBuilder!=null){
        _assemblyBuilder.SetEntryPoint(methodBuilder, PEFileKinds.ConsoleApplication);
      }

      _assemblyBuilder.Save(_executableName);
    }
  }
}