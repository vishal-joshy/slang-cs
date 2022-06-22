using System.Reflection.Emit;

namespace SLANG
{
  // Variable assignment statement
  public class AssignmentStatement : Stmt
  {
    private Variable _variable;
    private Expression _expression;

    public AssignmentStatement(Variable var, Expression ex)
    {
      _variable = var;
      _expression = ex;
    }

    public AssignmentStatement(Symbol s, Expression ex)
    {
      _variable = new Variable(s);
      _expression = ex;
    }

    public Variable GetVariable() => _variable;
    public Expression GetExpression() => _expression;

    public override Symbol accept(Visitor v, RuntimeContext rtx)
    {
      return v.visit(rtx,this);
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      if(!_expression.Compile(dtx)){
        throw new Exception("Compilation string error");
      }
      Symbol info = dtx.TABLE.Get(_variable.Name);
      LocalBuilder localBuilder = dtx.GetLocalVariables(info.loc_position);
      dtx.CodeOutput.Emit(OpCodes.Stloc,localBuilder);
      return true;
    }
  }
}