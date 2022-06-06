using System;

namespace SLANG
{
  public struct ValueTable
  {
    public TOKEN tok;          // Token id
    public String Value;       // Token string
    public ValueTable(TOKEN tok, String Value)
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
    private ValueTable[] _valueTable = null;
    private string _lastString;

    public LexicalAnalyzer(string s)
    {
      _expression = s;
      _length = _expression.Length;
      _index = 0;

      _valueTable = new ValueTable[2];
      _valueTable[0] = new ValueTable(TOKEN.PRINT, "PRINT");
      _valueTable[1] = new ValueTable(TOKEN.PRINTLN, "PRINTLINE");
    }

    public TOKEN GetToken()
    {
      TOKEN token = TOKEN.ILLEGAL_TOKEN;

      while (_index < _length &&
      (_expression[_index] == ' ' || _expression[_index] == '\t')) //White spaces
      {
        _index++;
      }

      if (_index == _length) return TOKEN.NULL; //EOL

      switch (_expression[_index])
      {
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
        default:
          {
            if (char.IsLetter(_expression[_index]))
            {
              String tem = Convert.ToString(_expression[_index]);
              _index++;
              while (_index < _length && (char.IsLetterOrDigit(_expression[_index]) ||
              _expression[_index] == '_'))
              {
                tem += _expression[_index];
                _index++;
              }
              tem = tem.ToUpper();
              for (int i = 0; i < this._valueTable.Length; ++i)
              {
                if (_valueTable[i].Value.CompareTo(tem) == 0)
                  return _valueTable[i].tok;

              }
              this._lastString = tem;
              return TOKEN.UNQUOTED_STRING;
            }
            else if (int.TryParse(_expression[_index].ToString(), out int n))    //Numerics
            {
              string tempString = "";
              while (_index < _length && int.TryParse(_expression[_index].ToString(), out int m)) //loop to get the whole number
              {
                tempString = tempString + Convert.ToString(_expression[_index]);
                _index++;
              }
              token = TOKEN.DOUBLE;
              _number = Convert.ToDouble(tempString);
            }
            else
            {
              throw new Exception("Illegal token");
            }
            break;
          }
      }
      Console.WriteLine("Token: " + token);
      return token;
    }

    public double GetNumber()
    {
      return _number;
    }
  }
}
