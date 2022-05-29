using System;

namespace SLANG{
  public class ExpressionBuilder{
    private string _expressionString;

    public ExpressionBuilder(string s){
      _expressionString = s;
    }

    public Expression GetExpression(){
      try{
        RDParser parser = new RDParser(_expressionString);
        return parser.CallExpr();
      }
      catch(Exception e){
        Console.WriteLine(e.Message);
        return null;
      }
    }

  }
}