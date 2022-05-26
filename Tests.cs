using Xunit;
using SLANG;

public class TestClass
{
  [Theory]
  [InlineData(1, Operator.PLUS, 3, 4)]
  [InlineData(4, Operator.MINUS, 2, 2)]
  [InlineData(3, Operator.MULT, 2, 6)]
  [InlineData(6, Operator.DIV, 2, 3)]
  public void BinaryConstatTest(int a, Operator op, int b, int result)
  {
    Expression e1 = new NumericConstant(a);
    Expression e2 = new NumericConstant(b);

    Expression binaryExp = new BinaryConstant(e1, op, e2);

    Assert.Equal(binaryExp.Evaluate(null), result);
  }

  [Theory]
  [InlineData(1, Operator.MINUS, -1)]
  [InlineData(1, Operator.PLUS, 1)]
  public void UnaryConstantTest(int a, Operator op, int result)
  {
    Expression e1 = new NumericConstant(a);

    Expression unaryExp = new UnaryConstant(op, e1);

    Assert.Equal(unaryExp.Evaluate(null), result);
  }

  // AST test for 3(5+6)
  [Fact]
  public void BinaryConstatTest2()
  {
    Expression e1 = new NumericConstant(3);
    Expression e2 = new NumericConstant(5);
    Expression e3 = new NumericConstant(6);
    Expression binaryExp = new BinaryConstant(e1,Operator.MULT ,new BinaryConstant(e2, Operator.PLUS, e3));
    Assert.Equal(binaryExp.Evaluate(null), 33);
  }

}
