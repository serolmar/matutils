namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite realizar a leitura de valores numéricos e nulo caso não seja possível.
    /// </summary>
    public class NullableDoubleParser : IParse<Nullable<double>, string,string>
    {
        private DoubleParser<string> doubleParser = new DoubleParser<string>();

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out Nullable<double> value)
        {
            value = default(Nullable<double>);
            var readed = default(double);
            if (this.doubleParser.TryParse(symbolListToParse, out readed))
            {
                value = readed;
            }

            return true;
        }
    }
}
