namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerParser<SymbType> : IParse<int, string, SymbType>, IParse<object, string, SymbType>
    {
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out int value)
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

        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out object value)
        {
            var temp = default(int);
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
