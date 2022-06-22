using System.Reflection.Emit;
namespace SLANG
{
  public class PrintStatement : Stmt
  {
    private Expression _expression;

    public PrintStatement(Expression ex)
    {
      _expression = ex;
    }

    public override Symbol accept(Visitor v, RuntimeContext rtx)
    {
      return v.visit(rtx,this);
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      _expression.Compile(dtx);
      System.Type type = Type.GetType("System.Console");
      Type[] parameters = new Type[1];
      TYPE tData = _expression.Get_Type();

      if(tData == TYPE.NUMERIC){
        parameters[0] = typeof(double);
      }else if(tData == TYPE.STRING){
        parameters[0] = typeof(string);
      } else if(tData == TYPE.BOOL){
        parameters[0] = typeof(bool);
      }else{
        throw new Exception("Invalid TYPE");
      }

      dtx.CodeOutput.Emit(OpCodes.Call, type.GetMethod("Write", parameters));
      return true;
    }


    public Expression GetExpression() => _expression;
  }


  public class PrintLineStatement : Stmt
  {
    private Expression _expression;

    public PrintLineStatement(Expression ex)
    {
      _expression = ex;
    }

    public override Symbol accept(Visitor v, RuntimeContext rtx)
    {
      return v.visit(rtx, this);
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      _expression.Compile(dtx);
      System.Type type = Type.GetType("System.Console");
      Type[] parameters = new Type[1];
      TYPE tData = _expression.Get_Type();

      if(tData == TYPE.NUMERIC){
        parameters[0] = typeof(double);
      }else if(tData == TYPE.STRING){
        parameters[0] = typeof(string);
      } else if(tData == TYPE.BOOL){
        parameters[0] = typeof(bool);
      } else{
        throw new Exception("Invalid TYPE");
      }
      dtx.CodeOutput.Emit(OpCodes.Call, type.GetMethod("WriteLine", parameters));
      return true;
    }

    public Expression GetExpression() => _expression;
  }
}