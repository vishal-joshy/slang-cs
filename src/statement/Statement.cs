namespace SLANG
{
  // Statement base class
  public abstract class Stmt
  {
    public abstract Symbol Execute(RuntimeContext con);
  }
}