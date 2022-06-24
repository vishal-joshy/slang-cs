using System.Reflection.Emit;

namespace SLANG{
  public class LogicalExpression : Expression{
    private Expression _lExpression, _rExpression;
    private TOKEN _operator;
    private TYPE _type;

    public LogicalExpression(Expression e1, TOKEN op, Expression e2){
      _lExpression = e1;
      _rExpression = e2;
      _operator = op;
    }

    public Expression GetLExpression() => _lExpression;
    public Expression GetRExpression() => _rExpression;
    public override TYPE Get_Type() => _type;
    public TOKEN GetOperator() => _operator;

    public override Symbol accept(RuntimeContext cont, Visitor v)
    {
      return v.visit(cont,this);
    }

    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE lEval = _lExpression.TypeCheck(cont);
      TYPE rEval = _rExpression.TypeCheck(cont);

      if(lEval == rEval && lEval == TYPE.BOOL){
        _type = TYPE.BOOL;
        return _type;
      }else{
        throw new Exception("Wrong Type");
      }
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      _lExpression.Compile(dtx);
      _rExpression.Compile(dtx);

      if(_operator == TOKEN.AND)
        dtx.CodeOutput.Emit(OpCodes.And);
      else if (_operator == TOKEN.OR)
        dtx.CodeOutput.Emit(OpCodes.Or);

      return true;
    }
  }

  public class LogicalNot : Expression{
    private Expression _expression;
    private TYPE _type;

    public LogicalNot(Expression e){
      _expression = e;
    }

    public override TYPE Get_Type()=> _type;
    public Expression GetExpression() => _expression;

    public override Symbol accept(RuntimeContext cont, Visitor v)
    {
      return v.visit(cont, this);
    }

    public override TYPE TypeCheck(CompilationContext cont){
      TYPE eval = _expression.TypeCheck(cont);

      if(eval == TYPE.BOOL){
        _type = TYPE.BOOL;
        return _type;
      }else{
        throw new Exception("Invalid Type");
      }
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      _expression.Compile(dtx);
      dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
      dtx.CodeOutput.Emit(OpCodes.Ceq);
      dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
      dtx.CodeOutput.Emit(OpCodes.Ceq);
      return true;
    }


  }
}