namespace SLANG
{
  // Statement base class
  public abstract class Stmt
  {
    public abstract Symbol accept(Visitor v,RuntimeContext con);
    public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx);
  }
}