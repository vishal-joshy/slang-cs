using System.Collections;

namespace SLANG
{
  // Varibale data
  public class Symbol
  {
    public string Name { get; set; }
    public TYPE Type { get; set; }
    public string StringValue { get; set; }
    public double DoubleValue { get; set; }
    public bool BooleanValue { get; set; }

    public string GetValueAsString()
    {
      switch (Type)
      {
        case TYPE.NUMERIC: return DoubleValue.ToString();
        case TYPE.STRING: return StringValue;
        case TYPE.BOOL: return BooleanValue.ToString();
        default: return null;
      }
    }
  }

  public class SymbolTable
  {
    private Hashtable dataTable = new Hashtable();

    public bool Add(Symbol s)
    {
      dataTable[s.Name] = s;
      return true;
    }

    public Symbol Get(string name)
    {
      return dataTable[name] as Symbol;
    }

    public void Assign(string name, Symbol s)
    {
      dataTable[name] = s;
    }

    public void Assign(Variable var, Symbol s)
    {
      string variableName = var.GetName();
      s.Name = variableName;
      dataTable[variableName] = s;
    }
  }
}