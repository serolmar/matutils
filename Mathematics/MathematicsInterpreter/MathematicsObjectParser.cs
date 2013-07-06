﻿using System;
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

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out AMathematicsObject mathematicsObject)
        {
            var valueReader = new StringReader(symbolListToParse[0].SymbolValue);
            var symbolReader = new StringSymbolReader(valueReader, true);
            this.IgnoreVoids(symbolReader);

            var readed = symbolReader.Peek();
            if (readed.SymbolType == "eof")
            {
                mathematicsObject = new EmptyMathematicsObject();
                return true;
            }
            else if (readed.SymbolType == "left_parenthesis")
            {
            }
            else if (readed.SymbolType == "left_bracket")
            {
            }
            else if (readed.SymbolType == "integer")
            {
                mathematicsObject = new IntegerMathematicsObject() { Value = int.Parse(symbolListToParse[0].SymbolValue) };
                return true;
            }
            else if (readed.SymbolType == "double")
            {
                mathematicsObject = new DoubleMathematicsObject() { Value = double.Parse(symbolListToParse[0].SymbolValue, CultureInfo.InvariantCulture.NumberFormat) };
                return true;
            }
            else if (readed.SymbolType == "string")
            {
                mathematicsObject = new NameMathematicsObject(symbolListToParse[0].SymbolValue, this.mediator);
                return true;
            }
            else if (readed.SymbolType == "double_quote")
            {
                mathematicsObject = new StringMathematicsObject() { Value = symbolListToParse[0].SymbolValue };
                return true;
            }
            else if (readed.SymbolType == "boolean")
            {
                mathematicsObject = new BooleanMathematicsObject() { Value = bool.Parse(symbolListToParse[0].SymbolValue) };
                return true;
            }
            else
            {
                mathematicsObject = null;
                return false;
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
