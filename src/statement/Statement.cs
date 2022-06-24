namespace SLANG
{
  // Statement base class
  public abstract class Stmt
  {
    public abstract Symbol accept(RuntimeContext con,Visitor v);
    public abstract bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT dtx);
  }
}