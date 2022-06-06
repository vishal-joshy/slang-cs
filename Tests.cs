using Xunit;
using SLANG;

public class AbstractSynatxTreeTest
{
  [Fact]
  public void NumericConstantTest()
  {
    Expression e1 = new NumericConstant(5);
    Assert.Equal(5, e1.Evaluate(null));
  }

  [Theory]
  [InlineData(1, OPERATOR.PLUS, 3, 4)]
  [InlineData(4, OPERATOR.MINUS, 2, 2)]
  [InlineData(3, OPERATOR.MULT, 2, 6)]
  [InlineData(6, OPERATOR.DIV, 2, 3)]
  public void BinaryExpressionTest(int a, OPERATOR op, int b, int result)
  {
    Expression e1 = new NumericConstant(a);
    Expression e2 = new NumericConstant(b);

    Expression binaryExp = new BinaryExpression(e1, op, e2);

    Assert.Equal(binaryExp.Evaluate(null), result);
  }

  [Theory]
  [InlineData(1, OPERATOR.MINUS, -1)]
  [InlineData(1, OPERATOR.PLUS, 1)]
  public void UnaryExpressionTest(int a, OPERATOR op, int result)
  {
    Expression e1 = new NumericConstant(a);

    Expression unaryExp = new UnaryExpression(op, e1);

    Assert.Equal(result, unaryExp.Evaluate(null));
  }

  // AST test for 3(5+6)
  [Fact]
  public void BinaryExpressionTest2()
  {
    Expression e1 = new NumericConstant(3);
    Expression e2 = new NumericConstant(5);
    Expression e3 = new NumericConstant(6);
    Expression binaryExp = new BinaryExpression(e1, OPERATOR.MULT, new BinaryExpression(e2, OPERATOR.PLUS, e3));
    Assert.Equal(33, binaryExp.Evaluate(null));
  }
}


public class LexicalAnalyzerTest
{
  [Theory]
  [InlineData("6", TOKEN.DOUBLE)]
  [InlineData("+", TOKEN.PLUS)]
  [InlineData("(", TOKEN.OPAREN)]
  void GetTokenTest(string s, TOKEN expected)
  {
    LexicalAnalyzer la = new LexicalAnalyzer(s);
    Assert.Equal(expected, la.GetToken());
  }

  [Theory]
  [InlineData("2", 2)]
  [InlineData("+", 0)]
  [InlineData("364", 364)]
  void GetNumberTest(string s, double expected)
  {
    LexicalAnalyzer la = new LexicalAnalyzer(s);
    la.GetToken();
    Assert.Equal(expected, la.GetNumber());
  }
}
public class RDParserTest
{
  [Theory]
  [InlineData("2+3",5)]
  [InlineData("2+(3*4)", 14)]
  [InlineData("2+3*4-5", 9)]
  void RDParserTest1(string expressionString, int expected)
  {
    RDParser parser = new RDParser(expressionString);
    Assert.Equal(expected, parser.CallExpr().Evaluate(null));
  }
}

public class StatementTest{
  [Fact]
  public void PrintStatementTest()
  {
    PrintStatement ps = new PrintStatement(new NumericConstant(5));
    Assert.Equal(true, ps.Execute(null));
  }
  [Fact]
  public void PrintLineStatementTest()
  {
    PrintLineStatement pst = new PrintLineStatement(new NumericConstant(5));
    Assert.Equal(true, pst.Execute(null));
  }
}