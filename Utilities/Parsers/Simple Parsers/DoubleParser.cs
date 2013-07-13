using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class DoubleParser : IParse<double, string, string>
    {
        private NumberStyles numberStyles;

        private IFormatProvider formatProvider;

        public DoubleParser()
            : this(NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat)
        {
        }

        public DoubleParser(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                throw new ExpressionReaderException("Expecting a non null format provider.");
            }

            this.numberStyles = numberStyles;
            this.formatProvider = formatProvider;
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out double value)
        {
            if (symbolListToParse.Length > 1)
            {
                value = 0.0;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return double.TryParse(firstSymbol.SymbolValue,
                    this.numberStyles,
                    this.formatProvider,
                    out value);
            }
        }
    }
}
