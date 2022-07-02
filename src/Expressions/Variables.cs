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

        public Variable(COMPILATION_CONTEXT cont, String name, double _value)
        {
            SYMBOL s = new SYMBOL(name, TYPE_INFO.NUMERIC);
            s.DoubleValue = _value;
            cont.TABLE.Add(s);
            _name = name;
        }

        public Variable(COMPILATION_CONTEXT cont, String name, bool _value)
        {
            SYMBOL s = new SYMBOL(name, TYPE_INFO.BOOL);
            s.BooleanValue = _value;
            cont.TABLE.Add(s);
            _name = name;
        }

        public Variable(COMPILATION_CONTEXT cont, String name, string _value)
        {
            SYMBOL s = new SYMBOL(name, TYPE_INFO.STRING);
            s.StringValue = _value;
            cont.TABLE.Add(s);
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

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

        public override SYMBOL Evaluate(RUNTIME_CONTEXT cont)
        {
            if (cont.TABLE == null)
            {
                return null;
            }
            else
            {
                SYMBOL a = cont.TABLE.Get(_name);
                return a;
            }
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

        public override TYPE_INFO get_type() => _type;

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            SYMBOL info = cont.TABLE.Get(_name);
            LocalBuilder lb = cont.GetLocal(info.loc_position);
            cont.CodeOutput.Emit(OpCodes.Ldloc, lb);
            return true;
        }
    }
}