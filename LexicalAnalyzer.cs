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

            _keywords = new ValueTable[16];
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
            _keywords[13] = new ValueTable(TOKEN.END, "END");
            _keywords[14] = new ValueTable(TOKEN.FUNCTION, "FUNCTION");
            _keywords[15] = new ValueTable(TOKEN.RETURN, "RETURN");
        }

        protected TOKEN GetNext()
        {
            LastToken = CurrentToken;
            CurrentToken = GetToken();
            return CurrentToken;
        }

        public double GetNumber() => _number;
        public string GetString() => _lastString;

        public void Expect(TOKEN expectedToken)
        {
            if (CurrentToken != expectedToken)
            {
                throw new Exception("Missing token , Expected - " + expectedToken);
            }
        }

        public TOKEN GetToken()
        {
            //Skip whitespace
            while (_index < _length && IsWhitespace(_expression[_index]))
            {
                _index++;
            }

            if (_index == _length) return TOKEN.NULL; //EOL

            switch (_expression[_index])
            {
                case ',': _index++; return TOKEN.COMMA;
                case '+': _index++; return TOKEN.PLUS;
                case '-': _index++; return TOKEN.MINUS;
                case '/': _index++; return TOKEN.DIV;
                case '*': _index++; return TOKEN.MULT;
                case '(': _index++; return TOKEN.OPAREN;
                case ')': _index++; return TOKEN.CPAREN;
                case ';': _index++; return TOKEN.SEMI;
                case '!':
                    if (_expression[_index + 1] == '=')
                    {
                        _index = _index + 2;
                        return TOKEN.NOTEQUALITY;
                    }
                    else
                    {
                        _index++;
                        return TOKEN.NOT;
                    }
                case '>':
                    if (_expression[_index + 1] == '=')
                    {
                        _index = _index + 2;
                        return TOKEN.GREATER_THAN_OR_EQUALITY;
                    }
                    else
                    {
                        _index++;
                        return TOKEN.GREATER_THAN;
                    }
                case '<':
                    if (_expression[_index + 1] == '=')
                    {
                        _index = _index + 2;
                        return TOKEN.LESS_THAN_OR_EQUALITY;
                    }
                    else
                    {
                        _index++;
                        return TOKEN.LESS_THAN;
                    }
                case '=':
                    if (_expression[_index + 1] == '=')
                    {
                        _index = _index + 2;
                        return TOKEN.EQUALITY;
                    }
                    else
                    {
                        _index++;
                        return TOKEN.ASSIGN;
                    }
                case '&':
                    if (_expression[_index + 1] == '&')
                    {
                        _index = _index + 2;
                        return TOKEN.AND;
                    }
                    else
                    {
                        throw new Exception("& missing for AND");
                    }
                case '|':
                    if (_expression[_index + 1] == '|')
                    {
                        _index = _index + 2;
                        return TOKEN.OR;
                    }
                    else
                    {
                        throw new Exception("| missing for OR");
                    }
                case '"':
                    this._lastString = ExtractQuotedString();
                    return TOKEN.STRING;
                default:
                    {
                        if (char.IsLetter(_expression[_index]))
                        {
                            string word = ExtractUnquotedString();

                            //check if token is a keyword
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
                        else if (int.TryParse(_expression[_index].ToString(), out int n))
                        {
                            this._number = ExtractNumber();
                            return TOKEN.NUMERIC;

                        }
                        else
                        {
                            throw new Exception("Illegal token");
                        }
                    }
            }
        }

        private string ExtractUnquotedString()
        {
            string result = Convert.ToString(_expression[_index]);
            _index++;
            while (_index < _length && (char.IsLetterOrDigit(_expression[_index]) || _expression[_index] == '_'))
            {
                result += _expression[_index];
                _index++;
            }
            return result.ToUpper();
        }

        private double ExtractNumber()
        {
            string tempString = "";
            while (_index < _length && int.TryParse(_expression[_index].ToString(), out int m))
            {
                tempString = tempString + Convert.ToString(_expression[_index]);
                _index++;
            }
            return Convert.ToDouble(tempString);
        }

        private bool IsWhitespace(char c)
        {
            if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
            {
                return true;
            }
            return false;
        }

        private string ExtractQuotedString()
        {
            string result = "";
            _index++;
            while (_index < _length && _expression[_index] != '"')
            {
                result += _expression[_index];
                _index++;
            }
            if (_index == _length)
            {
                throw new Exception("Illegal Token" + _expression[_index]);
            }
            else
            {
                _index++;
                return result;
            }
        }
    }
}