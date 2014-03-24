namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class MultiDimensionalRangeParser<T> : 
        IParse<MultiDimensionalRange<T>, string, string>
    {
        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private MultiDimensionalRangeReader<T, string, string, ISymbol<string,string>[]> multiDimensionalReader;

        /// <summary>
        /// O leitor dos elmentos contidos na matriz multidimensional.
        /// </summary>
        private IParse<T, string, string> elementsParser;

        public MultiDimensionalRangeParser(IParse<T,string,string> elementsParser)
        {
            if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.multiDimensionalReader = new MultiDimensionalRangeReader<T, string, string, ISymbol<string, string>[]>(
                    new RangeNoConfigReader<T, string, string, ISymbol<string, string>[]>());
                this.elementsParser = elementsParser;
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out MultiDimensionalRange<T> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.multiDimensionalReader.TryParseRange(arrayReader, this.elementsParser, out value);
        }
    }
}
