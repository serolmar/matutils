using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class DecimalParser : IParse<decimal, string, string>
    {
        private NumberStyles numberStyles;

        private IFormatProvider formatProvider;

        public DecimalParser()
            : this(NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat)
        {
        }

        public DecimalParser(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                throw new ExpressionReaderException("Expecting a non null format provider.");
            }

            this.numberStyles = numberStyles;
            this.formatProvider = formatProvider;
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out decimal value)
        {
            if (symbolListToParse.Length > 1)
            {
                value = 0.0M;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return decimal.TryParse(firstSymbol.SymbolValue,
                    this.numberStyles,
                    this.formatProvider,
                    out value);
            }
        }
    }
}
