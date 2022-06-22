using System.Reflection.Emit;

namespace SLANG
{
  // Variable node , stores variable info
  public class Variable : Expression
  {
    private string _name;
    public TYPE _type;

    public Variable(Symbol info)
    {
      _name = info.Name;
    }

    public Variable(CompilationContext ct, string name, double val)
    {
      Symbol s = new Symbol();
      s.Name = name;
      s.DoubleValue = val;
      s.Type = TYPE.NUMERIC;
      _name = name;
    }

    public Variable(CompilationContext ct, string name, string val)
    {
      Symbol s = new Symbol();
      s.Name = name;
      s.StringValue = val;
      s.Type = TYPE.STRING;
      _name = name;
    }

    public Variable(CompilationContext ct, string name, bool val)
    {
      Symbol s = new Symbol();
      s.Name = name;
      s.BooleanValue = val;
      s.Type = TYPE.BOOL;
      _name = name;
    }

    public string GetName() => _name;
    public override TYPE Get_Type() => _type;

    public string Name
    {
      get => _name;
      set => _name = value;
    }

    public override Symbol accept(RuntimeContext cont,Visitor v)
    {
      return v.visit(cont,this);
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      Symbol info = dtx.TABLE.Get(_name);
      LocalBuilder localBuilder = dtx.GetLocalVariables(info.loc_position);
      dtx.CodeOutput.Emit(OpCodes.Ldloc, localBuilder);
      return true;
    }


    public override TYPE TypeCheck(CompilationContext cont)
    {
      if (cont.TABLE == null)
      {
        return TYPE.ILLEGAL;
      }
      else
      {
        Symbol result = cont.TABLE.Get(_name);
        if (result != null)
        {
          _type = result.Type;
          return result.Type;
        }
        return TYPE.ILLEGAL;
      }
    }
  }
}