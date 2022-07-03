using System.Reflection.Emit;

namespace SLANG
{
    public class BooleanConstant : Expression
    {
        private SYMBOL _symbol;

        public BooleanConstant(bool value)
        {
            _symbol = new SYMBOL(null, TYPE_INFO.BOOL);
            _symbol.BooleanValue = value;
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont) => _symbol.Type;
        public override TYPE_INFO get_type() => _symbol.Type;

        public SYMBOL GetConstant() => _symbol;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            cont.CodeOutput.Emit(OpCodes.Ldc_I4, (_symbol.BooleanValue) ? 1 : 0);
            return true;
        }
    }

    public class NumericConstant : Expression
    {
        private SYMBOL _symbol;

        public NumericConstant(double value)
        {
            _symbol = new SYMBOL(null, TYPE_INFO.NUMERIC);
            _symbol.DoubleValue = value;
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont) => _symbol.Type;
        public override TYPE_INFO get_type() => _symbol.Type;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public SYMBOL GetConstant() => _symbol;

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            cont.CodeOutput.Emit(OpCodes.Ldc_R8, _symbol.DoubleValue);
            return true;
        }
    }


    public class StringLiteral : Expression
    {
        private SYMBOL _symbol;
        public StringLiteral(string value)
        {
            _symbol = new SYMBOL(null, TYPE_INFO.STRING);
            _symbol.StringValue = value;
        }

        public override TYPE_INFO TypeCheck(COMPILATION_CONTEXT cont) => _symbol.Type;
        public override TYPE_INFO get_type() => _symbol.Type;

        public SYMBOL GetConstant() => _symbol;

        public override SYMBOL accept(RUNTIME_CONTEXT cont, IVisitor v)
        {
            return v.Visit(cont, this);
        }

        public override bool Compile(DNET_EXECUTABLE_GENERATION_CONTEXT cont)
        {
            cont.CodeOutput.Emit(OpCodes.Ldstr, _symbol.StringValue);
            return true;
        }
    }
}