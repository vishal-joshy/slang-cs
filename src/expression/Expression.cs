namespace SLANG
{
  // Expression base class
  public abstract class Expression
  {
    public abstract Symbol Evaluate(RuntimeContext cont);
    public abstract TYPE TypeCheck(CompilationContext cont);
    public abstract TYPE Get_Type ();
  }
}