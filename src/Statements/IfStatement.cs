using System.Collections;
using System.Reflection.Emit;

namespace SLANG
{
    public class IfStatement : Statement
    {
        private Expression _condition;
        private ArrayList _trueStatements;
        private ArrayList _elseStatements;

        public IfStatement(Expression condition, ArrayList truePart, ArrayList elsePart)
        {
            _condition = condition;
            _trueStatements = truePart;
            _elseStatements = elsePart;
        }

        public Expression GetCondition() => _condition;
        public ArrayList GetTruePart() => _trueStatements;
        public ArrayList GetElsePart() => _elseStatements;

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }
    }
}