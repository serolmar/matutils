using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class BoolParser<SymbType> : IParse<bool, string, SymbType>
    {
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out bool value)
        {
            value = false;
            if (symbolListToParse.Length > 1)
            {
                value = false;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return bool.TryParse(firstSymbol.SymbolValue, out value);
            }
        }
    }
}
