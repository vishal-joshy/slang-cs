using System;

namespace SLANG
{
  public class LexicalAnalyzer
  {
    private string _expression;
    private int _index;
    private int _length;
    private double _number;

    public LexicalAnalyzer(string s)
    {
      _expression = s;
      _length = _expression.Length;
      _index = 0;
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
        default:
          {
            if (int.TryParse(_expression[_index].ToString(), out int n))    //Numerics
            {
              string tempString = "";
              while (_index < _length && int.TryParse(_expression[_index].ToString(), out int m))
              {
                tempString = tempString + Convert.ToString(_expression[_index]);
                Console.WriteLine(_number);
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
      return token;
    }

    public double GetNumber()
    {
      return _number;
    }
  }
}
