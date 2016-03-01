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
    /// <typeparam name="T">O tipo de objectos que constituem as entradas do alcance multidimensional.</typeparam>
    public class MultiDimensionalRangeParser<T> :
        IParse<MultiDimensionalRange<T>, string, string>
    {
        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private MultiDimensionalRangeReader<T, string, string> multiDimensionalReader;

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
                this.multiDimensionalReader = new MultiDimensionalRangeReader<T, string, string>(
                    new RangeNoConfigReader<T, string, string>());
                this.elementsParser = elementsParser;
            }
        }

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public MultiDimensionalRange<T> Parse(
            ISymbol<string, string>[] symbolListToParse, 
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var arrayReader = new ArraySymbolReader<string, string>(
                symbolListToParse, 
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            var value = default(MultiDimensionalRange<T>);
            this.multiDimensionalReader.TryParseRange(arrayReader, this.elementsParser, errorLogs, out value);
            return value;
        }
    }
}
