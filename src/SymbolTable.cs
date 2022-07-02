using System.Collections;

namespace SLANG
{
    // Variable data
    public class SYMBOL
    {
        public string Name { get; set; }
        public TYPE_INFO Type { get; set; }
        public string StringValue { get; set; }
        public double DoubleValue { get; set; }
        public bool BooleanValue { get; set; }
        public int loc_position = 0;

        public SYMBOL()
        {

        }

        public SYMBOL(string name, TYPE_INFO type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string GetValueAsString()
        {
            switch (Type)
            {
                case TYPE_INFO.NUMERIC: return DoubleValue.ToString();
                case TYPE_INFO.STRING: return StringValue;
                case TYPE_INFO.BOOL: return BooleanValue.ToString();
                default: return null;
            }
        }

    }

    public class SymbolTable
    {
        private Hashtable dataTable = new Hashtable();

        public bool Add(SYMBOL s)
        {
            dataTable[s.Name] = s;
            return true;
        }

        public SYMBOL Get(string name)
        {
            return dataTable[name] as SYMBOL;
        }

        public void Assign(string name, SYMBOL s)
        {
            dataTable[name] = s;
        }

        public void Assign(Variable var, SYMBOL s)
        {
            string variableName = var.GetName();
            s.Name = variableName;
            dataTable[variableName] = s;
        }
    }
}