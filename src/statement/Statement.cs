namespace SLANG
{
  // Statement base class
  public abstract class Stmt
  {
    public abstract Symbol accept(Visitor v,RuntimeContext con);
  }
}