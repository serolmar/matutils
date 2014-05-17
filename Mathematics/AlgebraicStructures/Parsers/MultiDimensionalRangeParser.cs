namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de matrizes multidimensionais.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

        /// <summary>
        /// Istancia um novo objecto do tipo <see cref="MultiDimensionalRangeParser{T}"/>.
        /// </summary>
        /// <param name="elementsParser">O leitor de elementos.</param>
        /// <exception cref="ArgumentNullException">Se o leitor de elementos for nulo.</exception>
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

        /// <summary>
        /// Experimenta a leitura da matriz multidimensiona.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos que contém uma representação da matriz.</param>
        /// <param name="value">Recebe a leitura da matriz.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out MultiDimensionalRange<T> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.multiDimensionalReader.TryParseRange(arrayReader, this.elementsParser, out value);
        }
    }
}
