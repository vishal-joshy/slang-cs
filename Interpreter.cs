namespace SLANG
{
  public interface Visitor
  {
    Symbol visit(RuntimeContext rtx, PrintStatement ps);
    Symbol visit(RuntimeContext rtx, PrintLineStatement pl);
    Symbol visit(RuntimeContext rtx, AssignmentStatement astmt);
    Symbol visit(RuntimeContext rtx, VariableDeclarationStatement vds);
    Symbol visit(RuntimeContext rtx, NumericConstant n);
    Symbol visit(RuntimeContext rtx, BooleanConstant b);
    Symbol visit(RuntimeContext rtx, StringLiteral s);
    Symbol visit(RuntimeContext rtx, BinaryPlus bp);
    Symbol visit(RuntimeContext rtx, BinaryMinus bm);
    Symbol visit(RuntimeContext rtx, BinaryMultiplication bm);
    Symbol visit(RuntimeContext rtx, BinaryDivision bd);
    Symbol visit(RuntimeContext rtx, UnaryPlus up);
    Symbol visit(RuntimeContext rtx, UnaryMinus um);
    Symbol visit(RuntimeContext rtx, Variable v);
  }

  public class Interpreter : Visitor
  {
    public Symbol visit(RuntimeContext rtx, PrintStatement ps)
    {
      Expression exp = ps.GetExpression();
      Symbol result = exp.accept(rtx, this);
      Console.Write(result.GetValueAsString());
      return null;
    }

    public Symbol visit(RuntimeContext rtx, PrintLineStatement pls)
    {
      Expression exp = pls.GetExpression();
      Symbol result = exp.accept(rtx, this);
      Console.WriteLine(result.GetValueAsString());
      return null;
    }

    public Symbol visit(RuntimeContext rtx, AssignmentStatement astmt)
    {
      Variable var = astmt.GetVariable();
      Expression exp = astmt.GetExpression();
      Symbol result = exp.accept(rtx, this);
      rtx.TABLE.Assign(var, result);
      return null;
    }

    public Symbol visit(RuntimeContext rtx, VariableDeclarationStatement vds)
    {
      Symbol s = vds.GetInfo();
      rtx.TABLE.Add(s);
      vds.Var = new Variable(s);
      return null;
    }
    public Symbol visit(RuntimeContext rtx, NumericConstant n)
    {
      return n.GetSymbol();
    }
    public Symbol visit(RuntimeContext rtx, BooleanConstant b)
    {
      return b.GetSymbol();
    }
    public Symbol visit(RuntimeContext rtx, StringLiteral s)
    {
      return s.GetSymbol();
    }

    public Symbol visit(RuntimeContext rtx, BinaryPlus bp)
    {
      Symbol lEval = bp.GetLExpression().accept(rtx, this);
      Symbol rEval = bp.GetRExpression().accept(rtx, this);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue + rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else if (lEval.Type == TYPE.STRING && rEval.Type == TYPE.STRING)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.STRING;
        result.StringValue = lEval.StringValue + rEval.StringValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary plus");
      }
    }

    public Symbol visit(RuntimeContext rtx, BinaryMinus bm)
    {
      Symbol lEval = bm.GetLExpression().accept(rtx, this);
      Symbol rEval = bm.GetRExpression().accept(rtx, this);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue - rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary minus");
      }
    }

    public Symbol visit(RuntimeContext rtx, BinaryMultiplication bm)
    {
      Symbol lEval = bm.GetLExpression().accept(rtx, this);
      Symbol rEval = bm.GetRExpression().accept(rtx, this);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue * rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary multiplication");
      }
    }

    public Symbol visit(RuntimeContext rtx, BinaryDivision bd)
    {
      Symbol lEval = bd.GetLExpression().accept(rtx, this);
      Symbol rEval = bd.GetRExpression().accept(rtx, this);

      if (lEval.Type == TYPE.NUMERIC && rEval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = lEval.DoubleValue / rEval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid types for binary division");
      }
    }

    public Symbol visit(RuntimeContext rtx, Variable v)
    {
      if (rtx.TABLE == null)
      {
        return null;
      }
      else
      {
        Symbol result = rtx.TABLE.Get(v.GetName());
        return result;
      }
    }

    public Symbol visit(RuntimeContext rtx, UnaryPlus up)
    {
      Symbol eval = up.GetExpression().accept(rtx, this);
      if (eval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = eval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid type for unary plus");
      }
    }
    public Symbol visit(RuntimeContext rtx, UnaryMinus um)
    {
      Symbol eval = um.GetExpression().accept(rtx, this);
      if (eval.Type == TYPE.NUMERIC)
      {
        Symbol result = new Symbol();
        result.Type = TYPE.NUMERIC;
        result.DoubleValue = eval.DoubleValue;
        result.Name = "";
        return result;
      }
      else
      {
        throw new Exception("Invalid type for unary minus");
      }
    }
  }
}