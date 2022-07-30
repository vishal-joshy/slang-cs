using System.Reflection.Emit;
using System.Collections;

namespace SLANG
{
    public class Compiler : IVisitor
    {
        // Expression
        // Constants
        public SYMBOL Visit(CONTEXT cont, BooleanConstant boolConst)
        {
            SYMBOL s = boolConst.GetConstant();
            cont.CodeOutput.Emit(OpCodes.Ldc_I4, s.BooleanValue ? 1 : 0);
            return null;
        }
        public SYMBOL Visit(CONTEXT cont, NumericConstant numConst)
        {
            SYMBOL s = numConst.GetConstant();
            cont.CodeOutput.Emit(OpCodes.Ldc_R8, s.DoubleValue);
            return null;
        }
        public SYMBOL Visit(CONTEXT cont, StringLiteral str)
        {
            SYMBOL s = str.GetConstant();
            cont.CodeOutput.Emit(OpCodes.Ldstr, s.StringValue);
            return null;
        }

        // Operations
        public SYMBOL Visit(CONTEXT cont, BinaryExpression bExp)
        {
            bExp.GetLExp().accept(cont,this);
            bExp.GetRExp().accept(cont,this);
            if (bExp.get_type() == TYPE_INFO.STRING)
            {
                Type[] str2 = { typeof(string), typeof(string) };
                cont.CodeOutput.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", str2));
            }
            else
            {
                switch (bExp.GetOperator())
                {
                    case ARITHMETIC_OPERATOR.PLUS: cont.CodeOutput.Emit(OpCodes.Add); break;
                    case ARITHMETIC_OPERATOR.MINUS: cont.CodeOutput.Emit(OpCodes.Sub); break;
                    case ARITHMETIC_OPERATOR.MULT: cont.CodeOutput.Emit(OpCodes.Mul); break;
                    case ARITHMETIC_OPERATOR.DIV: cont.CodeOutput.Emit(OpCodes.Div); break;
                    default: throw new Exception("Invalid operator for arithmetic operation");
                }
            }
            return null;
        }

        public SYMBOL Visit(CONTEXT cont, UnaryExpression uExp)
        {
            uExp.GetExpression().accept(cont,this);
            if (uExp.GetOperator() == ARITHMETIC_OPERATOR.MINUS)
                cont.CodeOutput.Emit(OpCodes.Neg);

            return null;
        }

        public SYMBOL Visit(CONTEXT cont, LogicalExpression logicalExp)
        {
            logicalExp.GetLExp().accept(cont,this);
            logicalExp.GetRExp().accept(cont,this);

            if (logicalExp.GetOperator() == TOKEN.AND)
                cont.CodeOutput.Emit(OpCodes.And);
            if (logicalExp.GetOperator() == TOKEN.OR)
                cont.CodeOutput.Emit(OpCodes.Or);

            return null;
        }

        public SYMBOL Visit(CONTEXT cont, LogicalNot lNotExp)
        {
            lNotExp.GetExpression().accept(cont,this);

            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
            cont.CodeOutput.Emit(OpCodes.Ceq);
            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
            cont.CodeOutput.Emit(OpCodes.Ceq);

            return null;
        }

        public SYMBOL Visit(CONTEXT cont, RelationalExpression relationExp)
        {
            relationExp.GetLExp().accept(cont,this);
            relationExp.GetRExp().accept(cont,this);
            RELATIONAL_OPERATOR op = relationExp.GetOperator();

            if (relationExp.get_type() == TYPE_INFO.STRING)
            {
                Type[] str = { typeof(string), typeof(string) };
                cont.CodeOutput.Emit(OpCodes.Call, typeof(string).GetMethod("Compare", str));

                if (op == RELATIONAL_OPERATOR.EQUALITY)
                {
                    cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                    cont.CodeOutput.Emit(OpCodes.Ceq);
                }
                else
                {
                    cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                    cont.CodeOutput.Emit(OpCodes.Ceq);
                    cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                    cont.CodeOutput.Emit(OpCodes.Ceq);
                }
                return null;
            }

            if (op == RELATIONAL_OPERATOR.EQUALITY)
                cont.CodeOutput.Emit(OpCodes.Ceq);
            else if (op == RELATIONAL_OPERATOR.GREATER_THAN)
                cont.CodeOutput.Emit(OpCodes.Cgt);
            else if (op == RELATIONAL_OPERATOR.LESS_THAN)
                cont.CodeOutput.Emit(OpCodes.Clt);
            else if (op == RELATIONAL_OPERATOR.NOTEQUALITY)
            {
                cont.CodeOutput.Emit(OpCodes.Ceq);
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            else if (op == RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY)
            {
                cont.CodeOutput.Emit(OpCodes.Clt);
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            else if (op == RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY)
            {
                cont.CodeOutput.Emit(OpCodes.Cgt);
                cont.CodeOutput.Emit(OpCodes.Ldc_I4, 0);
                cont.CodeOutput.Emit(OpCodes.Ceq);
            }
            return null;
        }

        // Variabe
        public SYMBOL Visit(CONTEXT cont, Variable varExp)
        {
            SYMBOL info = cont.TABLE.Get(varExp.GetName());
            LocalBuilder lb = cont.GetLocal(info.loc_position);
            cont.CodeOutput.Emit(OpCodes.Ldloc, lb);
            return null;
        }

        // Function
        public SYMBOL Visit(CONTEXT cont, CallProcedureExpression callProcExp)
        {
            Procedure proc = callProcExp.GetProcedure();
            if (proc == null)
            {
                string pName = callProcExp.GetProcedureName();
                cont.GetProgram().FindProcedure(pName);
            }

            string name = proc.Name;
            TModule module = cont.GetProgram();
            MethodBuilder mBuilder = module.GetEntryPoint(name);

            ArrayList actuals = callProcExp.GetActuals();

            foreach (Expression exp in actuals)
            {
                exp.accept(cont,this);
            }

            cont.CodeOutput.Emit(OpCodes.Call, mBuilder);
            return null;
        }

        // Statements
        public SYMBOL Visit(CONTEXT cont, PrintStatement ps)
        {
            ps.GetExpression().accept(cont,this);

            System.Type SType = Type.GetType("System.Console");
            Type[] parameters = new Type[1];

            TYPE_INFO tData = ps.GetExpression().get_type();

            if (tData == TYPE_INFO.STRING)
                parameters[0] = typeof(string);
            else if (tData == TYPE_INFO.NUMERIC)
                parameters[0] = typeof(double);
            else
                parameters[0] = typeof(bool);

            if (ps.GetIsPrintLine())
            {
                cont.CodeOutput.Emit(OpCodes.Call, SType.GetMethod("WriteLine", parameters));
            }
            else
            {
                cont.CodeOutput.Emit(OpCodes.Call, SType.GetMethod("Write", parameters));
            }
            return null;
        }

        // Variables
        public SYMBOL Visit(CONTEXT cont, VariableDeclStatement vds)
        {
            SYMBOL inf = vds.GetSymbol();
            System.Type type = (inf.Type == TYPE_INFO.BOOL) ?
                typeof(bool) : (inf.Type == TYPE_INFO.NUMERIC) ?
                typeof(double) : typeof(string);

            int s = cont.DeclareLocal(type);
            inf.loc_position = s;
            cont.TABLE.Add(inf);
            return null;
        }
        public SYMBOL Visit(CONTEXT cont, AssignmentStatement aStmt)
        {
            aStmt.GetExpression().accept(cont, this);
            // if (!aStmt.GetExpression().Compile(cont))
            // {
            //     throw new Exception("Compilation Error");
            // }

            SYMBOL info = cont.TABLE.Get(aStmt.GetVariable().Name);

            LocalBuilder lb = cont.GetLocal(info.loc_position);
            cont.CodeOutput.Emit(OpCodes.Stloc, lb);
            return null;
        }

        // Control
        public SYMBOL Visit(CONTEXT cont, IfStatement ifStmt)
        {
            System.Reflection.Emit.Label trueLabel, falseLabel;
            trueLabel = cont.CodeOutput.DefineLabel();
            falseLabel = cont.CodeOutput.DefineLabel();

            ifStmt.GetCondition().accept(cont,this);

            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
            cont.CodeOutput.Emit(OpCodes.Ceq);

            cont.CodeOutput.Emit(OpCodes.Brfalse, falseLabel);

            foreach (Statement s in ifStmt.GetTruePart())
            {
                s.accept(cont, this);
            }

            cont.CodeOutput.Emit(OpCodes.Br, trueLabel);
            cont.CodeOutput.MarkLabel(falseLabel);

            if (ifStmt.GetElsePart() != null)
            {
                foreach (Statement s in ifStmt.GetElsePart())
                {
                    s.accept(cont, this);
                }
            }

            cont.CodeOutput.MarkLabel(trueLabel);
            return null;
        }

        public SYMBOL Visit(CONTEXT cont, WhileStatement wStmt)
        {
            Label true_label, false_label;
            true_label = cont.CodeOutput.DefineLabel();
            false_label = cont.CodeOutput.DefineLabel();

            cont.CodeOutput.MarkLabel(true_label);
            wStmt.GetCondition().accept(cont, this);
            cont.CodeOutput.Emit(OpCodes.Ldc_I4, 1);
            cont.CodeOutput.Emit(OpCodes.Ceq);
            cont.CodeOutput.Emit(OpCodes.Brfalse, false_label);

            foreach (Statement rst in wStmt.GetBody())
            {
                rst.accept(cont, this);
            }

            cont.CodeOutput.Emit(OpCodes.Br, true_label);
            cont.CodeOutput.MarkLabel(false_label);
            return null;
        }

        // Function
        public SYMBOL Visit(CONTEXT cont, ReturnStatement rStmt)
        {
            if (rStmt.GetExpression() != null)
            {
                rStmt.GetExpression().accept(cont, this);
            }
            cont.CodeOutput.Emit(OpCodes.Ret);
            return null;
        }
    }
}