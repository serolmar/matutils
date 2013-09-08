namespace Utilities.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class BigIntegerParser<SymbType> : IParse<BigInteger, string, SymbType>, IParse<object, string, SymbType>
    {
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out BigInteger value)
        {
            if (symbolListToParse.Length > 1)
            {
                value = 0;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return BigInteger.TryParse(firstSymbol.SymbolValue, out value);
            }
        }

        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out object value)
        {
            var temp = default(BigInteger);
            if (this.TryParse(symbolListToParse, out temp))
            {
                value = temp;
                return true;
            }
            else
            {
                value = default(object);
                return false;
            }
        }
    }
}
