using Xunit;
using SLANG;
using System.Text.Json;

public class BinaryExpressionTest
{
  public class BinaryPlusTestNumeric
  {
    Expression numericAddition = new BinaryPlus(new NumericConstant(7), new NumericConstant(4));
    Expression stringConcat = new BinaryPlus(new StringLiteral("Hello"), new StringLiteral("World"));
    RuntimeContext rtx = new RuntimeContext();
    CompilationContext ctx = new CompilationContext();

    [Fact]
    public void EvaluateMethodTest1()
    {
      Symbol s = new Symbol();
      s.Type = TYPE.NUMERIC;
      s.DoubleValue = 11;
      s.Name = "";
      string result = JsonSerializer.Serialize(numericAddition.Evaluate(rtx));
      string expected = JsonSerializer.Serialize(s);
      Assert.Equal(expected, result);
    }

    [Fact]
    public void EvaluateMethodTest2()
    {
      Symbol s = new Symbol();
      s.Type = TYPE.STRING;
      s.StringValue = "HelloWorld";
      s.Name = "";
      string result = JsonSerializer.Serialize(stringConcat.Evaluate(rtx));
      string expected = JsonSerializer.Serialize(s);
      Assert.Equal(expected, result);
    }

    [Fact]
    public void TypeCheckMethodTest1()
    {
      Assert.Equal(TYPE.NUMERIC, numericAddition.TypeCheck(ctx));
    }

    [Fact]
    public void TypeCheckMethodTest2()
    {
      Assert.Equal(TYPE.STRING, stringConcat.TypeCheck(ctx));
    }

    [Fact]
    public void Get_TypeMethodTest1()
    {
      numericAddition.TypeCheck(ctx);
      Assert.Equal(TYPE.NUMERIC, numericAddition.Get_Type());
    }

    [Fact]
    public void Get_TypeMethodTest2()
    {
      stringConcat.TypeCheck(ctx);
      Assert.Equal(TYPE.STRING, stringConcat.Get_Type());
    }
  }

  public class BinaryMinusTest
  {
    Expression exp = new BinaryMinus(new NumericConstant(7), new NumericConstant(4));
    RuntimeContext rtx = new RuntimeContext();
    CompilationContext ctx = new CompilationContext();

    [Fact]
    public void EvaluateMethodTest()
    {
      Symbol s = new Symbol();
      s.Type = TYPE.NUMERIC;
      s.DoubleValue = 3;
      s.Name = "";
      string result = JsonSerializer.Serialize(exp.Evaluate(rtx));
      string expected = JsonSerializer.Serialize(s);
      Assert.Equal(expected, result);
    }

    [Fact]
    public void TypeCheckMethodTest()
    {
      Assert.Equal(TYPE.NUMERIC, exp.TypeCheck(ctx));
    }

    [Fact]
    public void Get_TypeMethodTest()
    {
      exp.TypeCheck(ctx);
      Assert.Equal(TYPE.NUMERIC, exp.Get_Type());
    }
  }

  public class BinaryMultiplicationTest
  {
    Expression exp = new BinaryMultiplication(new NumericConstant(7), new NumericConstant(4));
    RuntimeContext rtx = new RuntimeContext();
    CompilationContext ctx = new CompilationContext();

    [Fact]
    public void EvaluateMethodTest()
    {
      Symbol s = new Symbol();
      s.Type = TYPE.NUMERIC;
      s.DoubleValue = 28;
      s.Name = "";
      string result = JsonSerializer.Serialize(exp.Evaluate(rtx));
      string expected = JsonSerializer.Serialize(s);
      Assert.Equal(expected, result);
    }

    [Fact]
    public void TypeCheckMethodTest()
    {
      Assert.Equal(TYPE.NUMERIC, exp.TypeCheck(ctx));
    }

    [Fact]
    public void Get_TypeMethodTest()
    {
      exp.TypeCheck(ctx);
      Assert.Equal(TYPE.NUMERIC, exp.Get_Type());
    }
  }

  public class BinaryDivisionTest
  {
    Expression exp = new BinaryDivision(new NumericConstant(7), new NumericConstant(4));
    RuntimeContext rtx = new RuntimeContext();
    CompilationContext ctx = new CompilationContext();

    [Fact]
    public void EvaluateMethodTest()
    {
      Symbol s = new Symbol();
      s.Type = TYPE.NUMERIC;
      s.DoubleValue = 1.75;
      s.Name = "";
      string result = JsonSerializer.Serialize(exp.Evaluate(rtx));
      string expected = JsonSerializer.Serialize(s);
      Assert.Equal(expected, result);
    }

    [Fact]
    public void TypeCheckMethodTest()
    {
      Assert.Equal(TYPE.NUMERIC, exp.TypeCheck(ctx));
    }

    [Fact]
    public void Get_TypeMethodTest()
    {
      exp.TypeCheck(ctx);
      Assert.Equal(TYPE.NUMERIC, exp.Get_Type());
    }
  }
}