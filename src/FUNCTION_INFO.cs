using System.Collections;

namespace SLANG
{
    class FUNCTION_INFO
    {
        public TYPE_INFO ReturnValue;
        public string Name;
        public ArrayList TypeInfo;

        public FUNCTION_INFO(string name, TYPE_INFO ret_value, ArrayList formals)
        {
            ReturnValue = ret_value;
            TypeInfo = formals;
            Name = name;
        }
    }
}
