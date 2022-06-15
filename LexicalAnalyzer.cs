using System;

namespace SLANG
{
  public struct ValueTable
  {
    public TOKEN tok;          // Token id
    public string Value;       // Token string

    public ValueTable(TOKEN tok, string Value)
    {
      this.tok = tok;
      this.Value = Value;
    }
  }

  public class LexicalAnalyzer
  {
    private string _expression;
    private int _index;
    private int _length;
    private double _number;
    private ValueTable[] _keyword;
    public string _lastString = "";

    protected TOKEN CurrentToken;
    protected TOKEN LastToken;

    public LexicalAnalyzer(string s)
    {
      _expression = s;
      _length = _expression.Length;
      _index = 0;

      _keyword = new ValueTable[7];
      _keyword[0] = new ValueTable(TOKEN.PRINT, "PRINT");
      _keyword[1] = new ValueTable(TOKEN.PRINTLN, "PRINTLINE");
      _keyword[2] = new ValueTable(TOKEN.VAR_NUMBER, "NUMERIC");
      _keyword[3] = new ValueTable(TOKEN.VAR_STRING, "STRING");
      _keyword[4] = new ValueTable(TOKEN.VAR_BOOLEAN, "BOOLEAN");
      _keyword[5] = new ValueTable(TOKEN.BOOLEAN_TRUE, "TRUE");
      _keyword[6] = new ValueTable(TOKEN.BOOLEAN_FALSE, "FALSE");
    }

    protected TOKEN GetNext()
    {
      LastToken = CurrentToken;
      CurrentToken = GetToken();
      return LastToken;
    }

    public TOKEN GetToken()
    {
    re_start:
      TOKEN token = TOKEN.ILLEGAL_TOKEN;
      // white space
      while (_index < _length && (_expression[_index] == ' ' || _expression[_index] == '\t'))
      {
        _index++;
      }

      if (_index == _length) return TOKEN.NULL; //EOL

      switch (_expression[_index])
      {
        case '\n':
        case '\r':
          _index++;
          goto re_start;
        case '+':
          token = TOKEN.PLUS;
          _index++; break;
        case '-':
          token = TOKEN.MINUS;
          _index++; break;
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
        case ';':
          token = TOKEN.SEMI;
          _index++;
          break;
        case '=':
          token = TOKEN.ASSIGN;
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
              String tem = Convert.ToString(_expression[_index]);
              _index++;

              while (_index < _length && (char.IsLetterOrDigit(_expression[_index]) || _expression[_index] == '_'))
              {
                tem += _expression[_index];
                _index++;
              }

              tem = tem.ToUpper();

              for (int i = 0; i < this._keyword.Length; ++i)
              {
                if (_keyword[i].Value.CompareTo(tem) == 0)
                {
                  return _keyword[i].tok;
                }
              }

              this._lastString = tem;
              return TOKEN.UNQUOTED_STRING;

            }
            else if (int.TryParse(_expression[_index].ToString(), out int n)) //Numerics
            {
              string tempString = "";
              while (_index < _length && int.TryParse(_expression[_index].ToString(), out int m)) //loop to get the whole number
              {
                tempString = tempString + Convert.ToString(_expression[_index]);
                _index++;
              }
              token = TOKEN.NUMERIC;
              _number = Convert.ToDouble(tempString);
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

    public double GetNumber()
    {
      return _number;
    }

    public string GetString()
    {
      return _lastString;
    }
  }
}
