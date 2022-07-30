using System.Collections;

namespace SLANG
{
    public interface IVisitor
    {
        // Expression
        // Constants
        SYMBOL Visit(CONTEXT cont, BooleanConstant boolConst);
        SYMBOL Visit(CONTEXT cont, NumericConstant numConst);
        SYMBOL Visit(CONTEXT cont, StringLiteral str);
        // Operations
        SYMBOL Visit(CONTEXT cont, BinaryExpression bExp);
        SYMBOL Visit(CONTEXT cont, UnaryExpression uExp);
        SYMBOL Visit(CONTEXT cont, LogicalExpression logicalExp);
        SYMBOL Visit(CONTEXT cont, LogicalNot lNotExp);
        SYMBOL Visit(CONTEXT cont, RelationalExpression relationExp);
        // Variabe
        SYMBOL Visit(CONTEXT cont, Variable varExp);
        // Function
        SYMBOL Visit(CONTEXT cont, CallProcedureExpression callProcExp);

        // Statements
        SYMBOL Visit(CONTEXT cont, PrintStatement ps);
        // Variables
        SYMBOL Visit(CONTEXT cont, VariableDeclStatement vds);
        SYMBOL Visit(CONTEXT cont, AssignmentStatement aStmt);
        // Control
        SYMBOL Visit(CONTEXT cont, IfStatement ifStmt);
        SYMBOL Visit(CONTEXT cont, WhileStatement wStmt);
        // Function
        SYMBOL Visit(CONTEXT cont, ReturnStatement rStmt);
    }

    public class Interpreter : IVisitor
    {
        // Expression
        // Constants
        public SYMBOL Visit(CONTEXT cont, BooleanConstant boolConst) => boolConst.GetConstant();
        public SYMBOL Visit(CONTEXT cont, NumericConstant numConst) => numConst.GetConstant();
        public SYMBOL Visit(CONTEXT cont, StringLiteral str) => str.GetConstant();

        // Operations
        public SYMBOL Visit(CONTEXT cont, BinaryExpression bExp)
        {
            SYMBOL lEval = bExp.GetLExp().accept(cont,this);
            SYMBOL rEval = bExp.GetRExp().accept(cont,this);
            ARITHMETIC_OPERATOR op = bExp.GetOperator();

            if (lEval.Type == TYPE_INFO.STRING) //String concat
            {
                SYMBOL result = new SYMBOL("", TYPE_INFO.STRING);
                result.StringValue = lEval.StringValue + rEval.StringValue;
                return result;
            }
            else if (lEval.Type == TYPE_INFO.NUMERIC)
            {
                SYMBOL result = new SYMBOL("", TYPE_INFO.NUMERIC);

                switch (op)
                {
                    case ARITHMETIC_OPERATOR.PLUS: result.DoubleValue = lEval.DoubleValue + rEval.DoubleValue; break;
                    case ARITHMETIC_OPERATOR.MINUS: result.DoubleValue = lEval.DoubleValue - rEval.DoubleValue; break;
                    case ARITHMETIC_OPERATOR.MULT: result.DoubleValue = lEval.DoubleValue * rEval.DoubleValue; break;
                    case ARITHMETIC_OPERATOR.DIV: result.DoubleValue = lEval.DoubleValue / rEval.DoubleValue; break;
                    default: throw new Exception("Invalid operator for arithmetic operation");
                }

                return result;
            }
            else
            {
                throw new Exception("Invalid type");
            }
        }

        public SYMBOL Visit(CONTEXT cont, UnaryExpression uExp)
        {
            SYMBOL eval = uExp.GetExpression().accept(cont,this);
            ARITHMETIC_OPERATOR op = uExp.GetOperator();
            SYMBOL result = new SYMBOL("", TYPE_INFO.NUMERIC);

            if (eval.Type != TYPE_INFO.NUMERIC)
            {
                throw new Exception("Type mismatch");
            }
            if (op == ARITHMETIC_OPERATOR.PLUS)
            {
                result.DoubleValue = eval.DoubleValue;
            }
            else
            {
                result.DoubleValue = -eval.DoubleValue;
            }

            return result;
        }

        public SYMBOL Visit(CONTEXT cont, LogicalExpression logicalExp)
        {
            SYMBOL lEval = logicalExp.GetLExp().accept(cont,this);
            SYMBOL rEval = logicalExp.GetRExp().accept(cont,this);
            TOKEN op = logicalExp.GetOperator();
            SYMBOL result = new SYMBOL("", TYPE_INFO.BOOL);

            if (lEval.Type != rEval.Type)
                throw new Exception("Type mismatch");

            if (lEval.Type != TYPE_INFO.BOOL)
                throw new Exception("Invalid type for logical operation");

            if (op == TOKEN.AND)
                result.BooleanValue = (lEval.BooleanValue && rEval.BooleanValue);
            else if (op == TOKEN.OR)
                result.BooleanValue = (lEval.BooleanValue || rEval.BooleanValue);
            else
                throw new Exception("Invalid Logical Operator");

            return result;
        }

        public SYMBOL Visit(CONTEXT cont, LogicalNot lNotExp)
        {
            SYMBOL eval = lNotExp.GetExpression().accept(cont,this);

            if (eval.Type == TYPE_INFO.BOOL)
            {
                SYMBOL result = new SYMBOL("", TYPE_INFO.BOOL);
                result.BooleanValue = !eval.BooleanValue;
                return result;
            }
            else
            {
                throw new Exception("Invalid type for logical Expression");
            }

        }

        public SYMBOL Visit(CONTEXT cont, RelationalExpression relationalExp)
        {
            SYMBOL lEval = relationalExp.GetLExp().accept(cont,this);
            SYMBOL rEval = relationalExp.GetRExp().accept(cont,this);
            RELATIONAL_OPERATOR op = relationalExp.GetOperator();

            SYMBOL result = new SYMBOL("", TYPE_INFO.BOOL);

            if (lEval.Type == TYPE_INFO.NUMERIC && rEval.Type == TYPE_INFO.NUMERIC)
            {
                if (op == RELATIONAL_OPERATOR.EQUALITY)
                    result.BooleanValue = lEval.DoubleValue == rEval.DoubleValue;
                else if (op == RELATIONAL_OPERATOR.NOTEQUALITY)
                    result.BooleanValue = lEval.DoubleValue != rEval.DoubleValue;
                else if (op == RELATIONAL_OPERATOR.GREATER_THAN)
                    result.BooleanValue = lEval.DoubleValue > rEval.DoubleValue;
                else if (op == RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY)
                    result.BooleanValue = lEval.DoubleValue >= rEval.DoubleValue;
                else if (op == RELATIONAL_OPERATOR.LESS_THAN)
                    result.BooleanValue = lEval.DoubleValue < rEval.DoubleValue;
                else if (op == RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY)
                    result.BooleanValue = lEval.DoubleValue <= rEval.DoubleValue;

                return result;
            }

            else if (lEval.Type == TYPE_INFO.STRING && rEval.Type == TYPE_INFO.STRING)
            {
                if (op == RELATIONAL_OPERATOR.EQUALITY)
                {
                    result.BooleanValue = (String.Compare(lEval.StringValue, rEval.StringValue) == 0) ? true : false;
                }
                else if (op == RELATIONAL_OPERATOR.NOTEQUALITY)
                {
                    result.BooleanValue = String.Compare(lEval.StringValue, rEval.StringValue) != 0;
                }
                else
                {
                    result.BooleanValue = false;
                }
                return result;
            }

            if (lEval.Type == TYPE_INFO.BOOL && rEval.Type == TYPE_INFO.BOOL)
            {
                if (op == RELATIONAL_OPERATOR.EQUALITY)
                    result.BooleanValue = lEval.BooleanValue == rEval.BooleanValue;
                else if (op == RELATIONAL_OPERATOR.NOTEQUALITY)
                    result.BooleanValue = lEval.BooleanValue != rEval.BooleanValue;
                else
                    result.BooleanValue = false;

                return result;
            }
            return null;
        }

        // Variable
        public SYMBOL Visit(CONTEXT cont, Variable varExp)
        {
            string variableName = varExp.GetName();
            if (cont.TABLE == null)
            {
                return null;
            }
            else
            {
                SYMBOL a = cont.TABLE.Get(variableName);
                return a;
            }
        }

        // Function
        public SYMBOL Visit(CONTEXT cont, CallProcedureExpression callProcExp)
        {
            string procedureName = callProcExp.GetProcedureName();
            Procedure procedure = callProcExp.GetProcedure();
            ArrayList actuals = callProcExp.GetActuals();

            if (procedure != null)
            {
                RUNTIME_CONTEXT ctx = new RUNTIME_CONTEXT(cont.GetProgram());
                ArrayList lst = new ArrayList();
                foreach (Expression ex in actuals)
                {
                    lst.Add(ex.accept(cont,this));
                }
                return procedure.Execute(ctx, lst);
            }
            else
            {
                procedure = cont.GetProgram().FindProcedure(procedureName);
                RUNTIME_CONTEXT ctx = new RUNTIME_CONTEXT(cont.GetProgram());
                ArrayList lst = new ArrayList();

                foreach (Expression ex in actuals)
                {
                    lst.Add(ex.accept(cont,this));
                }
                return procedure.Execute(ctx, lst);
            }
        }

        // Statements
        public SYMBOL Visit(CONTEXT cont, PrintStatement ps)
        {
            SYMBOL eval = ps.GetExpression().accept(cont,this);
            bool printLine = ps.GetIsPrintLine();
            string result = eval.GetValueAsString();

            if (printLine)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.Write(result);
            }
            return null;
        }

        // Variable
        public SYMBOL Visit(CONTEXT cont, VariableDeclStatement vds)
        {
            SYMBOL inf = vds.GetSymbol();
            cont.TABLE.Add(inf);
            vds.SetVar(inf);
            return null;
        }

        public SYMBOL Visit(CONTEXT cont, AssignmentStatement aStmt)
        {
            SYMBOL eval = aStmt.GetExpression().accept(cont,this);
            Variable var = aStmt.GetVariable();
            cont.TABLE.Assign(var, eval);
            return null;
        }

        // Control
        public SYMBOL Visit(CONTEXT cont, IfStatement ifStmt)
        {
            Expression exp = ifStmt.GetCondition();
            ArrayList trueStatements = ifStmt.GetTruePart();
            ArrayList elseStatements = ifStmt.GetElsePart();

            SYMBOL condition = exp.accept(cont,this);

            if (condition == null || condition.Type != TYPE_INFO.BOOL)
                return null;

            SYMBOL result = null;

            if (condition.BooleanValue == true)
            {
                foreach (Statement s in trueStatements)
                {
                    result = s.accept(cont,this);
                    if (result != null)
                        return result;
                }
            }
            else if (elseStatements != null) //condition == false and else statement exists
            {
                foreach (Statement s in elseStatements)
                {
                    result = s.accept(cont,this);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public SYMBOL Visit(CONTEXT cont, WhileStatement wStmt)
        {
            ArrayList statements = wStmt.GetBody();

        Loop:

            SYMBOL condition = wStmt.GetCondition().accept(cont,this);

            if (condition == null || condition.Type != TYPE_INFO.BOOL)
                return null;

            if (condition.BooleanValue != true)
                return null;

            SYMBOL tsp = null;
            foreach (Statement rst in statements)
            {
                tsp = rst.accept(cont,this);
                if (tsp != null)
                {
                    return tsp;
                }
            }
            goto Loop;
        }

        public SYMBOL Visit(CONTEXT cont, ReturnStatement rStmt)
        {
            SYMBOL s = (rStmt.GetExpression() == null) ? null : rStmt.GetExpression().accept(cont,this);
            rStmt.SetSymbol(s);
            return s;
        }
    }
}