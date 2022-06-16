using Xunit;
using SLANG;
using System.Text.Json;

public class AssignmentStatementTest
{
  [Fact]
  public void EvaluateMethodTest()
  {
    Symbol s= new Symbol();
    s.Name="test";
    Variable var = new Variable(s);
    Expression exp = new NumericConstant(5);
    Stmt stmt = new AssignmentStatement(var, exp);
    RuntimeContext rtx = new RuntimeContext();

    stmt.Execute(rtx);

    Symbol expected = new Symbol();
    expected.Name="test";
    expected.Type=TYPE.NUMERIC;
    expected.DoubleValue=5;
    string expectedStr = JsonSerializer.Serialize(expected);
    string actualStr = JsonSerializer.Serialize(rtx.TABLE.Get("test"));

    Assert.Equal(expectedStr, actualStr);
  }
}