using System.Collections;

namespace SLANG {
  public interface Visitor {
    Symbol visit(RuntimeContext rtx, PrintStatement ps);
    Symbol visit(RuntimeContext rtx, PrintLineStatement pl);
    Symbol visit(RuntimeContext rtx, AssignmentStatement astmt);
    Symbol visit(RuntimeContext rtx, VariableDeclarationStatement vds);
    Symbol visit(RuntimeContext rtx, NumericConstant n);
    Symbol visit(RuntimeContext rtx, BooleanConstant b);
    Symbol visit(RuntimeContext rtx, StringLiteral s);
    Symbol visit(RuntimeContext rtx, BinaryExpression bp);
    Symbol visit(RuntimeContext rtx, UnaryPlus up);
    Symbol visit(RuntimeContext rtx, UnaryMinus um);
    Symbol visit(RuntimeContext rtx, Variable v);
    Symbol visit(RuntimeContext rtx, RelationalExpression r);
    Symbol visit(RuntimeContext rtx, LogicalExpression l);
    Symbol visit(RuntimeContext rtx, LogicalNot ln);
    Symbol visit(RuntimeContext rtx, IfStatement ifs);
    Symbol visit(RuntimeContext rtx, WhileStatment ws);
    Symbol visit(RuntimeContext rtx, ReturnStatement rs);
    Symbol visit(RuntimeContext rtx, CallProcedureExpression cpe);
  }

  public class Interpreter : Visitor {
    public Symbol visit(RuntimeContext rtx, PrintStatement ps) {
      Expression exp = ps.GetExpression();
      Symbol result = exp.accept(rtx, this);
      Console.Write(result.GetValueAsString());
      return null;
    }

    public Symbol visit(RuntimeContext rtx, PrintLineStatement pls) {
      Expression exp = pls.GetExpression();
      Symbol result = exp.accept(rtx, this);
      Console.WriteLine(result.GetValueAsString());
      return null;
    }

    public Symbol visit(RuntimeContext rtx, AssignmentStatement astmt) {
      Variable var = astmt.GetVariable();
      Expression exp = astmt.GetExpression();
      Symbol result = exp.accept(rtx, this);
      rtx.TABLE.Assign(var, result);
      return null;
    }

    public Symbol visit(RuntimeContext rtx, VariableDeclarationStatement vds) {
      Symbol s = vds.GetInfo();
      rtx.TABLE.Add(s);
      vds.Var = new Variable(s);
      return null;
    }
    public Symbol visit(RuntimeContext rtx, NumericConstant n) {
      return n.GetSymbol();
    }
    public Symbol visit(RuntimeContext rtx, BooleanConstant b) {
      return b.GetSymbol();
    }
    public Symbol visit(RuntimeContext rtx, StringLiteral s) {
      return s.GetSymbol();
    }

    public Symbol visit(RuntimeContext rtx, BinaryExpression bp) {
      Symbol lEval = bp.GetLExpression().accept(rtx, this);
      Symbol rEval = bp.GetRExpression().accept(rtx, this);
      OPERATOR op = bp.GetOperator();

      if (lEval.Type != rEval.Type) {
        throw new Exception("Type mismatch for binary operation");
      }

      if (lEval.Type == TYPE.NUMERIC) {
        Symbol result = new Symbol();
        result.Type= TYPE.NUMERIC;
        result.Name = "";
        switch (op) {
          case OPERATOR.PLUS: result.DoubleValue = lEval.DoubleValue + rEval.DoubleValue; return result;
          case OPERATOR.MINUS: result.DoubleValue = lEval.DoubleValue - rEval.DoubleValue; return result;
          case OPERATOR.MULT: result.DoubleValue = lEval.DoubleValue * rEval.DoubleValue; return result;
          case OPERATOR.DIV: result.DoubleValue = lEval.DoubleValue / rEval.DoubleValue; return result;
          default: throw new Exception("Invalid operator");
        }
      } else if (lEval.Type == TYPE.STRING) {
        Symbol result = new Symbol();
        result.Type = TYPE.STRING;
        result.StringValue = lEval.StringValue + rEval.StringValue;
        result.Name = "";
        return result;
      } else {
        throw new Exception("Invalid types for binary plus");
      }
    }

    public Symbol visit(RuntimeContext rtx, Variable v) {
      if (rtx.TABLE == null) {
        return null;
      } else {
        Symbol result = rtx.TABLE.Get(v.GetName());
        return result;
      }
    }

    public Symbol visit(RuntimeContext rtx, UnaryPlus up) {
      Symbol eval = up.GetExpression().accept(rtx, this);
      if (eval.Type == TYPE.NUMERIC) {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = eval.DoubleValue;
        result.Name = "";
        return result;
      } else {
        throw new Exception("Invalid type for unary plus");
      }
    }
    public Symbol visit(RuntimeContext rtx, UnaryMinus um) {
      Symbol eval = um.GetExpression().accept(rtx, this);
      if (eval.Type == TYPE.NUMERIC) {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = eval.DoubleValue;
        result.Name = "";
        return result;
      } else {
        throw new Exception("Invalid type for unary minus");
      }
    }

    public Symbol visit(RuntimeContext rtx, RelationalExpression r) {
      Symbol lEval = r.GetLExpression().accept(rtx, this);
      Symbol rEval = r.GetRExpression().accept(rtx, this);
      RELATIONAL_OPERATOR op = r.GetOperator();
      Symbol result = new Symbol();
      result.Name = "";
      result.Type = TYPE.BOOL;

      if (lEval.Type != rEval.Type) {
        throw new Exception("Invalid types for relational operations");
      }

      if (lEval.Type == TYPE.NUMERIC) {
        if (op == RELATIONAL_OPERATOR.EQUALITY)
          result.BooleanValue = lEval.DoubleValue == rEval.DoubleValue;
        else if (op == RELATIONAL_OPERATOR.NOTEQUALITY)
          result.BooleanValue = lEval.DoubleValue != rEval.DoubleValue;
        else if (op == RELATIONAL_OPERATOR.GREATER_THAN)
          result.BooleanValue = lEval.DoubleValue > rEval.DoubleValue;
        else if (op == RELATIONAL_OPERATOR.LESS_THAN)
          result.BooleanValue = lEval.DoubleValue < rEval.DoubleValue;
        else if (op == RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY)
          result.BooleanValue = lEval.DoubleValue >= rEval.DoubleValue;
        else if (op == RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY)
          result.BooleanValue = lEval.DoubleValue <= rEval.DoubleValue;
        else
          throw new Exception("Invalid Relational Operator");

        return result;

      } else if (lEval.Type == TYPE.STRING) {
        if (op == RELATIONAL_OPERATOR.EQUALITY)
          result.BooleanValue = String.Compare(lEval.StringValue, rEval.StringValue) == 0;
        else if (op == RELATIONAL_OPERATOR.NOTEQUALITY)
          result.BooleanValue = String.Compare(lEval.StringValue, rEval.StringValue) != 0;
        else
          result.BooleanValue = false;
        return result;

      } else if (lEval.Type == TYPE.BOOL) {
        if (op == RELATIONAL_OPERATOR.EQUALITY)
          result.BooleanValue = lEval.BooleanValue == rEval.BooleanValue;
        else if (op == RELATIONAL_OPERATOR.NOTEQUALITY)
          result.BooleanValue = lEval.BooleanValue != rEval.BooleanValue;
        else
          result.BooleanValue = false;
        return result;
      }
      return null;
    }

    public Symbol visit(RuntimeContext rtx, LogicalExpression l) {
      Symbol lEval = l.GetLExpression().accept(rtx, this);
      Symbol rEval = l.GetRExpression().accept(rtx, this);
      TOKEN op = l.GetOperator();

      if (lEval.Type == TYPE.BOOL && rEval.Type == TYPE.BOOL) {
        Symbol result = new Symbol();
        result.Name = "";
        result.Type = TYPE.BOOL;
        if (op == TOKEN.AND)
          result.BooleanValue = lEval.BooleanValue && rEval.BooleanValue;
        else if (op == TOKEN.OR)
          result.BooleanValue = lEval.BooleanValue || rEval.BooleanValue;
        else
          return null;

        return result;
      }
      return null;
    }

    public Symbol visit(RuntimeContext rtx, LogicalNot ln) {
      Symbol eval = ln.GetExpression().accept(rtx, this);

      if (eval.Type == TYPE.BOOL) {
        Symbol result = new Symbol();
        result.Type = TYPE.BOOL;
        result.Name = "";
        result.BooleanValue = !eval.BooleanValue;
        return result;
      } else {
        return null;
      }
    }

    public Symbol visit(RuntimeContext rtx, IfStatement ifs) {
      Symbol eval = ifs.GetCondition().accept(rtx, this);
      if (eval.Type != TYPE.BOOL) {
        throw new Exception("Condition evaluation failed");
      }

      if (eval.BooleanValue == true) {
        ArrayList statements = ifs.GetStatements("trueStatements");
        foreach (Stmt s in statements) {
          s.accept(rtx, this);
        }
      } else {
        ArrayList statements = ifs.GetStatements("falseStatements");
        if (statements != null) {
          foreach (Stmt s in statements) {
            s.accept(rtx, this);
          }
        }
      }
      return null;
    }

    public Symbol visit(RuntimeContext rtx, WhileStatment ws) {
    Wloop:
      Symbol condition = ws.GetCondition().accept(rtx, this);
      ArrayList statements = ws.GetStatements();
      if (condition == null || condition.Type != TYPE.BOOL) {
        throw new Exception("Condition evaluation error");
      }
      if (condition.BooleanValue != true)
        return null;

      if (condition.BooleanValue == true) {
        Symbol tsp = null;
        foreach (Stmt s in statements) {
          tsp = s.accept(rtx, this);
          if (tsp != null) {
            return tsp;
          }
        }
      }
      goto Wloop;
    }

    public Symbol visit(RuntimeContext rtx, ReturnStatement rs) {
      Expression exp = rs.GetExpression();
      rs.inf = exp == null ? null : exp.accept(rtx, this);
      return rs.inf;
    }

    public Symbol visit(RuntimeContext rtx, CallProcedureExpression cpe) {
      Procedure procedure = cpe.GetProcedure();
      ArrayList actuals = cpe.GetActuals();
      if (procedure != null) {
        RuntimeContext cont = new RuntimeContext(rtx.GetModule());
        ArrayList lst = new ArrayList();

        foreach (Expression e in actuals) {
          lst.Add(e.accept(rtx, this));
        }
        return procedure.Execute(cont, lst);
      } else {
        procedure = rtx.GetModule().FindProcedure(cpe.GetProcedureName());
        RuntimeContext cont = new RuntimeContext(rtx.GetModule());
        ArrayList lst = new ArrayList();

        foreach (Expression e in actuals) {
          lst.Add(e.accept(rtx, this));
        }
        return procedure.Execute(cont, lst);
      }
    }
  }
}