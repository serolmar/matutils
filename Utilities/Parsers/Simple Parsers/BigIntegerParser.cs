namespace Utilities.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class BigIntegerParser : IParse<BigInteger, string, string>
    {
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out BigInteger value)
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
    }
}
