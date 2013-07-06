using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class IntegerParser : IParse<int, string, string>
    {
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out int value)
        {
            if (symbolListToParse.Length > 1)
            {
                value = 0;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return int.TryParse(firstSymbol.SymbolValue, out value);
            }
        }
    }
}
