using System.Collections;
using System.Collections.Generic;

namespace SLANG
{
  public class RDParser : LexicalAnalyzer
  {
    public TModuleBuilder prog=null;
    public RDParser(String str) : base(str) {
      prog = new TModuleBuilder();
     }

    public ArrayList Parse(ProcedureBuilder ctx)
    {
      GetNext();
      return StatementList(ctx);
    }

    public TModule DoParse(){
      ProcedureBuilder builder = new ProcedureBuilder("MAIN",new CompilationContext());
      ArrayList statements = Parse(builder);

      foreach (Stmt statement in statements)
      {
        builder.AddStatement(statement);
      }

      Procedure p = builder.GetProcedure();
      prog.AddProcedure(p);
      return prog.GetProgram();
    }

    // <stmtlist> := { <statement> }+
    private ArrayList StatementList(ProcedureBuilder ctx)
    {
      ArrayList arr = new ArrayList();
      while (CurrentToken != TOKEN.ELSE && CurrentToken != TOKEN.WEND &&
        CurrentToken != TOKEN.ENDIF && CurrentToken != TOKEN.NULL)
      {
        Stmt temp = Statement(ctx);
        if (temp != null)
        {
          arr.Add(temp);
        }
      }
      return arr;
    }

    // <statement> := <printstmt> | <printlinestmt> | <variabledecalstmt> | <assignmentstmt>
    private Stmt Statement(ProcedureBuilder ctx)
    {
      Stmt result = null;
      switch (CurrentToken)
      {
        case TOKEN.VAR_BOOLEAN:
        case TOKEN.VAR_NUMBER:
        case TOKEN.VAR_STRING:
          result = ParseVariableDeclarationStatement(ctx);
          GetNext();
          break;
        case TOKEN.UNQUOTED_STRING:
          result = ParseAssignmentStatement(ctx);
          GetNext();
          break;
        case TOKEN.PRINT:
          result = ParsePrintStatement(ctx);
          GetNext();
          break;
        case TOKEN.PRINTLN:
          result = ParsePrintLNStatement(ctx);
          GetNext();
          break;
        case TOKEN.IF:
          result = ParseIfStatement(ctx);
          GetNext();
          break;
        case TOKEN.WHILE:
          result = ParseWhileStatement(ctx);
          GetNext();
          break;
        default:
          throw new Exception("Invalid statement" + CurrentToken);
      }
      return result;
    }

    //<BExpr> ::= <LExpr> LogicalOps <BExpr>
    public Expression BExpr(ProcedureBuilder ctx){
      TOKEN token;
      Expression returnValue = LExpr(ctx);
      while(CurrentToken == TOKEN.AND || CurrentToken == TOKEN.OR){
        token = CurrentToken;
        GetNext();
        Expression e1 = BExpr(ctx);
        returnValue = new LogicalExpression(returnValue, token, e1);
      }
      return returnValue;
    }

    //<LExpr> ::= <RExpr> RelationalOps <LExpr>
    public Expression LExpr(ProcedureBuilder ctx){
      TOKEN token;
      Expression returnValue = RExpr(ctx);
      while(CurrentToken == TOKEN.EQUALITY || CurrentToken == TOKEN.NOTEQUALITY||
        CurrentToken == TOKEN.GREATER_THAN || CurrentToken == TOKEN.GREATER_THAN_OR_EQUALITY ||
        CurrentToken == TOKEN.LESS_THAN || CurrentToken == TOKEN.LESS_THAN_OR_EQUALITY)
      {
        token= CurrentToken;
        GetNext();
        Expression e1 = LExpr(ctx);
        RELATIONAL_OPERATOR op = GetRelationalOperator(token);
        returnValue = new RelationalExpression(returnValue, op, e1);
      }
      return returnValue;
    }

    //<RExpr> ::= <Term> | Term { + | - } <Expr>
    public Expression RExpr(ProcedureBuilder ctx)
    {
      TOKEN token;
      Expression returnValue = Term(ctx);
      while (CurrentToken == TOKEN.PLUS || CurrentToken == TOKEN.MINUS)
      {
        token = CurrentToken;
        GetNext();
        Expression e1 = RExpr(ctx);

        if (token == TOKEN.PLUS)
          returnValue = new BinaryPlus(returnValue, e1);
        else
          returnValue = new BinaryMinus(returnValue, e1);

      }
      return returnValue;
    }

    // <Term> ::= <Factor> | <Factor> {*|/} <Term>
    public Expression Term(ProcedureBuilder ctx)
    {
      TOKEN token;
      Expression returnValue = Factor(ctx);
      while (CurrentToken == TOKEN.MULT || CurrentToken == TOKEN.DIV)
      {
        token = CurrentToken;
        GetNext();
        Expression e1 = Term(ctx);

        if (token == TOKEN.MULT)
          returnValue = new BinaryMultiplication(returnValue, e1);
        else
          returnValue = new BinaryDivision(returnValue, e1);
      }
      return returnValue;
    }

    // <Factor>::= <number> | ( <expr> ) | {+|-} <factor> | <variable> | TRUE | FALSE
    public Expression Factor(ProcedureBuilder ctx)
    {
      TOKEN token;
      Expression returnValue = null;
      // <number>
      if (CurrentToken == TOKEN.NUMERIC)
      {
        returnValue = new NumericConstant(GetNumber());
        GetNext();
      }
      else if (CurrentToken == TOKEN.STRING)
      {
        returnValue = new StringLiteral(GetString());
        GetNext();
      }
      // ( <expr> ) brackets
      else if (CurrentToken == TOKEN.OPAREN)
      {
        GetNext();
        returnValue = BExpr(ctx);
        if (CurrentToken != TOKEN.CPAREN)
        {
          throw new Exception("Missing )");
        }
        CurrentToken = GetToken();
      }

      // {+/-} factor
      else if (CurrentToken == TOKEN.PLUS || CurrentToken == TOKEN.MINUS)
      {
        token = CurrentToken;
        GetNext();
        returnValue = Factor(ctx);
        if (token == TOKEN.PLUS)
          returnValue = new UnaryPlus(returnValue);
        else
          returnValue = new UnaryMinus(returnValue);
      }
      // variable
      else if (CurrentToken == TOKEN.UNQUOTED_STRING)
      {
        string str = GetString();
        Symbol s = ctx.TABLE.Get(str);
        if (s == null)
          throw new Exception("Undefined Symbol");
        GetNext();
        returnValue = new Variable(s);
      }
      // TRUE|FALSE
      else if (CurrentToken == TOKEN.BOOLEAN_TRUE || CurrentToken == TOKEN.BOOLEAN_FALSE)
      {
        returnValue = new BooleanConstant(CurrentToken == TOKEN.BOOLEAN_TRUE);
        GetNext();
      }
      else
      {
        Console.WriteLine("Illegal Token");
        throw new Exception();
      }
      return returnValue;
    }

      private Stmt ParseAssignmentStatement(ProcedureBuilder ctx)
    {
      string variableName = GetString();
      Symbol s = ctx.TABLE.Get(variableName);
      if (s == null)
      {
        throw new Exception("Variable " + variableName + " is not declared");
      }
      GetNext();
      if (CurrentToken != TOKEN.ASSIGN)
      {
        throw new Exception("Invalid assignment statement");
      }
      GetNext();
      Expression expression = BExpr(ctx);
      if (expression.TypeCheck(ctx.Context) != s.Type)
      {
        throw new Exception("Type mismatch in assignment");
      }
      return new AssignmentStatement(s, expression);
    }

    private Stmt ParseVariableDeclarationStatement(ProcedureBuilder ctx)
    {
      TOKEN token = CurrentToken;
      GetNext();

      TYPE getTypeOfToken(TOKEN tok)
      {
        switch (token)
        {
          case TOKEN.VAR_BOOLEAN:
            return TYPE.BOOL;
          case TOKEN.VAR_NUMBER:
            return TYPE.NUMERIC;
          case TOKEN.VAR_STRING:
            return TYPE.STRING;
          default:
            throw new Exception("Invalid type");
        }
      }

      if (CurrentToken == TOKEN.UNQUOTED_STRING)
      {
        Symbol s = new Symbol();
        s.Name = GetString();
        s.Type = getTypeOfToken(token);
        GetNext();
        if (CurrentToken == TOKEN.SEMI)
        {
          ctx.TABLE.Add(s);
          return new VariableDeclarationStatement(s);
        }
        else
        {
          throw new Exception("; expected");
        }
      }
      else
      {
        throw new Exception("invalid variable declaration");
      }
    }


    //  <printstmt> := print <expr >;
    private Stmt ParsePrintStatement(ProcedureBuilder ctx)
    {
      GetNext();
      Expression a = BExpr(ctx);
      a.TypeCheck(ctx.Context);
      if (CurrentToken != TOKEN.SEMI)
      {
        throw new Exception("; is expected");
      }
      return new PrintStatement(a);
    }

    //  <printlinestmt> := printline <expr >;
    private Stmt ParsePrintLNStatement(ProcedureBuilder ctx)
    {
      GetNext();
      Expression a = BExpr(ctx);
      a.TypeCheck(ctx.Context);
      if (CurrentToken != TOKEN.SEMI)
      {
        throw new Exception("; is expected");
      }
      return new PrintLineStatement(a);
    }

    private Stmt ParseIfStatement(ProcedureBuilder ctx){
      GetNext();
      ArrayList truePart = null;
      ArrayList falsePart = null;
      Expression exp= BExpr(ctx);

      if(ctx.TypeCheck(exp)!= TYPE.BOOL){
        throw new Exception("Expects boolean expression");
      }

      if(CurrentToken != TOKEN.THEN){
        throw new Exception("THEN expected");
      }

      GetNext();
      truePart = StatementList(ctx);
      if(CurrentToken == TOKEN.ENDIF){
        return new IfStatement(exp, truePart, falsePart);
      }

      if(CurrentToken != TOKEN.ELSE){
        throw new Exception("ELSE expected");
      }
      GetNext();
      falsePart = StatementList(ctx);

      if(CurrentToken != TOKEN.ENDIF){
        throw new Exception("ENDIF expected");
      }
      return new IfStatement(exp, truePart, falsePart);
    }

    private Stmt ParseWhileStatement(ProcedureBuilder ctx){
      GetNext();
      Expression exp = BExpr(ctx);
      if(ctx.TypeCheck(exp)!= TYPE.BOOL){
        throw new Exception("Expects boolean expression");
      }
      ArrayList loopBody = StatementList(ctx);
      if(CurrentToken != TOKEN.WEND){
        throw new Exception("WEND expected");
      }
      return new WhileStatment(exp, loopBody);
    }

    private RELATIONAL_OPERATOR GetRelationalOperator(TOKEN token){
      switch(token){
        case TOKEN.EQUALITY : return RELATIONAL_OPERATOR.EQUALITY;
        case TOKEN.GREATER_THAN : return RELATIONAL_OPERATOR.GREATER_THAN;
        case TOKEN.GREATER_THAN_OR_EQUALITY : return RELATIONAL_OPERATOR.GREATER_THAN_OR_EQUALITY;
        case TOKEN.LESS_THAN : return RELATIONAL_OPERATOR.LESS_THAN;
        case TOKEN.LESS_THAN_OR_EQUALITY : return RELATIONAL_OPERATOR.LESS_THAN_OR_EQUALITY;
        case TOKEN.NOTEQUALITY : return RELATIONAL_OPERATOR.NOTEQUALITY;
        default : throw new Exception("Invalid token");
      }
    }
  }
}