using System.Collections;

namespace SLANG
{
  // Varibale data
  public class Symbol
  {
    public string Name;
    public TYPE Type;
    public string StringValue;
    public double DoubleValue;
    public bool BooleanValue;

    public void Dump()
    {
      Console.WriteLine("Symbol\nName: " + Name);
      Console.WriteLine("Type: " + Type);
      Console.WriteLine("StringValue: " + StringValue);
      Console.WriteLine("DoubleValue: " + DoubleValue);
      Console.WriteLine("BooleanValue: " + BooleanValue);
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