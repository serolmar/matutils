using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class BoolParser : IParse<bool, string, string>
    {
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out bool value)
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
