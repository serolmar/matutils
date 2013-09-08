using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public class DecimalParser<SymbType> : IParse<decimal, string, SymbType>, IParse<object, string, SymbType>
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

        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out decimal value)
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

        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out object value)
        {
            var temp = default(decimal);
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
