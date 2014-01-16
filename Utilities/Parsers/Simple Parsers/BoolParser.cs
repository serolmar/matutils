namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BoolParser<SymbType> : IParse<bool, string, SymbType>, IParse<object, string, SymbType>
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

        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out object value)
        {
            var temp = default(bool);
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
