using System;

namespace SLANG
{
  public class LexicalAnalyzer
  {
    private string _expression;
    private int _index;
    private int _length;
    private double _number;
    private ValueTable[] _keywords;
    public string _lastString = "";

    protected TOKEN CurrentToken;
    protected TOKEN LastToken;

    public LexicalAnalyzer(string s)
    {
      _expression = s;
      _length = _expression.Length;
      _index = 0;

      _keywords = new ValueTable[13];
      _keywords[0] = new ValueTable(TOKEN.PRINT, "PRINT");
      _keywords[1] = new ValueTable(TOKEN.PRINTLN, "PRINTLINE");
      _keywords[2] = new ValueTable(TOKEN.VAR_NUMBER, "NUMERIC");
      _keywords[3] = new ValueTable(TOKEN.VAR_STRING, "STRING");
      _keywords[4] = new ValueTable(TOKEN.VAR_BOOLEAN, "BOOLEAN");
      _keywords[5] = new ValueTable(TOKEN.BOOLEAN_TRUE, "TRUE");
      _keywords[6] = new ValueTable(TOKEN.BOOLEAN_FALSE, "FALSE");
      _keywords[7] = new ValueTable(TOKEN.IF, "IF");
      _keywords[8] = new ValueTable(TOKEN.ELSE, "ELSE");
      _keywords[9] = new ValueTable(TOKEN.ENDIF, "ENDIF");
      _keywords[10] = new ValueTable(TOKEN.WHILE, "WHILE");
      _keywords[11] = new ValueTable(TOKEN.WEND, "WEND");
      _keywords[12] = new ValueTable(TOKEN.THEN, "THEN");
    }

    protected void GetNext()
    {
      LastToken = CurrentToken;
      CurrentToken = GetToken();
    }

    public double GetNumber()=> _number;
    public string GetString()=> _lastString;
    private double ExtractNumber() {
      string tempString = "";
      while (_index < _length && int.TryParse(_expression[_index].ToString(), out int m)){
        tempString = tempString + Convert.ToString(_expression[_index]);
        _index++;
      }
      return Convert.ToDouble(tempString);
    }

    private bool CheckIfWhitespace(char c){
      if(c == ' ' || c == '\t' ||c == '\r' ||c == '\n'){
        return true;
      }
      return false;
    }

    public TOKEN GetToken()
    {
      TOKEN token = TOKEN.ILLEGAL_TOKEN;
      // white space
      while (_index < _length && CheckIfWhitespace(_expression[_index]))
      {
        _index++;
      }

      if (_index == _length) return TOKEN.NULL; //EOL

      switch (_expression[_index])
      {
        case '+':
          token = TOKEN.PLUS;
          _index++;
          break;
        case '-':
          token = TOKEN.MINUS;
          _index++;
          break;
        case '/':
          token = TOKEN.DIV;
          _index++;
          break;
        case '*':
          token = TOKEN.MULT;
          _index++;
          break;
        case '(':
          token = TOKEN.OPAREN;
          _index++;
          break;
        case ')':
          token = TOKEN.CPAREN;
          _index++;
          break;
        case '!':
          if(_expression[_index+1] == '='){
            token = TOKEN.NOTEQUALITY;
            _index = _index + 2;
            break;
          }else{
            token = TOKEN.NOT;
            _index++;
            break;
          }
        case '>':
          if(_expression[_index+1] == '='){
            token = TOKEN.GREATER_THAN_OR_EQUALITY;
            _index = _index + 2;
            break;
          }else{
            token = TOKEN.GREATER_THAN;
            _index++;
            break;
          }
        case '<':
          if(_expression[_index+1] == '='){
            token = TOKEN.LESS_THAN_OR_EQUALITY;
            _index = _index + 2;
            break;
          }else{
            token = TOKEN.LESS_THAN;
            _index++;
            break;
          }
        case '=':
          if(_expression[_index+1] == '='){
            token = TOKEN.EQUALITY;
            _index = _index + 2;
            break;
          }else{
            token = TOKEN.ASSIGN;
            _index++;
            break;
          }
        case '&':
          if(_expression[_index+1] == '&'){
            token = TOKEN.AND;
            _index = _index + 2;
          }
          break;
        case '|':
          if(_expression[_index+1] == '|'){
            token = TOKEN.OR;
            _index = _index + 2;
          }
          break;
        case ';':
          token = TOKEN.SEMI;
          _index++;
          break;
        case '"':
          string temp = "";
          _index++;
          while (_index < _length && _expression[_index] != '"')
          {
            temp += _expression[_index];
            _index++;
          }
          if (_index == _length)
          {
            token = TOKEN.ILLEGAL_TOKEN;
            return token;
          }
          else
          {
            _index++;
            _lastString = temp;
            token = TOKEN.STRING;
            return token;
          }

        default:
          {
            if (char.IsLetter(_expression[_index]))
            {
              String word = Convert.ToString(_expression[_index]);
              _index++;

              while (_index < _length && (char.IsLetterOrDigit(_expression[_index]) || _expression[_index] == '_'))
              {
                word += _expression[_index];
                _index++;
              }

              word = word.ToUpper();

              for (int i = 0; i < this._keywords.Length; ++i)
              {
                if (_keywords[i].Value.CompareTo(word) == 0)
                {
                  return _keywords[i].tok;
                }
              }

              this._lastString = word;
              return TOKEN.UNQUOTED_STRING;

            }
            else if (int.TryParse(_expression[_index].ToString(), out int n)) //Numerics
            {
              this._number = ExtractNumber();
              token = TOKEN.NUMERIC;
            }
            else
            {
              throw new Exception("Illegal token");
            }
            break;
          }
      }
      return token;
    }
  }
}
