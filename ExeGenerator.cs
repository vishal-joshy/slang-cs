using System.Reflection;
using System.Reflection.Emit;

namespace SLANG
{
    public class ExeGenerator
    {
        private AssemblyBuilder _asm_builder = null;
        private ModuleBuilder _moduleBuilder = null;
        private TypeBuilder _typeBuilder = null;
        private string _exeName = "";
        private TModule _module = null;

        public ExeGenerator(TModule module, string name)
        {
            _module = module;
            AppDomain _app_domain = Thread.GetDomain();
            AssemblyName _asm_name = new AssemblyName();
            _asm_name.Name = "MyAssembly";
            _exeName = name;
            _asm_builder = AppDomain.CurrentDomain.DefineDynamicAssembly(_asm_name, AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _asm_builder.DefineDynamicModule("DynamicModule1", _exeName, false);
            _typeBuilder = _moduleBuilder.DefineType("MainClass");
        }

        public TypeBuilder type_builder
        {
            get
            {
                return _typeBuilder;
            }
        }

        public void Save()
        {
            _typeBuilder.CreateType();
            MethodBuilder mb = _module.GetEntryPoint("MAIN");
            if (mb != null)
            {
                _asm_builder.SetEntryPoint(mb, PEFileKinds.ConsoleApplication);
            }
            _asm_builder.Save(_exeName);
        }
    }
}
