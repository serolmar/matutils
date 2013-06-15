using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class DoubleParser : IParse<double, string, string>
    {

        public DoubleParser()
        {
        }

        public double Parse(ISymbol<string, string>[] symbolListToParse)
        {
            if (symbolListToParse.Length > 1)
            {
                var errorBuilder = new StringBuilder();
                errorBuilder.Append("Invalid double:");
                foreach (var symbol in symbolListToParse)
                {
                    errorBuilder.AppendFormat(" {0}", symbol.SymbolValue);
                }

                throw new ExpressionReaderException(errorBuilder.ToString());
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                var parsed = 0.0;
                if (double.TryParse(firstSymbol.SymbolValue, 
                    NumberStyles.Float, 
                    CultureInfo.InvariantCulture.NumberFormat, 
                    out parsed))
                {
                    return parsed;
                }
                else
                {
                    throw new ExpressionReaderException(string.Format("Invalid double: {0}", firstSymbol.SymbolValue));
                }
            }
        }
    }
}
