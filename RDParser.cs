using System.Collections;

namespace SLANG
{
    public class RDParser : LexicalAnalyzer
    {
        TModuleBuilder prog = null;

        public RDParser(string str) : base(str)
        {
            prog = new TModuleBuilder();
        }

        public TModule DoParse()
        {
            try
            {
                GetNext();
                return ParseFunctions();
            }
            catch (Exception e)
            {
                Console.WriteLine("Parse Error -------");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        // <BExpr> ::= <BExpr> {&& | ||} <LExpr>
        private Expression BExpr(ProcedureBuilder pb)
        {
            TOKEN token;
            Expression returnValue = LExpr(pb);
            while (CurrentToken == TOKEN.AND || CurrentToken == TOKEN.OR)
            {
                token = CurrentToken;
                CurrentToken = GetNext();
                Expression e2 = LExpr(pb);

                returnValue = new LogicalExpression(token, returnValue, e2);
            }
            return returnValue;
        }

       // <LExpr> ::=  <LExpr> {== | != | < | > | <= | >=} <Expr>
        private Expression LExpr(ProcedureBuilder pb)
        {
            TOKEN token;
            Expression returnValue = Expr(pb);
            while (IsRelationalOperator(CurrentToken))
            {
                token = CurrentToken;
                CurrentToken = GetNext();
                Expression e2 = Expr(pb);
                RELATIONAL_OPERATOR op = GetRelOp(token);

                returnValue = new RelationalExpression(op, returnValue, e2);
            }
            return returnValue;
        }

        // <Expr> ::= <Term> | <Term> {+|-} <Expr>
        private Expression Expr(ProcedureBuilder ctx)
        {
            TOKEN token;
            Expression returnValue = Term(ctx);
            while (CurrentToken == TOKEN.PLUS || CurrentToken == TOKEN.MINUS)
            {
                token = CurrentToken;
                CurrentToken = GetToken();
                Expression e1 = Expr(ctx);

                if (token == TOKEN.PLUS)
                    returnValue = new BinaryExpression(ARITHMETIC_OPERATOR.PLUS, returnValue, e1);
                else
                    returnValue = new BinaryExpression(ARITHMETIC_OPERATOR.MINUS, returnValue, e1);
            }
            return returnValue;
        }

        // <Term> ::= <Factor> | <Factor> {*|/} <Term>
        private Expression Term(ProcedureBuilder ctx)
        {
            TOKEN token;
            Expression returnValue = Factor(ctx);

            while (CurrentToken == TOKEN.MULT || CurrentToken == TOKEN.DIV)
            {
                token = CurrentToken;
                CurrentToken = GetToken();
                Expression e1 = Term(ctx);

                if (token == TOKEN.MULT)
                    returnValue = new BinaryExpression(ARITHMETIC_OPERATOR.MULT, returnValue, e1);
                else
                    returnValue = new BinaryExpression(ARITHMETIC_OPERATOR.DIV, returnValue, e1);
            }
            return returnValue;
        }

        // <Factor> ::= NUMBER|STRING|TRUE|FALSE|VARIABLE|(<Expr>)|{+| -}<Factor>
        private Expression Factor(ProcedureBuilder ctx)
        {
            TOKEN token;
            Expression returnValue = null;
            if (CurrentToken == TOKEN.NUMERIC)
            {
                returnValue = new NumericConstant(GetNumber());
                CurrentToken = GetToken();
            }
            else if (CurrentToken == TOKEN.STRING)
            {
                returnValue = new StringLiteral(GetString());
                CurrentToken = GetToken();
            }
            else if (CurrentToken == TOKEN.BOOLEAN_FALSE || CurrentToken == TOKEN.BOOLEAN_TRUE)
            {
                returnValue = new BooleanConstant(CurrentToken == TOKEN.BOOLEAN_TRUE ? true : false);
                CurrentToken = GetToken();
            }
            else if (CurrentToken == TOKEN.OPAREN)
            {
                CurrentToken = GetToken();
                returnValue = BExpr(ctx);
                Expect(TOKEN.CPAREN);
                CurrentToken = GetToken();
            }

            else if (CurrentToken == TOKEN.PLUS || CurrentToken == TOKEN.MINUS)
            {
                token = CurrentToken;
                CurrentToken = GetToken();
                returnValue = Factor(ctx);
                if (token == TOKEN.PLUS)
                    returnValue = new UnaryExpression(ARITHMETIC_OPERATOR.PLUS, returnValue);
                else
                    returnValue = new UnaryExpression(ARITHMETIC_OPERATOR.MINUS, returnValue);
            }
            else if (CurrentToken == TOKEN.NOT)
            {
                token = CurrentToken;
                CurrentToken = GetToken();
                returnValue = Factor(ctx);
                returnValue = new LogicalNot(returnValue);
            }
            else if (CurrentToken == TOKEN.UNQUOTED_STRING)
            {
                string str = GetString();
                if (!prog.IsFunction(str))
                {
                    SYMBOL inf = ctx.GetSymbol(str);
                    if (inf == null)
                        throw new Exception("Undefined symbol");

                    GetNext();
                    return new Variable(inf);
                }
                Procedure p = prog.GetProc(str);
                Expression ptr = ParseCallProc(ctx, p);
                GetNext();
                return ptr;
            }
            else
            {
                Console.WriteLine("Illegal Token");
                throw new Exception();
            }
            return returnValue;
        }

        private Expression ParseCallProc(ProcedureBuilder pb, Procedure p)
        {
            GetNext();
            Expect(TOKEN.OPAREN);
            GetNext();
            ArrayList actualparams = new ArrayList();

            while (true)
            {
                Expression Expression = BExpr(pb);
                Expression.TypeCheck(pb.Context);

                if (CurrentToken == TOKEN.COMMA)
                {
                    actualparams.Add(Expression);
                    GetNext();
                    continue;
                }
                Expect(TOKEN.CPAREN);
                actualparams.Add(Expression);
                break;
            }

            if (p != null)
                return new CallProcedureExpression(p, actualparams);
            else
                return new CallProcedureExpression(pb.Name, true, actualparams);
        }


        private TModule ParseFunctions()
        {
            while (CurrentToken == TOKEN.FUNCTION)
            {
                ProcedureBuilder b = ParseFunction();
                Procedure s = b.GetProcedure();

                if (s == null)
                {
                    Console.WriteLine("Error While Parsing Functions");
                    return null;
                }
                prog.Add(s);
                GetNext();
            }
            return prog.GetProgram();
        }

        private ProcedureBuilder ParseFunction()
        {
            ProcedureBuilder pb = new ProcedureBuilder("", new COMPILATION_CONTEXT());

            Expect(TOKEN.FUNCTION);
            CurrentToken = GetNext();

            if (!(CurrentToken == TOKEN.VAR_BOOLEAN || CurrentToken == TOKEN.VAR_NUMBER || CurrentToken == TOKEN.VAR_STRING))
            {
                throw new Exception("Return type expected");
            }

            pb.TYPE = (CurrentToken == TOKEN.VAR_BOOLEAN) ?
                TYPE_INFO.BOOL : (CurrentToken == TOKEN.VAR_NUMBER) ?
                TYPE_INFO.NUMERIC : TYPE_INFO.STRING;

            CurrentToken = GetNext();
            Expect(TOKEN.UNQUOTED_STRING);
            pb.Name = GetString();

            CurrentToken = GetNext();
            Expect(TOKEN.OPAREN);
            FormalParameters(pb);
            Expect(TOKEN.CPAREN);
            CurrentToken = GetNext();
            ArrayList lst = StatementList(pb);
            Expect(TOKEN.END);

            foreach (Statement s in lst)
            {
                pb.AddStatement(s);
            }
            return pb;
        }

        private void FormalParameters(ProcedureBuilder pb)
        {
            Expect(TOKEN.OPAREN);
            GetNext();

            ArrayList lst_types = new ArrayList();

            while (CurrentToken == TOKEN.VAR_BOOLEAN ||
                CurrentToken == TOKEN.VAR_NUMBER ||
                CurrentToken == TOKEN.VAR_STRING)
            {
                SYMBOL inf = new SYMBOL();

                inf.Type = (CurrentToken == TOKEN.VAR_BOOLEAN) ?
                    TYPE_INFO.BOOL : (CurrentToken == TOKEN.VAR_NUMBER) ?
                    TYPE_INFO.NUMERIC : TYPE_INFO.STRING;

                GetNext();
                Expect(TOKEN.UNQUOTED_STRING);

                inf.Name = GetString();
                lst_types.Add(inf.Type);
                pb.AddFormals(inf);
                pb.AddLocal(inf);

                GetNext();

                if (CurrentToken != TOKEN.COMMA)
                {
                    break;
                }
                GetNext();
            }
            prog.AddFunctionProtoType(pb.Name, pb.TYPE, lst_types);
            return;
        }

        //  <statementList> := { <statement> }+
        private ArrayList StatementList(ProcedureBuilder ctx)
        {
            ArrayList arr = new ArrayList();

            while (
                    (CurrentToken != TOKEN.ELSE) &&
                    (CurrentToken != TOKEN.ENDIF) &&
                    (CurrentToken != TOKEN.WEND) &&
                    (CurrentToken != TOKEN.END)
              )
            {
                Statement temp = ParseStatement(ctx);
                if (temp != null)
                {
                    arr.Add(temp);
                }
            }
            return arr;
        }

        // <statement> := <print> | <variabledecal> | <assignmentstmt> || <if> || <while> || <return>
        private Statement ParseStatement(ProcedureBuilder ctx)
        {
            Statement returnValue = null;
            switch (CurrentToken)
            {
                case TOKEN.VAR_STRING:
                case TOKEN.VAR_NUMBER:
                case TOKEN.VAR_BOOLEAN:
                    returnValue = ParseVariableDeclStatement(ctx);
                    GetNext();
                    return returnValue;
                case TOKEN.PRINT:
                    returnValue = ParsePrintStatement(ctx);
                    GetNext();
                    break;
                case TOKEN.PRINTLN:
                    returnValue = ParsePrintLNStatement(ctx);
                    GetNext();
                    break;
                case TOKEN.UNQUOTED_STRING:
                    returnValue = ParseAssignmentStatement(ctx);
                    GetNext();
                    return returnValue;
                case TOKEN.IF:
                    returnValue = ParseIfStatement(ctx);
                    GetNext();
                    return returnValue;
                case TOKEN.WHILE:
                    returnValue = ParseWhileStatement(ctx);
                    GetNext();
                    return returnValue;
                case TOKEN.RETURN:
                    returnValue = ParseReturnStatement(ctx);
                    GetNext();
                    return returnValue;

                default:
                    throw new Exception("Invalid statement");
            }
            return returnValue;
        }

        private Statement ParsePrintStatement(ProcedureBuilder pb)
        {
            GetNext();
            Expression exp = BExpr(pb);
            exp.TypeCheck(pb.Context);
            Expect(TOKEN.SEMI);
            return new PrintStatement(exp, false);
        }

        private Statement ParsePrintLNStatement(ProcedureBuilder pb)
        {
            GetNext();
            Expression exp = Expr(pb);
            exp.TypeCheck(pb.Context);
            Expect(TOKEN.SEMI);
            return new PrintStatement(exp, true);
        }

        private Statement ParseVariableDeclStatement(ProcedureBuilder pb)
        {
            TOKEN tok = CurrentToken;
            CurrentToken = GetNext();
            Expect(TOKEN.UNQUOTED_STRING);
            SYMBOL inf = new SYMBOL();
            inf.Name = GetString();
            inf.Type = (tok == TOKEN.VAR_BOOLEAN) ? TYPE_INFO.BOOL : (tok == TOKEN.VAR_NUMBER) ? TYPE_INFO.NUMERIC : TYPE_INFO.STRING;
            CurrentToken = GetNext();
            Expect(TOKEN.SEMI);
            pb.TABLE.Add(inf);
            return new VariableDeclStatement(inf);
        }

        private Statement ParseAssignmentStatement(ProcedureBuilder pb)
        {
            string variableName = GetString();
            SYMBOL variable = pb.TABLE.Get(variableName);
            if (variable == null)
            {
                throw new Exception("variable not found");
            }
            GetNext();
            Expect(TOKEN.ASSIGN);

            GetNext();
            Expression Expression = BExpr(pb);

            if (Expression.TypeCheck(pb.Context) != variable.Type)
            {
                throw new Exception("Type mismatch in assignment");
            }

            Expect(TOKEN.SEMI);
            return new AssignmentStatement(variable, Expression);
        }

        private Statement ParseIfStatement(ProcedureBuilder pb)
        {
            GetNext();
            ArrayList truePart = null;
            ArrayList falsePart = null;
            Expression condition = BExpr(pb);

            if (pb.TypeCheck(condition) != TYPE_INFO.BOOL)
            {
                throw new Exception("Expects a boolean expression");
            }

            Expect(TOKEN.THEN);
            GetNext();
            truePart = StatementList(pb);

            if (CurrentToken == TOKEN.ENDIF)
            {
                return new IfStatement(condition, truePart, falsePart);
            }

            Expect(TOKEN.ELSE);
            GetNext();
            falsePart = StatementList(pb);

            Expect(TOKEN.ENDIF);
            return new IfStatement(condition, truePart, falsePart);
        }

        private Statement ParseWhileStatement(ProcedureBuilder pb)
        {
            GetNext();
            Expression condition = BExpr(pb);
            if (pb.TypeCheck(condition) != TYPE_INFO.BOOL)
            {
                throw new Exception("Expects a boolean expression as condition");
            }
            ArrayList loopBody = StatementList(pb);
            Expect(TOKEN.WEND);
            return new WhileStatement(condition, loopBody);
        }

        private Statement ParseReturnStatement(ProcedureBuilder pb)
        {
            GetNext();
            Expression exp = BExpr(pb);
            Expect(TOKEN.SEMI);
            pb.TypeCheck(exp);
            return new ReturnStatement(exp);
        }

        private RELATIONAL_OPERATOR GetRelOp(TOKEN tok)
        {
            if (tok == TOKEN.EQUALITY)
                return RELATIONAL_OPERATOR.EQUALITY;
            else if (tok == TOKEN.NOTEQUALITY)
                return RELATIONAL_OPERATOR.NOTEQUALITY;
            else if (tok == TOKEN.GREATER_THAN)
                return RELATIONAL_OPERATOR.GREATER_THAN;
            else if (tok == TOKEN.GREATER_THAN_OR_EQUALITY)
                return RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY;
            else if (tok == TOKEN.LESS_THAN)
                return RELATIONAL_OPERATOR.LESS_THAN;
            else
                return RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY;
        }

        private static bool IsRelationalOperator(TOKEN token)
        {
            if (token == TOKEN.GREATER_THAN || token == TOKEN.LESS_THAN
                || token == TOKEN.GREATER_THAN_OR_EQUALITY || token == TOKEN.LESS_THAN_OR_EQUALITY
                || token == TOKEN.NOTEQUALITY || token == TOKEN.EQUALITY)
            {
                return true;
            }
            return false;
        }
    }
}
