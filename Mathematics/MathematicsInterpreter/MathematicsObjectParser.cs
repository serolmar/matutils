using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;
using Utilities.Collections;
using System.Globalization;

namespace Mathematics.MathematicsInterpreter
{
    class MathematicsObjectParser : IParse<AMathematicsObject, string, string>
    {
        private string[] voidSymbols = {"blancks"};

        private MathematicsInterpreterMediator mediator;

        public MathematicsObjectParser(MathematicsInterpreterMediator mediator)
        {
            this.mediator = mediator;
        }

        public AMathematicsObject Parse(ISymbol<string, string>[] symbolListToParse)
        {
            var valueReader = new StringReader(symbolListToParse[0].SymbolValue);
            var symbolReader = new StringSymbolReader(valueReader, true);
            this.IgnoreVoids(symbolReader);

            var readed = symbolReader.Peek();
            if (readed.SymbolType == "eof")
            {
                return new EmptyMathematicsObject();
            }
            else if (readed.SymbolType == "left_parenthesis")
            {
            }
            else if (readed.SymbolType == "left_bracket")
            {
            }
            else if (readed.SymbolType == "integer")
            {
                return new IntegerMathematicsObject() { Value = int.Parse(symbolListToParse[0].SymbolValue) };
            }
            else if (readed.SymbolType == "double")
            {
                return new DoubleMathematicsObject() { Value = double.Parse(symbolListToParse[0].SymbolValue, CultureInfo.InvariantCulture.NumberFormat) };
            }
            else if (readed.SymbolType == "string")
            {
                return new NameMathematicsObject(symbolListToParse[0].SymbolValue, this.mediator);
            }
            else if (readed.SymbolType == "double_quote")
            {
                return new StringMathematicsObject() { Value = symbolListToParse[0].SymbolValue };
            }
            else if (readed.SymbolType == "boolean")
            {
                return new BooleanMathematicsObject() { Value = bool.Parse(symbolListToParse[0].SymbolValue) };
            }
            else
            {
                throw new ExpressionInterpreterException(string.Format("Parse error. Unexpected expression: {0}", symbolListToParse[0].SymbolValue));
            }

            var lispStyleListParser = LispStyleList<AMathematicsObject>.GetParser(this);

            var multidimensionalRangeParser = new MultiDimensionalRangeNoConfigParser<AMathematicsObject, string, string, CharSymbolReader>();
            var multidimensionalRange = multidimensionalRangeParser.ParseRange(symbolReader , this);
            throw new NotImplementedException();
        }

        private void IgnoreVoids(SymbolReader<CharSymbolReader, string, string> symbolReader)
        {
            ISymbol<string, string> symbol = symbolReader.Peek();
            while (this.voidSymbols.Contains(symbol.SymbolType))
            {
                symbolReader.Get();
                symbol = symbolReader.Peek();
            }
        }
    }
}
