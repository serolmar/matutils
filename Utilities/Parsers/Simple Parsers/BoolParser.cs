using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class BoolParser : IParse<bool, string, string>
    {
        public bool Parse(ISymbol<string, string>[] symbolListToParse)
        {
            if (symbolListToParse.Length > 1)
            {
                var errorBuilder = new StringBuilder();
                errorBuilder.Append("Invalid bool:");
                foreach (var symbol in symbolListToParse)
                {
                    errorBuilder.AppendFormat(" {0}", symbol.SymbolValue);
                }

                throw new ExpressionReaderException(errorBuilder.ToString());
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                var parsed = false;
                if (bool.TryParse(firstSymbol.SymbolValue, out parsed))
                {
                    return parsed;
                }
                else
                {
                    throw new ExpressionReaderException(string.Format("Invalid bool: {0}", firstSymbol.SymbolValue));
                }
            }
        }
    }
}
