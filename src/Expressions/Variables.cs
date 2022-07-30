using System.Reflection.Emit;

namespace SLANG
{
    public class Variable : Expression
    {
        private string _name;
        private TYPE_INFO _type;
        public Variable(SYMBOL inf)
        {
            _name = inf.Name;
        }

        public Variable(COMPILATION_CONTEXT cont, string name, double _value)
        {
            SYMBOL s = new SYMBOL(name, TYPE_INFO.NUMERIC);
            s.DoubleValue = _value;
            cont.TABLE.Add(s);
            _name = name;
        }

        public Variable(COMPILATION_CONTEXT cont, string name, bool _value)
        {
            SYMBOL s = new SYMBOL(name, TYPE_INFO.BOOL);
            s.BooleanValue = _value;
            cont.TABLE.Add(s);
            _name = name;
        }

        public Variable(COMPILATION_CONTEXT cont, string name, string _value)
        {
            SYMBOL s = new SYMBOL(name, TYPE_INFO.STRING);
            s.StringValue = _value;
            cont.TABLE.Add(s);
            _name = name;
        }

        public string GetName() => _name;

        public override TYPE_INFO get_type() => _type;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public override SYMBOL accept(CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont)
        {
            if (cont.TABLE == null)
            {
                return TYPE_INFO.ILLEGAL;
            }
            else
            {
                SYMBOL inf = cont.TABLE.Get(_name);
                if (inf != null)
                {
                    _type = inf.Type;
                    return _type;
                }
                return TYPE_INFO.ILLEGAL;
            }
        }
    }
}