
namespace SLANG
{

    public abstract class Stmt
    {
        public abstract bool Execute(RuntimeContext con);
    }


    public class PrintStatement : Stmt
    {

        private Expression _expression;

        public PrintStatement(Expression ex)
        {
            _expression = ex;
        }
        public override bool Execute(RuntimeContext con)
        {
            double result = _expression.Evaluate(con);
            Console.Write(result.ToString());
            return true;
        }
    }

    public class PrintLineStatement : Stmt
    {
        private Expression _expression;

        public PrintLineStatement(Expression ex)
        {
            _expression = ex;
        }

        public override bool Execute(RuntimeContext con)
        {
            double result = _expression.Evaluate(con);
            Console.WriteLine(result.ToString());
            return true;
        }
    }
}