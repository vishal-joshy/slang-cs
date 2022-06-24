using System.Reflection.Emit;
namespace SLANG {
  public class RelationalExpression : Expression {
    private RELATIONAL_OPERATOR _operator;
    private Expression _lExpression,_rExpression;
    private TYPE _type;
    private TYPE _opType;

    public RelationalExpression(Expression e1, RELATIONAL_OPERATOR op, Expression e2){
      _lExpression = e1;
      _rExpression = e2;
      _operator = op;
    }

    public Expression GetLExpression() => _lExpression;
    public Expression GetRExpression() => _rExpression;
    public RELATIONAL_OPERATOR GetOperator() => _operator;

    public override TYPE Get_Type() => _type;

    public override TYPE TypeCheck(CompilationContext cont)
    {
      TYPE lType = _lExpression.TypeCheck(cont);
      TYPE rType = _rExpression.TypeCheck(cont);

      if(lType != rType){
        throw new Exception("Wrong Types in expression");
      }

      if(lType == TYPE.STRING && (!(_operator == RELATIONAL_OPERATOR.EQUALITY || _operator == RELATIONAL_OPERATOR.NOTEQUALITY)))
        throw new Exception("Only == and != supported for strings");
      if(lType == TYPE.BOOL && (!(_operator == RELATIONAL_OPERATOR.EQUALITY || _operator == RELATIONAL_OPERATOR.NOTEQUALITY)))
        throw new Exception("Only == and != supported for boolean values");

      _opType = lType;
      _type = TYPE.BOOL;
      return _type;
    }

    public override Symbol accept(RuntimeContext cont, Visitor v)
    {
      return v.visit(cont,this);
    }

    public bool CompileStringRelationalOperator(DNET_EXECUTABLE_GENERATION_CONTEXT dtx){
      _lExpression.Compile(dtx);
      _rExpression.Compile(dtx);

      Type[] str2 = { typeof(string), typeof(string) };

      dtx.CodeOutput.Emit(OpCodes.Call, typeof(string).GetMethod("Compare", str2));

      if(_operator == RELATIONAL_OPERATOR.EQUALITY){
        dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
        dtx.CodeOutput.Emit(OpCodes.Ceq);
      } else{
        dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
        dtx.CodeOutput.Emit(OpCodes.Ceq);
        dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
        dtx.CodeOutput.Emit(OpCodes.Ceq);
      }
      return true;
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx)
    {
      if(_opType== TYPE.STRING){
        return CompileStringRelationalOperator(dtx);
      }

      _lExpression.Compile(dtx);
      _rExpression.Compile(dtx);

      if(_operator == RELATIONAL_OPERATOR.EQUALITY)
        dtx.CodeOutput.Emit(OpCodes.Ceq);
      else if (_operator == RELATIONAL_OPERATOR.GREATER_THAN)
        dtx.CodeOutput.Emit(OpCodes.Cgt);
      else if (_operator == RELATIONAL_OPERATOR.LESS_THAN)
        dtx.CodeOutput.Emit(OpCodes.Clt);
      else if (_operator == RELATIONAL_OPERATOR.NOTEQUALITY)
      {
        dtx.CodeOutput.Emit(OpCodes.Ceq);
        dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
        dtx.CodeOutput.Emit(OpCodes.Ceq);
      } else if(_operator == RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY){
        dtx.CodeOutput.Emit(OpCodes.Clt);
        dtx.CodeOutput.Emit(OpCodes.Ldc_I4,0);
        dtx.CodeOutput.Emit(OpCodes.Ceq);
      } else if (_operator == RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY){
        dtx.CodeOutput.Emit(OpCodes.Cgt);
        dtx.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
        dtx.CodeOutput.Emit(OpCodes.Ceq);
      } else {
        throw new Exception("Invalid Operator for RelationalExpression");
      }
      return true;


    }

  }
}