using System.Reflection.Emit;

namespace SLANG {
  public class BinaryExpression : Expression {
    private Expression _exp1, _exp2;
    private TYPE _type;
    private OPERATOR _operator;

    public BinaryExpression(Expression e1, Expression e2, OPERATOR op) {
      _exp1 = e1;
      _exp2 = e2;
      _operator = op;
    }

    public override Symbol accept(RuntimeContext cont, Visitor v) {
      return v.visit(cont, this);
    }

    public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx) {
      _exp1.Compile(dtx);
      _exp2.Compile(dtx);

      if (_type == TYPE.NUMERIC) {
        switch (_operator) {
          case OPERATOR.PLUS: dtx.CodeOutput.Emit(OpCodes.Add); return true;
          case OPERATOR.MINUS: dtx.CodeOutput.Emit(OpCodes.Sub); return true;
          case OPERATOR.MULT: dtx.CodeOutput.Emit(OpCodes.Mul); return true;
          case OPERATOR.DIV: dtx.CodeOutput.Emit(OpCodes.Div); return true;
          default: throw new Exception("Invalid Operator");
        }
      } else if (_type == TYPE.STRING) {
        Type[] str2 = { typeof(string), typeof(string) };
        dtx.CodeOutput.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", str2));
        return true;
      } else {
        throw new Exception("Invalid types for binary operation");
      }
    }

    public override TYPE TypeCheck(CompilationContext cont) {
      TYPE lEval = _exp1.TypeCheck(cont);
      TYPE rEval = _exp2.TypeCheck(cont);

      if (lEval == rEval && lEval != TYPE.BOOL) {
        _type = lEval;
        return _type;
      } else {
        throw new Exception("Invalid types for binary plus");
      }
    }

    public Expression GetLExpression() => _exp1;
    public Expression GetRExpression() => _exp2;
    public OPERATOR GetOperator() => _operator;
    public override TYPE Get_Type() => _type;

  }
}