using System.Collections;

namespace SLANG
{
    class TModuleBuilder
    {
        private ArrayList procedures;
        private ArrayList prototypes = null;

        public TModuleBuilder()
        {
            procedures = new ArrayList();
            prototypes = new ArrayList();
        }

        public bool IsFunction(string name)
        {
            foreach (FUNCTION_INFO fInfo in prototypes)
            {
                if (fInfo.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddFunctionProtoType(string name, TYPE_INFO retType, ArrayList typeInfos)
        {
            FUNCTION_INFO fn = new FUNCTION_INFO(name, retType, typeInfos);
            prototypes.Add(fn);
        }

        public bool Add(Procedure p)
        {
            procedures.Add(p);
            return true;
        }


        public TModule GetProgram()
        {
            return new TModule(procedures);
        }


        public Procedure GetProc(string name)
        {
            foreach (Procedure p in procedures)
            {
                if (p.Name.Equals(name))
                {
                    return p;
                }
            }
            return null;
        }
    }
}
